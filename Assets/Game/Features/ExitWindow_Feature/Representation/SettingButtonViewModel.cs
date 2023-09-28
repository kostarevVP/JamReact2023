using WKosArch.Services.SoundService;
using WKosArch.UIService.Views.Windows;

public class SettingButtonViewModel : WindowViewModel
{
    protected override void AwakeInternal()
    {
        base.AwakeInternal();
    }

    internal void OpenSetting()
    {
        UI.ShowWindow<SoundSettingViewModel>();

        Window.Hide();
    }
}
