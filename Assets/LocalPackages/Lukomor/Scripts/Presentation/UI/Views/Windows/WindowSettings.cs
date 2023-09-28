using System;
using WKosArch.Presentation.Common;

namespace WKosArch.Presentation.Views.Windows
{
    [Serializable]
    public struct WindowSettings
    {
        public UILayer TargetLayer;
        public bool IsPreCached;
        public bool OpenWhenCreated;
    }
}