namespace WKosArch.UIService.Views.Windows
{
    public interface IWindowOpenHandler
    {
        WindowViewModel OpeningWindowViewModel { get; }
        IWindow OpeningWindow { get; }
        void WithBackDestination<TWindowViewModel>() where TWindowViewModel : WindowViewModel;
    }
}