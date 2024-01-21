#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public sealed class PackageNameTrail : IAndroidTrail
    {
        private string[] _packageNames;

        public PackageNameTrail(params string[] packageNames)
        {
            this._packageNames = packageNames;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject("com.am1goo.bloodseeker.android.trails.PackageNameTrail", _packageNames);
        }
    }
}
#endif
