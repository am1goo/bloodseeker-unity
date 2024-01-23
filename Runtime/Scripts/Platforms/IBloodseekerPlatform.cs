using System.Threading.Tasks;

namespace BloodseekerSDK
{
    public interface IBloodseekerPlatform
    {
        void SetRemoteUpdateConfig(RemoteUpdateConfig config);
        bool AddTrail(ITrail trail);
        Task<Report> Seek();
    }
}
