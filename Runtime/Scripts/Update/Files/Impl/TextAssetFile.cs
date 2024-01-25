using UnityEngine;

namespace BloodseekerSDK
{
    public class TextAssetFile : IFile
    {
        private TextAsset _textAsset;
        public byte[] bytes
        {
            get
            {
                if (_textAsset == null)
                    return null;

                return _textAsset.bytes;
            }
        }

        public TextAssetFile(TextAsset textAsset)
        {
            this._textAsset = textAsset;
        }
    }
}
