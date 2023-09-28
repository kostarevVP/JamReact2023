using Assets.LocalPackages.WKosArch.Scripts.Common.DIContainer;
using WKosArch.Domain.Contexts;
using WKosArch.Domain.Features;
using WKosArch.Extentions;
using WKosArch.Services.Scenes;
using UnityEngine;
using WKosArch.Services.UIService;

namespace WKosArch.Features.LoadLevelFeature
{
    [CreateAssetMenu(fileName = "LoadLevelFeature_Installer", menuName = "Game/Installers/LoadLevelFeature_Installer")]
    public class LoadLevelFeature_Installer : FeatureInstaller
    {
        private ILoadLevelFeature _feature;
        private ISceneManagementService _sceneManagementService;

        public override IFeature Create(IDIContainer container)
        {
            _sceneManagementService = container.Resolve<ISceneManagementService>();
            var ui = container.Resolve<UserInterface>();

            _feature = new LoadLevelFeature(ui);

            container.Bind(_feature);

            _sceneManagementService.SceneLoaded += SceneLoaded;

            Log.PrintColor($"[ILoadLevelFeature] Create and Bind", Color.cyan);
            return _feature;
        }

        public override void Dispose()
        {
            _sceneManagementService.SceneLoaded -= SceneLoaded;
        }

        private void SceneLoaded(string sceneName)
        {
            _feature.LoadGameLevelEnviroment(_sceneManagementService);
        }
    }
}
