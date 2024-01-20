#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public sealed class AndroidLibraryTrail : IAndroidTrail
    {
        private string[] _libraryNames;

        public AndroidLibraryTrail(params string[] libraryNames)
        {
            this._libraryNames = libraryNames;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject("com.am1goo.bloodseeker.android.trails.LibraryTrail", _libraryNames);
        }
    }
}
#endif
