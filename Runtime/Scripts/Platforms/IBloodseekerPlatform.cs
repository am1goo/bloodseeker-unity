using System.Threading.Tasks;

namespace BloodseekerSDK
{
    public interface IBloodseekerPlatform
    {
        bool AddTrail(ITrail trail);
        Task<Report> Seek();
    }
}
