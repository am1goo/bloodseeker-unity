using System.Threading.Tasks;

namespace BloodseekerSDK
{
    public interface IBloodseekerPlatform
    {
        void SetLocalUpdateConfig(LocalUpdateConfig config);
        void SetRemoteUpdateConfig(RemoteUpdateConfig config);
        bool AddTrail(ITrail trail);
        Task<Report> Seek();
    }
}
