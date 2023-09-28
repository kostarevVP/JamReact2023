using Cysharp.Threading.Tasks.Internal;
using System;

namespace Assets.LocalPackages.WKosArch.Scripts.Common.DIContainer
{
    public interface IDIContainer : IDisposable
    {
        bool IsRoot { get; }
        void Bind<T>(T instance) where T : class;
        T Resolve<T>() where T : class;
    }
}
