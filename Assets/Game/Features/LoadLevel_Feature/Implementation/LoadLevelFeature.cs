using WKosArch.Services.Scenes;
using WKosArch.Services.UIService;

namespace WKosArch.Features.LoadLevelFeature
{
    public class LoadLevelFeature : ILoadLevelFeature
    {
        private readonly UserInterface _ui;

        private bool _isReady;

        public bool IsReady => _isReady;


        public LoadLevelFeature(UserInterface ui)
        {
            _ui = ui;

            _isReady = true;
        }

        public void LoadGameLevelEnviroment(ISceneManagementService _sceneManagementService)
        {
            ShowSettingButton();

            _sceneManagementService.SceneReadyToStart = true;
        }

        private void ShowSettingButton()
        {
            _ui.ShowWindow<SettingButtonViewModel>();
        }
    }
}