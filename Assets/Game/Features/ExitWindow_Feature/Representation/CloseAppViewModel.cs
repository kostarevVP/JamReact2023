using WKosArch.Common.DIContainer;
using UnityEngine;
using WKosArch.UIService.Views.Windows;

public class CloseAppViewModel : WindowViewModel
{
    private ISaveLoadService _saveLoadFeature;

    protected override void AwakeInternal()
    {
        base.AwakeInternal();

        _saveLoadFeature = new DIVar<ISaveLoadService>().Value;
    }


    public void CloseGame()
    {
        _saveLoadFeature.SaveProgress();

        UnPauseGame();

        Application.Quit();
    }

    public void HideWindow()
    {
        UnPauseGame();

        UI.ShowWindow<SettingButtonViewModel>();
        Window.Hide();
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
    }
    private void UnPauseGame()
    {
        Time.timeScale = 1;
    }
}
