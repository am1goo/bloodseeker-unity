namespace BloodseekerSDK
{
    public interface IBloodseekerPlatform
    {
        bool AddTrail(ITrail trail);
        Report Seek();
    }
}
