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
            AndroidJavaObject[] lookerObjs = new AndroidJavaObject[_lookers.Length];
            for (int i = 0; i < lookerObjs.Length; ++i)
            {
                Looker looker = _lookers[i];
                AndroidJavaObject lookerObj = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidManifestXmlTrail$Looker^"));
                lookerObj.Call(new SecureString("^setNodes^"), looker.nodes);
                lookerObj.Call(new SecureString("^setAttribute^"), looker.attribute);
                lookerObj.Call(new SecureString("^setValue^"), looker.value);
                lookerObjs[i] = lookerObj;
            }
            var result = new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidManifestXmlTrail^"), lookerObjs);
            for (int i = 0; i < lookerObjs.Length; ++i)
            {
                lookerObjs[i].Dispose();
            }
            return result;
        }

        [Serializable]
        public class Looker
        {
            public string[] nodes;
            public string attribute;
            public string value;

            public Looker(string[] nodes, string attribute, string value)
            {
                this.nodes = nodes;
                this.attribute = attribute;
                this.value = value;
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
