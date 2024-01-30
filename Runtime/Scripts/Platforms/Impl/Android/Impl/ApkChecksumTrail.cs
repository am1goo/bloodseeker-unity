#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public class ApkChecksumTrail : IAndroidTrail
    {
        private long _checksum;

        public ApkChecksumTrail(long checksum)
        {
            this._checksum = checksum;
        }

        public AndroidJavaObject AsJavaObject()
        {
            return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.ApkChecksumTrail^"), _checksum);
        }
    }
}
#endif
