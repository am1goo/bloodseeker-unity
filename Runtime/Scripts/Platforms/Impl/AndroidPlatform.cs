#if UNITY_ANDROID
using BloodseekerSDK.Android;
using System;
using System.Collections.Generic;
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

        public Report Seek()
        {
            using (AndroidJavaObject sdk = new AndroidJavaObject("com.am1goo.bloodseeker.Bloodseeker"))
            {
                var trls = new List<AndroidJavaObject>();
                foreach (var trail in _trails)
                {
                    var trl = trail.AsJavaObject();
                    trls.Add(trl);

                    var added = sdk.Call<bool>("addTrail", trl);
#if DEBUG
                    if (!added)
                    {
                        Debug.LogError($"Seek: {trail.GetType()} doens't added");
                    }
#endif
                }

                var report = sdk.Call<AndroidJavaObject>("seek");
                var isSuccess = report.Call<bool>("isSuccess");
                var entries = report.CallArray("getEntries");
                var errors = report.CallArray("getErrors");

                foreach (var trl in trls)
                {
                    trl.Dispose();
                }
                trls.Clear();

                return new Report(isSuccess, entries, errors);
            }
        }
    }
}
#endif
