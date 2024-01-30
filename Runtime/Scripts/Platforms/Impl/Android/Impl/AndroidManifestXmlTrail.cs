#if UNITY_ANDROID
using System;
using UnityEngine;

namespace BloodseekerSDK.Android
{
    public sealed class AndroidManifestXmlTrail : IAndroidTrail
    {
        private Looker[] _lookers;

        public AndroidManifestXmlTrail(params Looker[] lookers)
        {
            this._lookers = lookers;
        }

        public AndroidJavaObject AsJavaObject()
        {
            AndroidJavaObject[] lookers = new AndroidJavaObject[_lookers.Length];
            for (int i = 0; i < lookers.Length; ++i)
            {
                lookers[i] = _lookers[i].AsJavaObject();
            }
            var result = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidManifestXmlTrail^"), lookers);
            for (int i = 0; i < lookers.Length; ++i)
            {
                lookers[i].Dispose();
            }
            return result;
        }

        [Serializable]
        public class Looker
        {
            private string[] _nodes;
            private string _attribute;
            private string _value;

            public Looker(string[] nodes, string attribute, string value)
            {
                this._nodes = nodes;
                this._attribute = attribute;
                this._value = value;
            }

            public AndroidJavaObject AsJavaObject()
            {
                AndroidJavaObject obj = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidManifestXmlTrail$Looker^"));
                obj.Call(new SecureString("^setNodes^"), _nodes);
                obj.Call(new SecureString("^setAttribute^"), _attribute);
                obj.Call(new SecureString("^setValue^"), _value);
                return obj;
            }

            public static Looker UnityPlayerActivity()
            {
                return new Looker(
                    nodes:      new[] { "application", "activity" },
                    attribute:  "android:name",
                    value:      "com.unity3d.player.UnityPlayerActivity"
                );
            }
        }
    }
}
#endif
