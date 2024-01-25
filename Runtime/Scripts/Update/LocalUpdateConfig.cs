using System.IO;
using UnityEngine;

namespace BloodseekerSDK
{
    public class LocalUpdateConfig
    {
        public IFile file;
        public string secretKey;

        public interface IFile
        {
            byte[] bytes { get; }
        }

        public class RawFile : IFile
        {
            public byte[] bytes { get; private set; }

            public RawFile(byte[] bytes)
            {
                this.bytes = bytes;
            }
        }

        public class File : IFile
        {
            private FileInfo _fileInfo;
            private bool _loaded = false;

            private byte[] _bytes;
            public byte[] bytes
            {
                get
                {
                    if (_loaded)
                        return _bytes;

                    if (!_fileInfo.Exists)
                    {
                        _loaded = true;
                        return _bytes;
                    }

                    using (var stream = _fileInfo.OpenRead())
                    {
                        using (var reader = new MemoryStream())
                        {
                            stream.CopyTo(reader);
                            _bytes = reader.ToArray();
                            _loaded = true;
                        }
                    }
                    return _bytes;
                }
            }

            public File(string path) : this(new FileInfo(path))
            {
            }

            public File(FileInfo fileInfo)
            {
                this._fileInfo = fileInfo;
            }
        }

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
}
