﻿using Assets.LocalPackages.WKosArch.Scripts.Common.DIContainer;
using UnityEngine;

namespace WKosArch.UIService.Views
{
    public abstract class ViewModel : MonoBehaviour
    {
        public IView View { get; private set; }
        public bool IsActive => View.IsActive;
        public IDIContainer Container => _container;
        
        private IDIContainer _container;

        public void InjectDI(IDIContainer container)
        {
            _container = container;
        }

        private void Awake()
        {
            View = GetComponent<IView>();
            
            AwakeInternal();
        }

        protected virtual void AwakeInternal() { }

        public void Refresh()
        {
            RefreshInternal();
            
            View.Refresh();
        }

        public void Subscribe()
        {
            SubscribeInternal();
            
            View.Subscribe();
        }

        public void Unsubscribe()
        {
            UnsubscribeInternal();
            
            View.Unsubscribe();
        }

        protected virtual void RefreshInternal() { }
        protected virtual void SubscribeInternal() { }
        protected virtual void UnsubscribeInternal() { }
    }
}