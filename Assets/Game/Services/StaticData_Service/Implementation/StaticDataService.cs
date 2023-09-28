using System.Collections.Generic;
using UnityEngine;
using WKosArch.Extentions;
using WKosArch.Services.UIService;

namespace WKosArch.Services.StaticDataServices
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameProgressPath = "NewGameProgressStaticData";
        private const string SceneConfigsFolderPath = "SeneConfigs";

        public GameProgressConfig GameProgressConfig => _gameProgressStaticData;
        public Dictionary<string, UISceneConfig> SceneConfigsMap => _sceneConfigsMap;
        public bool IsReady => _isReady;

        private IAssetProviderService _assetProviderService;

        private GameProgressConfig _gameProgressStaticData;
        private Dictionary<string, UISceneConfig> _sceneConfigsMap = new Dictionary<string, UISceneConfig>();

        private bool _isReady;

        public StaticDataService(IAssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;

            LoadGameProgressConfig();
            LoadSceneConfigs();

            _isReady = true;
        }

        public void Dispose()
        {
            Clear();
        }

        private void LoadGameProgressConfig()
        {
            _gameProgressStaticData = _assetProviderService.Load<GameProgressConfig>(GameProgressPath);
        }

        private void LoadSceneConfigs()
        {
            var scenesConfigs = _assetProviderService.LoadAll<UISceneConfig>(SceneConfigsFolderPath);

            foreach (var sceneConfigs in scenesConfigs)
            {
                foreach (var scene in sceneConfigs.SceneName)
                {
                    if (!_sceneConfigsMap.ContainsKey(scene))
                        _sceneConfigsMap[scene] = sceneConfigs;
                    else
                        Log.PrintError($"You try add Scene WindowConfigs that is have another WindowConfig");
                }
            }
        }

        private void Clear()
        {
            _gameProgressStaticData = null;
            _sceneConfigsMap.Clear();
        }
    }
}
