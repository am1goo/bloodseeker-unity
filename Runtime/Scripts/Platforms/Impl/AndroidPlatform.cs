#if UNITY_ANDROID
using BloodseekerSDK.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace BloodseekerSDK
{
    public class AndroidPlatform : IBloodseekerPlatform
    {
        private LocalUpdateConfig _localConfig;
        private RemoteUpdateConfig _remoteConfig;
        private List<IAndroidTrail> _trails = new List<IAndroidTrail>();

        public AndroidPlatform()
        {

        }

        public void SetLocalUpdateConfig(LocalUpdateConfig config)
        {
            this._localConfig = config;
        }

        public void SetRemoteUpdateConfig(RemoteUpdateConfig config)
        {
            this._remoteConfig = config;
        }

        public bool AddTrail(ITrail trail)
        {
            if (!(trail is IAndroidTrail androidTrail))
                return false;

            if (_trails.Contains(androidTrail))
                return false;

            _trails.Add(androidTrail);
            return true;
        }

        public async Task<Report> Seek()
        {
            try
            {
                return await SeekLowLevel();
            }
            catch (Exception ex)
            {
                return Report.UnexpectedError(ex);
            }
        }

        private async Task<Report> SeekLowLevel()
        {
            AndroidJavaObject sdkObj;
            try
            {
                sdkObj = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.AndroidBloodseeker^"));
            }
            catch (AndroidJavaException ex)
            {
                return Report.NotInitialized(ex);
            }

            if (sdkObj.IsNull())
                return Report.NotInitialized();

            AndroidJavaObject asyncReportObj;
            try
            {
                asyncReportObj = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.AsyncReport^"));
            }
            catch (AndroidJavaException ex)
            {
                return Report.NotInitialized(ex);
            }

            if (asyncReportObj.IsNull())
                return Report.NotInitialized();

            var exceptions = new List<Exception>();

            try
            {
                if (_localConfig != null)
                {
                    var localConfigBytes = _localConfig.file?.bytes;
                    if (localConfigBytes != null)
                    {
                        using (AndroidJavaObject localConfigObj = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.update.LocalUpdateConfig^")))
                        {
                            localConfigObj.Call(new SecureString("^setFile^"), localConfigBytes);
                            localConfigObj.Call(new SecureString("^setSecretKey^"), _localConfig.secretKey);

                            bool added = sdkObj.Call<bool>(new SecureString("^setLocalUpdateConfig^"), localConfigObj);
                            if (!added)
                                exceptions.Add(new Exception($"{typeof(LocalUpdateConfig)} wasn't sets"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            try
            {
                if (_remoteConfig != null)
                {
                    using (AndroidJavaObject remoteConfigObj = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.update.RemoteUpdateConfig^")))
                    {
                        remoteConfigObj.Call(new SecureString("^setUrl^"), _remoteConfig.url);
                        remoteConfigObj.Call(new SecureString("^setSecretKey^"), _remoteConfig.secretKey);
                        remoteConfigObj.Call(new SecureString("^setCacheTTL^"), _remoteConfig.cacheTTL);

                        if (_remoteConfig.keystore != null)
                        {
                            var keystore = _remoteConfig.keystore;
                            using (AndroidJavaObject keystoreObj = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.update.RemoteUpdateConfig$Keystore^")))
                            {
                                keystoreObj.Call(new SecureString("^setCert^"), keystore.cert.bytes);
                                keystoreObj.Call(new SecureString("^setPwd^"), keystore.pwd);

                                remoteConfigObj.Call(new SecureString("^setKeystore^"), keystoreObj);
                            }
                        }

                        bool added = sdkObj.Call<bool>(new SecureString("^setRemoteUpdateConfig^"), remoteConfigObj);
                        if (!added)
                            exceptions.Add(new Exception($"{typeof(RemoteUpdateConfig)} wasn't sets"));
                    }
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            var trailsObjs = new List<AndroidJavaObject>();
            foreach (var trail in _trails)
            {
                AndroidJavaObject trailObj;
                try
                {
                    trailObj = trail.AsJavaObject();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    continue;
                }

                if (trailObj == null)
                    continue;

                bool added;
                try
                {
                    added = sdkObj.Call<bool>(new SecureString("^addTrail^"), trailObj);
                    if (!added)
                        exceptions.Add(new Exception($"{trail.GetType()} wasn't added"));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    trailObj.Dispose();
                    continue;
                }

                trailsObjs.Add(trailObj);
            }

            sdkObj.Call(new SecureString("^seekAsync^"), asyncReportObj);

            for (; ; )
            {
                var isDone = asyncReportObj.Call<bool>(new SecureString("^isDone^"));
                if (isDone)
                {
                    break;
                }
                else
                {
                    await Task.Delay(10);
                }
            }

            var report = default(Report);
            using (var resultObj = asyncReportObj.Call<AndroidJavaObject>(new SecureString("^getResult^")))
            {
                if (resultObj.IsNull())
                {
                    var exceptionObj = asyncReportObj.Call<AndroidJavaObject>(new SecureString("^getException^"));
                    var exceptionStr = exceptionObj.Call<string>(new SecureString("^toString^"));
                    exceptions.Add(new Exception(exceptionStr));
                    report = Report.UnexpectedError(exceptions);
                }
                else
                {
                    var isSuccess = resultObj.Call<bool>(new SecureString("^isSuccess^"));
                    var evidence = resultObj.CallArray(new SecureString("^getEvidence^"));
                    var errors = resultObj.CallArray(new SecureString("^getErrors^"));
                    
                    var result = isSuccess ? Report.Result.Found : Report.Result.Ok;

                    var errorsExceptions = exceptions.Select(x => x.ToString()).ToArray();
                    var errorsTotal = new string[errorsExceptions.Length + errors.Length];
                    errorsExceptions.CopyTo(errorsTotal, 0);
                    errors.CopyTo(errorsTotal, errorsExceptions.Length);

                    report = new Report(result, evidence, errorsTotal);
                }
            }

            foreach (var trailObj in trailsObjs)
            {
                trailObj.Dispose();
            }
            trailsObjs.Clear();
            asyncReportObj.Dispose();
            sdkObj.Dispose();

            return report;
        }
    }
}
#endif
