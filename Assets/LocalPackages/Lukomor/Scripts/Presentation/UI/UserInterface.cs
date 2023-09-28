using System;
using System.Collections.Generic;
using System.Linq;
using WKosArch.Presentation.Common;
using WKosArch.Presentation.Views.Windows;
using UnityEngine;

namespace WKosArch.Presentation
{
    public class UserInterface : MonoBehaviour
    {
        private const string PrefabPath = "[INTERFACE]";

        public event Action<WindowViewModel> WindowOpened;
        public event Action<WindowViewModel> WindowClosed;

        [SerializeField] private UILayerContainer[] _containers;

        public WindowViewModel FocusedWindowViewModel { get; private set; }
        public Camera UICamera { get; private set; }

        private static UserInterface _instance;
        private UISceneConfig _uiSceneConfig;
        private Dictionary<Type, WindowViewModel> _createdWindowViewModelsCache;
        private WindowsStack _windowStack;

        public static UserInterface CreateInstance()
        {
            if (_instance != null)
            {
                Debug.LogWarning($"UserInterface CreateInstance _instance = {_instance}");
                return _instance;
            }

            var prefab = Resources.Load<UserInterface>(PrefabPath);
            _instance = Instantiate(prefab);
            DontDestroyOnLoad(_instance);

            return _instance;
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            UICamera = GetComponentInChildren<Camera>();

            _createdWindowViewModelsCache = new Dictionary<Type, WindowViewModel>();
            _windowStack = new WindowsStack();
        }

        public void Build(UISceneConfig config)
        {
            _uiSceneConfig = config;

            DestroyOldWindows();
            CreateNewWindows();
        }

        public bool TryGetActiveWindowViewModel<T>(out T activeWindowViewModel) where T : WindowViewModel
        {
            activeWindowViewModel = null;

            var type = typeof(T);
            var result = false;

            if (_createdWindowViewModelsCache.TryGetValue(type, out var viewModel))
            {
                if (viewModel.IsActive)
                {
                    activeWindowViewModel = (T)viewModel;

                    result = true;
                }
            }

            return result;
        }

        public IWindowOpenHandler ShowWindow<T>() where T : WindowViewModel
        {
            var windowViewModelType = typeof(T);

            WindowViewModel windowViewModel;

            if (_createdWindowViewModelsCache.TryGetValue(windowViewModelType, out windowViewModel))
            {
                ActivateWindowViewModel(windowViewModel);
            }
            else
            {
                _uiSceneConfig.TryGetPrefab(out T prefab);

                if (prefab == null)
                {
                    Debug.Log($"<color=#FF0000>Couldn't open window ({windowViewModelType}). It doesn't exist in the config of this scene. </color>");
                    return null;
                }

                windowViewModel = CreateWindowViewModel(prefab);

                ActivateWindowViewModel(windowViewModel);
            }

            var handler = new WindowOpenHandler(windowViewModel, this);

            return handler;
        }

        public void SetBackDestination<TWindowViewModel>() where TWindowViewModel : WindowViewModel
        {
            var windowViewModelType = typeof(TWindowViewModel);

            _windowStack.Pop();
            _windowStack.Push(windowViewModelType);
            _windowStack.Push(FocusedWindowViewModel.GetType());
        }

        public void Back(bool hideCurrentWindow = true)
        {
            if (FocusedWindowViewModel.Window is IHomeWindow)
            {
                return;
            }

            if (hideCurrentWindow)
            {
                FocusedWindowViewModel.Window.Hide();
            }

            _windowStack.Pop();

            var windowTypeForRefreshing = _windowStack.Pop();
            var viewModelForRefreshing = _createdWindowViewModelsCache[windowTypeForRefreshing];

            ActivateWindowViewModel(viewModelForRefreshing);
        }

        public Transform GetContainer(UILayer layer)
        {
            return _containers.FirstOrDefault(container => container.layer == layer)?.transform;
        }

        private WindowViewModel CreateWindowViewModel(WindowViewModel prefabWindowViewModel)
        {
            var windowViewModelType = prefabWindowViewModel.GetType();

            if (_createdWindowViewModelsCache.TryGetValue(windowViewModelType, out var windowViewModel))
            {
                return windowViewModel;
            }

            var container = GetContainer(prefabWindowViewModel.WindowSettings.TargetLayer);
            var createdWindowViewModel = Instantiate(prefabWindowViewModel, container);

            _createdWindowViewModelsCache[windowViewModelType] = createdWindowViewModel;

            if (createdWindowViewModel.WindowSettings.OpenWhenCreated)
            {
                ActivateWindowViewModel(createdWindowViewModel);
            }
            else
            {
                createdWindowViewModel.Window.HideInstantly();
            }

            return createdWindowViewModel;
        }

        private void ActivateWindowViewModel(WindowViewModel windowViewModel)
        {
            windowViewModel.Refresh();

            if (!windowViewModel.Window.IsShown)
            {
                windowViewModel.Subscribe();

                var window = windowViewModel.Window;

                window.Show();
                window.Hidden += OnWindowHidden;
                window.Destroyed += OnWindowDestroyed;

                _windowStack.Push(windowViewModel.GetType());

                FocusedWindowViewModel = windowViewModel;

                WindowOpened?.Invoke(FocusedWindowViewModel);
            }
        }

        private void DestroyOldWindows()
        {
            foreach (var createdWindowViewModelItem in _createdWindowViewModelsCache)
            {
                Destroy(createdWindowViewModelItem.Value.gameObject);
            }

            _createdWindowViewModelsCache.Clear();
        }

        private void CreateNewWindows()
        {
            FocusedWindowViewModel = null;

            var prefabsForCreating = _uiSceneConfig.WindowPrefabs;

            foreach (var prefab in prefabsForCreating)
            {
                if (prefab.WindowSettings.IsPreCached)
                {
                    CreateWindowViewModel(prefab);
                }
            }
        }

        private void OnWindowDestroyed(WindowViewModel windowViewModel)
        {
            var window = windowViewModel.Window;

            window.Destroyed -= OnWindowDestroyed;
            window.Hidden -= OnWindowHidden;

            if (!windowViewModel.WindowSettings.IsPreCached)
            {
                var windowViewModelType = windowViewModel.GetType();

                _createdWindowViewModelsCache.Remove(windowViewModelType);
            }
        }

        private void OnWindowHidden(WindowViewModel windowViewModel)
        {
            _windowStack.RemoveLast(windowViewModel.GetType());
            windowViewModel.Unsubscribe();

            var focusedWindowType = _windowStack.GetLast();
            var focusedWindowViewModel = _createdWindowViewModelsCache[focusedWindowType];

            FocusedWindowViewModel = focusedWindowViewModel;

            WindowClosed?.Invoke(windowViewModel);
        }

    }
}