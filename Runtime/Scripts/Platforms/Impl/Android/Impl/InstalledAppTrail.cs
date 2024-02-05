#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public class InstalledAppTrail : IAndroidTrail
    {
        private string[] _appNames;

        public InstalledAppTrail(params string[] appNames)
        {
            this._appNames = appNames;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.InstalledAppTrail^"), _appNames);
        }
    }
}
#endif