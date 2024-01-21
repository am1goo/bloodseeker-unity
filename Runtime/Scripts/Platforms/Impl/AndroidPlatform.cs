﻿#if UNITY_ANDROID
using BloodseekerSDK.Android;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BloodseekerSDK
{
    public class AndroidPlatform : IBloodseekerPlatform
    {
        private List<IAndroidTrail> _trails = new List<IAndroidTrail>();

        public AndroidPlatform()
        {

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

        public Task<Report> Seek()
        {
            var report = SeekInternal();
            return Task.FromResult(report);
        }

        private Report SeekInternal()
        {
            AndroidJavaObject sdk;
            try
            {
                sdk = new AndroidJavaObject("com.am1goo.bloodseeker.android.Bloodseeker");
            }
            catch (AndroidJavaException ex)
            {
                return Report.NotInitialized(ex);
            }

            if (sdk.IsNull())
                return Report.NotInitialized();

            var trls = new List<AndroidJavaObject>();
            var exceptions = new List<Exception>();
            foreach (var trail in _trails)
            {
                AndroidJavaObject trl;
                try
                {
                    trl = trail.AsJavaObject();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    continue;
                }

                if (trl == null)
                    continue;

                bool added;
                try
                {
                    added = sdk.Call<bool>("addTrail", trl);
                    if (!added)
                        exceptions.Add(new Exception($"{trail.GetType()} wasn't added"));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    trl.Dispose();
                    continue;
                }

                trls.Add(trl);
            }

            var report = sdk.Call<AndroidJavaObject>("seek");
            var isSuccess = report.Call<bool>("isSuccess");
            var evidence = report.CallArray("getEvidence");
            var errors = report.CallArray("getErrors");

            foreach (var trl in trls)
            {
                trl.Dispose();
            }
            trls.Clear();
            sdk.Dispose();

            var result = isSuccess ? Report.Result.Found : Report.Result.Ok;
            return new Report(result, evidence, errors);
        }
    }
}
#endif
