#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public class PathInApkTrail : IAndroidTrail
    {
        private string[] _pathsInApk;

        public PathInApkTrail(params string[] pathsInApk)
        {
            this._pathsInApk = pathsInApk;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject("com.am1goo.bloodseeker.android.trails.PathInApkTrail", _pathsInApk);
        }
    }
}
#endif
