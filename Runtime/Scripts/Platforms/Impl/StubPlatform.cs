using System;
using System.Threading.Tasks;

namespace BloodseekerSDK
{
    public class StubPlatform : IBloodseekerPlatform
    {
        private static readonly Report _report = Report.Ok();

        public void SetLocalUpdateConfig(LocalUpdateConfig config)
        {
            //do nothing
        }

        public void SetRemoteUpdateConfig(RemoteUpdateConfig config)
        {
            //do nothing
        }

        public bool AddTrail(ITrail trail)
        {
            //do nothing
            return true;
        }

        public Task<Report> Seek()
        {
            return Task.FromResult(_report);
        }
    }
}
