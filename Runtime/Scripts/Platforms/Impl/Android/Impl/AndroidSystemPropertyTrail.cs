#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public class AndroidSystemPropertyTrail : IAndroidTrail
    {
        private SystemProperty[] _systemProperties;

        public AndroidSystemPropertyTrail(params SystemProperty[] systemProperties)
        {
            this._systemProperties = systemProperties;
        }

        public AndroidJavaObject AsJavaObject()
        {
            using (var systemProperties = new AndroidJavaArray(_systemProperties.Length, new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidSystemPropertyTrail$SystemProperty^")))
            {
                for (int i = 0; i < systemProperties.length; ++i)
                {
                    systemProperties[i] = _systemProperties[i].AsJavaObject();
                }
                return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidSystemPropertyTrail^"), systemProperties.AsJavaObject());
            }
        }

        public class SystemProperty
        {
            private string _key;
            private Condition _condition;
            private string _value;

            public SystemProperty(string key) : this(key, default, null)
            {
            }

            public SystemProperty(string key, Condition condition, string value)
            {
                _key = key;
                _condition = condition;
                _value = value;
            }

            public AndroidJavaObject AsJavaObject()
            {
                using (var conditionClass = new AndroidJavaClass("com.am1goo.bloodseeker.Condition"))
                {
                    using (var conditionObj = conditionClass.CallStatic<AndroidJavaObject>("valueOf", (int)_condition))
                    {
                        return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidSystemPropertyTrail$SystemProperty^"), _key, conditionObj, _value);
                    }
                }
            }
        }

        public enum Condition
        {
            Eq = 0,
            NonEq = 1,
        }
    }
}
#endif