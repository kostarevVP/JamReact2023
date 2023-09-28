using WKosArch.Domain.Features;

namespace WKosArch.Services.UIService
{
    public interface IUIService : IFeature
    {
        UserInterface UI { get; }
    }
}