﻿using System.Collections.Generic;
using System.Linq;
using Assets.LocalPackages.WKosArch.Scripts.Common.DIContainer;
using Cysharp.Threading.Tasks;
using WKosArch.Common.Utils.Async;
using WKosArch.Domain.Features;
using UnityEngine;
using System;

namespace WKosArch.Domain.Contexts
{
    public abstract class Context : MonoBehaviour, IContext
    {
        public bool IsReady { get; private set; }

        public IDIContainer Container => _container ??= CreateLocalContainer();


        [SerializeField] private FeatureInstaller[] _serviceFeaturesInstallers;
        [SerializeField] private FeatureInstaller[] _gameplayFeatureInstallers;
        
		private List<IFeature> _cachedServiceFeatures;
		private List<IFeature> _cachedGameplayFeatures;

        private IDIContainer _container;

		#region Unity Lifecycle

		private void Awake()
		{
			_cachedServiceFeatures = new List<IFeature>();
			_cachedGameplayFeatures = new List<IFeature>();
		}
		
		private void OnDestroy()
		{
            Container.Dispose();
            Destroy();
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			foreach (IFeature serviceFeature in _cachedServiceFeatures)
			{
				if(serviceFeature is IFocusPauseFeature focusFeature)
                    focusFeature.OnApplicationFocus(hasFocus);
			}

			foreach (IFeature gameplayFeature in _cachedGameplayFeatures)
			{
				if(gameplayFeature is IFocusPauseFeature focusFeature)
                    focusFeature.OnApplicationFocus(hasFocus);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			foreach (IFeature serviceFeature in _cachedServiceFeatures)
			{
				if(serviceFeature is IFocusPauseFeature pauseFeature)
                    pauseFeature.OnApplicationPause(pauseStatus);
			}

			foreach (IFeature gameplayFeature in _cachedGameplayFeatures)
			{
				if(gameplayFeature is IFocusPauseFeature pauseFeature)
                    pauseFeature.OnApplicationPause(pauseStatus);
			}
		}

		#endregion

		#region Lifecycle
		
		public virtual async UniTask InitializeAsync()
		{
			InstallServiceFeatures();
			InstallGameplayFeatures();

			InitializeServiceFeatures();
			InitializeGameplayFeatures();

			await WaitInitializationComplete();
		}

		public void Destroy()
		{
			DestroyServiceFeatures();
			DestroyGameplayFeatures();
		}

        #endregion

        protected abstract IDIContainer CreateLocalContainer();

        private void InstallServiceFeatures()
		{
			foreach (var serviceFeatureInstaller in _serviceFeaturesInstallers)
			{
				var createdFeature = serviceFeatureInstaller.Create(Container);

				_cachedServiceFeatures.Add(createdFeature);
			}
		}

		private void InstallGameplayFeatures()
		{
			foreach (var gameplayFeatureInstaller in _gameplayFeatureInstallers)
			{
				var createdFeature = gameplayFeatureInstaller.Create(Container);

				_cachedGameplayFeatures.Add(createdFeature);	
			}
		}

		private void InitializeServiceFeatures()
		{
			foreach (IFeature serviceFeature in _cachedServiceFeatures)
			{
				if(serviceFeature is IAsyncFeature asyncFeature)
                    asyncFeature.InitializeAsync();
			}
		}

		private void InitializeGameplayFeatures()
		{
			foreach (IFeature gameplayFeature in _cachedGameplayFeatures)
			{
				if(gameplayFeature is IAsyncFeature asyncFeature)
                    asyncFeature.InitializeAsync();
			}
		}

		private async UniTask WaitInitializationComplete()
		{
			await UnityAwaiters.WaitUntil(() =>
				_cachedGameplayFeatures.All(feature => feature.IsReady)
				&& _cachedServiceFeatures.All(service => service.IsReady));

			IsReady = true;
		}

		private void DestroyServiceFeatures()
		{
			foreach (IFeature serviceFeature in _cachedServiceFeatures)
			{
				if(serviceFeature is IAsyncFeature asyncFeature)
                    asyncFeature.DestroyAsync();
				if(serviceFeature is IDisposable disposable)
					disposable.Dispose();
			}
			
			_cachedServiceFeatures.Clear();
			
			foreach (var serviceFeaturesInstaller in _serviceFeaturesInstallers)
			{
				serviceFeaturesInstaller.Dispose();
			}
		}

		private void DestroyGameplayFeatures()
		{
			foreach (IFeature gameplayFeature in _cachedGameplayFeatures)
			{
				if(gameplayFeature is IAsyncFeature asyncFeature)
                    asyncFeature.DestroyAsync();
				if(gameplayFeature is IDisposable disposable)
					disposable.Dispose();
			}
			
			_cachedGameplayFeatures.Clear();

			foreach (var featureInstaller in _gameplayFeatureInstallers)
			{
				featureInstaller.Dispose();
			}
		}


    }
}