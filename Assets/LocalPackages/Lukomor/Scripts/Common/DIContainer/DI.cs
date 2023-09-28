using Assets.LocalPackages.WKosArch.Scripts.Common.DIContainer;

namespace WKosArch.Common.DIContainer
{
    public static class DI
    {
        private static IDIContainer _dIcontainer = null;

        public static void AddDIContainer(IDIContainer dIcontainer)
        {
            _dIcontainer = dIcontainer;
        }

        public static TResolve GetResolve<TResolve>() where TResolve : class
        {
            return _dIcontainer.Resolve<TResolve>();
        }

        public static void Bind<TResolve>(TResolve instance) where TResolve : class
        {
            _dIcontainer?.Bind(instance);
        }
    }
}