using UnityEngine.UI;
using WKosArch.UIService.Views.Windows;

public class WindowSettingButton : Window<SettingButtonViewModel>, IHomeWindow
{
    private Button _button;
    protected override void AwakeInternal()
    {
        base.AwakeInternal();
        _button = GetComponent<Button>();
    }

    public override void Subscribe()
    {
        base.Subscribe();
        _button.onClick.AddListener(ViewModel.OpenSetting);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _button.onClick.RemoveListener(ViewModel.OpenSetting);
    }

}
