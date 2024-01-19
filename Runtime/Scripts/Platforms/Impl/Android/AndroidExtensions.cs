#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public static class AndroidExtensions
    {
        public static string[] CallArray(this AndroidJavaObject obj, string methodName, params object[] args)
        {
            var resObj = obj.Call<AndroidJavaObject>(methodName, args);
            if (resObj == null)
                return null;

            var rawObj = resObj.GetRawObject();
            if (rawObj.ToInt32() == 0)
                return null;

            return AndroidJNIHelper.ConvertFromJNIArray<string[]>(rawObj);
        } 
    }
}
#endif
