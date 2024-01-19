namespace BloodseekerSDK
{
    public struct Report
    {
        public bool success;
        public string[] results;
        public string[] exceptions;

        public Report(bool success, string[] results, string[] exceptions)
        {
            this.success = success;
            this.results = results;
            this.exceptions = exceptions;
        }
    }
}
