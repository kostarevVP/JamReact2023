using WKosArch.Domain.Features;
using System;

namespace WKosArch.Services.Scenes
{
    public interface ISceneManagementService : IFeature
    {
        bool SceneReadyToStart { get; set; }

        event Action<string> SceneChanged;
        event Action<string> SceneLoaded;
        event Action<string> SceneLoadingStarted;
        event Action<string> SceneStarted;
        event Action<string> SceneUnloaded;

        void LoadScene(int sceneIndex);
        void LoadScene(string sceneName);
        void ReloadScene();
    }
}