using WKosArch.Domain.Features;
using WKosArch.Services.Scenes;

namespace WKosArch.Features.LoadLevelFeature
{
	public interface ILoadLevelFeature : IFeature
	{
		void LoadGameLevelEnviroment(ISceneManagementService _sceneManagementService);
	} 
}