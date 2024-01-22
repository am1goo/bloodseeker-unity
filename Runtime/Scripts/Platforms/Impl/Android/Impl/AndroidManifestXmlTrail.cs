#if UNITY_ANDROID
using System;
using System.Linq;
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
            var strs = _lookers.Select(looker => JsonUtility.ToJson(looker)).ToArray();
            return new AndroidJavaObject(new SecureString("^com.am1goo.bloodseeker.android.trails.AndroidManifestXmlTrail^"), strs);
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
