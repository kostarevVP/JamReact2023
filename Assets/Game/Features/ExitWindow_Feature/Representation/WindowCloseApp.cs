using WKosArch.Extentions;
using UnityEngine;
using UnityEngine.UI;
using WKosArch.UIService.Views.Windows;

public class WindowCloseApp : Window<CloseAppViewModel>
{
    [SerializeField] Button _agreeBtn;
    [SerializeField] Button _cancelBtn;

    public override void Subscribe()
    {
        base.Subscribe();
        Log.PrintWarning("Subscribe WindowClose");
        _agreeBtn.onClick.AddListener(ViewModel.CloseGame);
        _cancelBtn.onClick.AddListener(ViewModel.HideWindow);
        _cancelBtn.onClick.AddListener(HideWindow);
    }

    private void HideWindow()
    {
        ViewModel.Window.Hide();
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _agreeBtn.onClick.RemoveListener(ViewModel.CloseGame);
        _agreeBtn.onClick.RemoveListener(ViewModel.HideWindow);
    }
}
