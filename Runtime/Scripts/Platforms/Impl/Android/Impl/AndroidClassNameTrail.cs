#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public sealed class AndroidClassNameTrail : IAndroidTrail
    {
        private string[] _classNames;

        public AndroidClassNameTrail(string[] classNames)
        {
            this._classNames = classNames;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject("com.am1goo.bloodseeker.trails.ClassNameTrail", _classNames);
        }
    }
}
#endif
