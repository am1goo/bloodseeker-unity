namespace BloodseekerSDK
{
    public class StubPlatform : IBloodseekerPlatform
    {
        private static readonly Report _report = new Report(false, null, null);

        public bool AddTrail(ITrail trail)
        {
            //do nothing
            return true;
        }

        public Report Seek()
        {
            return _report;
        }
    }
}
