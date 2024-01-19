#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public interface IAndroidTrail : ITrail
    {
        AndroidJavaObject AsJavaObject();
    }
}
#endif
