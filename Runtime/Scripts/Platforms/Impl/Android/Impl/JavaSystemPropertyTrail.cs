#if UNITY_ANDROID
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public class JavaSystemPropertyTrail : IAndroidTrail
    {
        private SystemProperty[] _systemProperties;

        public JavaSystemPropertyTrail(params SystemProperty[] systemProperties)
        {
            this._systemProperties = systemProperties;
        }

        public AndroidJavaObject AsJavaObject()
        {
            using (var systemProperties = new AndroidJavaArray(_systemProperties.Length, new SecureString("^com.am1goo.bloodseeker.trails.JavaSystemPropertyTrail$SystemProperty^")))
            {
                for (int i = 0; i < systemProperties.length; ++i)
                {
                    systemProperties[i] = _systemProperties[i].AsJavaObject();
                }
                return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.trails.JavaSystemPropertyTrail^"), systemProperties.AsJavaObject());
            }
        }

        public class SystemProperty
        {
            private string _keyRegex;
            private Condition _condition;
            private string _valueRegex;

            public SystemProperty(string keyRegex) : this(keyRegex, default, null)
            {
            }

            public SystemProperty(string keyRegex, Condition condition, string valueRegex)
            {
                _keyRegex = keyRegex;
                _condition = condition;
                _valueRegex = valueRegex;
            }

            public AndroidJavaObject AsJavaObject()
            {
                using (var conditionClass = new AndroidJavaClass("com.am1goo.bloodseeker.Condition"))
                {
                    using (var conditionObj = conditionClass.CallStatic<AndroidJavaObject>("valueOf", (int)_condition))
                    {
                        return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.trails.JavaSystemPropertyTrail$SystemProperty^"), _keyRegex, conditionObj, _valueRegex);
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