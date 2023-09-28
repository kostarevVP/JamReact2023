using WKosArch.Common.DIContainer;
using UnityEngine;

namespace WKosArch.Presentation.Views.Windows
{
    public class WindowViewModel : ViewModel
    {
        [SerializeField] private WindowSettings _windowSettings;

        public WindowSettings WindowSettings => _windowSettings;
        public IWindow Window {
            get
            {
                if (_window == null)
                {
                    _window = (IWindow) View;
                }

                return _window;
            }
        }

        public UserInterface UI
        {
            get
            {
                if(_userInterface == null)
                {
                    _userInterface = new DIVar<UserInterface>().Value;
                }

                return _userInterface;
            }
        }

        private IWindow _window;
        private UserInterface _userInterface;
    }
}