#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public sealed class LibraryTrail : IAndroidTrail
    {
        private string[] _libraryNames;

        public LibraryTrail(params string[] libraryNames)
        {
            this._libraryNames = libraryNames;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.trails.LibraryTrail^"), _libraryNames);
        }
    }
}
#endif
