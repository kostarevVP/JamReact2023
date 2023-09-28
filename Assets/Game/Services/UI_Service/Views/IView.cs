namespace WKosArch.UIService.Views
{
    public interface IView<out TViewModel> : IView where TViewModel : ViewModel
    {
        TViewModel ViewModel { get; }
    }

    public interface IView
    {
        bool IsActive { get; }
        void Refresh();
        void Subscribe();
        void Unsubscribe();
    }
}