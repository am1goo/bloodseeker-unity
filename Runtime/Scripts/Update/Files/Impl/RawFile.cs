namespace BloodseekerSDK
{
    public class RawFile : IFile
    {
        public byte[] bytes { get; private set; }

        public RawFile(byte[] bytes)
        {
            this.bytes = bytes;
        }
    }
}
