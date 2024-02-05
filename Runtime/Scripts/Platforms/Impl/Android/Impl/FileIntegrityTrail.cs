#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public class FileIntegrityTrail : IAndroidTrail
    {
        private File[] _filesInApk;

        public FileIntegrityTrail(params File[] filesInApk)
        {
            this._filesInApk = filesInApk;
        }

        public AndroidJavaObject AsJavaObject()
        {
            using (var filesInApk = new AndroidJavaArray(_filesInApk.Length, new SecureString("^com.am1goo.bloodseeker.android.trails.FileIntegrityTrail$FileInApk^")))
            {
                for (int i = 0; i < filesInApk.length; ++i)
                {
                    filesInApk[i] = _filesInApk[i].AsJavaObject();
                }
                return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.FileIntegrityTrail^"), filesInApk.AsJavaObject());
            }
        }

        public class File
        {
            private string _pathInApk;
            private long _checksum;

            public File(string pathInApk, long checksum)
            {
                this._pathInApk = pathInApk;
                this._checksum = checksum;
            }

            public AndroidJavaObject AsJavaObject()
            {
                return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.FileIntegrityTrail$FileInApk^"), _pathInApk, _checksum);
            }
        }
    }
}
#endif
