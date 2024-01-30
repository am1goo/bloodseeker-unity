#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public static class AndroidExtensions
    {
        public static bool IsNull(this AndroidJavaObject obj)
        {
            if (obj == null)
                return true;

            var raw = obj.GetRawObject();
            return raw.ToInt32() == 0;
        }

        public static string GetJavaClass(this AndroidJavaObject obj)
        {
            using (var classObj = obj.Call<AndroidJavaObject>("getClass"))
            {
                return classObj.Call<string>("getName");
            }
        }

        public static string[] CallArray(this AndroidJavaObject obj, string methodName, params object[] args)
        {
            var resObj = obj.Call<AndroidJavaObject>(methodName, args);
            if (resObj == null)
                return null;

            if (resObj.IsNull())
                return null;

            var rawObj = resObj.GetRawObject();
            return AndroidJNIHelper.ConvertFromJNIArray<string[]>(rawObj);
        }
    }
}
#endif
