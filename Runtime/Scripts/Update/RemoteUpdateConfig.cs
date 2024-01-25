namespace BloodseekerSDK
{
    public class RemoteUpdateConfig
    {
        public string url;
        public string secretKey;
        public long cacheTTL;
        public Keystore keystore;

        public class Keystore
        {
            public IFile cert;
            public string pwd;
        }
    }
}
