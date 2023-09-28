using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace WKosArch.Services.Scenes
{
    public class SceneManagementService : ISceneManagementService
    {
        public event Action<string> SceneUnloaded;
        public event Action<string> SceneLoadingStarted;
        public event Action<string> SceneChanged;
        public event Action<string> SceneLoaded;
        public event Action<string> SceneStarted;

        private ILoadingScreen _loadingScreen;

        private string _currentSceneName;
        private bool _isReady;

        public bool SceneReadyToStart { get; set; }

        public bool IsReady => _isReady;

        public SceneManagementService(ILoadingScreen loadingScreen = null)
        {
            _loadingScreen = loadingScreen;

            SceneManager.sceneLoaded += (scene, _) => SceneLoaded?.Invoke(scene.name);
            SceneManager.sceneUnloaded += scene => SceneUnloaded?.Invoke(scene.name);
        }

        public void LoadScene(string sceneName)
        {
            LoadSceneAsync(sceneName);
        }

        public void ReloadScene()
        {
            LoadScene(_currentSceneName);
        }

        public void LoadScene(int sceneIndex)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            var lastSlash = path.LastIndexOf('/');
            var nameWithExtension = path.Substring(lastSlash + 1);
            var lastDot = nameWithExtension.LastIndexOf('.');
            var sceneName = nameWithExtension.Substring(0, lastDot);

            LoadScene(sceneName);
        }

        private async void LoadSceneAsync(string sceneName)
        {
            SceneLoadingStarted?.Invoke(sceneName);

            if (_loadingScreen != null)
            {
                await ShowAnimation(_loadingScreen.Show);
            }

            var async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (async.progress < 0.9f)
            {
                await UniTask.Yield();
            }

            async.allowSceneActivation = true;

            SceneChanged?.Invoke(sceneName);

            _currentSceneName = sceneName;


            if (SceneReadyToStart)
            {
                await UniTask.Yield();
            }


            if (_loadingScreen != null)
            {
                await ShowAnimation(_loadingScreen.Hide);
            }

            SceneStarted?.Invoke(sceneName);
        }

        private static async UniTask ShowAnimation(Action<Action> method)
        {
            var isCompleted = false;

            void OnComplete()
            {
                isCompleted = true;
            }

            method(OnComplete);

            while (!isCompleted)
            {
                await UniTask.Yield();
            }
        }

        public UniTask InitializeAsync()
        {
            _isReady = true;
            return UniTask.CompletedTask;
        }

        public UniTask DestroyAsync() =>
            UniTask.CompletedTask;

        public void OnApplicationFocus(bool hasFocus) { }

        public void OnApplicationPause(bool pauseStatus) { }
    }
}