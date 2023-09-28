using Cysharp.Threading.Tasks;
using System;

namespace WKosArch.Presentation.Views.Windows {
	public interface IWindow<out TWindowViewModel> : IWindow, IView<TWindowViewModel>
		where TWindowViewModel : WindowViewModel { }

	public interface IWindow : IView
	{
		event Action<WindowViewModel> Hidden;
		event Action<WindowViewModel> Destroyed;
		event Action<bool> BlockInteractingRequested;
		
		bool IsShown { get; }

        UniTask<IWindow> Show();
        UniTask<IWindow> Hide(bool forced = false);
		IWindow HideInstantly();
	}
}