#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public sealed class ClassNameTrail : IAndroidTrail
    {
        private string[] _classNames;

        public ClassNameTrail(params string[] classNames)
        {
            this._classNames = classNames;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.trails.ClassNameTrail^"), _classNames);
        }
    }
}
#endif
