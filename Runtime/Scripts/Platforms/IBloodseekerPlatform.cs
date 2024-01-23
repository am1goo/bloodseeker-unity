using System;
using System.Threading.Tasks;

namespace BloodseekerSDK
{
    public interface IBloodseekerPlatform
    {
        void SetUpdateUrl(Uri uri);
        bool AddTrail(ITrail trail);
        Task<Report> Seek();
    }
}
