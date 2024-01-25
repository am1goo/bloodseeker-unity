using System.IO;

namespace BloodseekerSDK
{
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
}
