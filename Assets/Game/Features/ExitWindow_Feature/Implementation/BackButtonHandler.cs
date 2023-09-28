using WKosArch.Common.DIContainer;
using UnityEngine;
using WKosArch.Services.UIService;

public class BackButtonHandler : MonoBehaviour
{
    private UserInterface _ui;

    private void Start()
    {
        _ui = new DIVar<UserInterface>().Value;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _ui.ShowWindow<CloseAppViewModel>();
        }
    }
}
