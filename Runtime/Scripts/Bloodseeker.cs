using System;
using System.Collections.Generic;

namespace BloodseekerSDK
{
    public static class Bloodseeker
    {
        private static readonly IBloodseekerPlatform _platform;

        static Bloodseeker()
        {
#if UNITY_ANDROID
            _platform = new AndroidPlatform();
#else
            _platform = new StubPlatform();
#endif
        }

        public static bool AddTrail(ITrail trail)
        {
            if (trail == null)
                return false;

            return _platform.AddTrail(trail);
        }

        public static Report Seek()
        {
            return _platform.Seek();
        }
    }
}
