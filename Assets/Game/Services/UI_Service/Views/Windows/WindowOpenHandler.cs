using WKosArch.Services.UIService;

namespace WKosArch.UIService.Views.Windows
{
    public class WindowOpenHandler : IWindowOpenHandler
    {
        public WindowViewModel OpeningWindowViewModel { get; }
        public IWindow OpeningWindow => OpeningWindowViewModel.Window;

        private UserInterface _ui;
        
        public WindowOpenHandler(WindowViewModel windowViewModel, UserInterface ui)
        {
            OpeningWindowViewModel = windowViewModel;
            _ui = ui;
        }

        public void WithBackDestination<TWindowViewModel>() where TWindowViewModel : WindowViewModel
        {
            _ui.SetBackDestination<TWindowViewModel>();
        }
    }
}