using Assets.LocalPackages.WKosArch.Scripts.Common.DIContainer;
using System;
using WKosArch.Services.Scenes;
using WKosArch.Services.StaticDataServices;

namespace WKosArch.Services.UIService
{
    public class UIService : IUIService, IDisposable
    {
        private readonly ISceneManagementService _sceneManagementService;
        private readonly IStaticDataService _staticDataService;

        public UserInterface UI { get; private set; }
        public bool IsReady => _isReady;

        private bool _isReady;

        public UIService(IStaticDataService staticDataService,
            ISceneManagementService sceneManagementService,
            IDIContainer container)
        {
            _staticDataService = staticDataService;
            _sceneManagementService = sceneManagementService;

            UI = UserInterface.CreateInstance(container);

            _sceneManagementService.SceneLoaded += SceneLoaded;
        }

        public void Dispose()
        {
            _sceneManagementService.SceneLoaded -= SceneLoaded;
        }

        private void SceneLoaded(string sceneName)
        {
            var config = _staticDataService.SceneConfigsMap[sceneName];

            UI.Build(config);
        }
    }
}