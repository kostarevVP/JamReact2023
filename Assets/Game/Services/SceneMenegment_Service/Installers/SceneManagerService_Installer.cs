using Cysharp.Threading.Tasks;
using WKosArch.Domain.Contexts;
using WKosArch.Domain.Features;
using WKosArch.Extentions;
using UnityEngine;
using Assets.LocalPackages.WKosArch.Scripts.Common.DIContainer;

namespace WKosArch.Services.Scenes
{
    [CreateAssetMenu(fileName = "SceneManagerService_Installer", menuName = "Game/Installers/SceneManagerService_Installer")]
    public class SceneManagerService_Installer : FeatureInstaller
    {
        [SerializeField] private GameObject _loadingScreenPrefab;

        private ProjectContext _projectContext;
        private ISceneManagementService _sceneManagementService;

        public override IFeature Create(IDIContainer container)
        {
            _projectContext = container.Resolve<ProjectContext>();

            ILoadingScreen loadingScreen = IsntatiateLoadingScreen();

            _sceneManagementService = new SceneManagementService(loadingScreen);


            _sceneManagementService.SceneChanged += LoadSceneContext;

            container.Bind(_sceneManagementService);

            Log.PrintColor($"[SceneManagementService] Create and Bind", Color.cyan);
            return _sceneManagementService;
        }


        public override void Dispose()
        {
            _sceneManagementService.SceneChanged -= LoadSceneContext;
        }

        private async void LoadSceneContext(string sceneName) => 
            await LoadContext(sceneName);

        private ILoadingScreen IsntatiateLoadingScreen()
        {
            ILoadingScreen loadingScreen = default;

            if (_loadingScreenPrefab != null)
            {
                var loadingScreenGo = Instantiate(_loadingScreenPrefab);
                loadingScreen = loadingScreenGo.GetComponent<ILoadingScreen>();

                DontDestroyOnLoad(loadingScreenGo);
            }

            return loadingScreen;
        }

        private async UniTask<SceneContext> LoadContext(string sceneName)
        {
            SceneContext sceneContext = _projectContext.GetSceneContext(sceneName);

            if (sceneContext != null)
            {
                await sceneContext.InitializeAsync();
            }

            return sceneContext;
        }

        private void OnValidate()
        {
            if (_loadingScreenPrefab != null && _loadingScreenPrefab.GetComponent<ILoadingScreen>() == null)
            {
                Log.PrintWarning($"SceneManagerInstaller doesn't have any ILoadingScreen component.");
            }
        }
    }
}