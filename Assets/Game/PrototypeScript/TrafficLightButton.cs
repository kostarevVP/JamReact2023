using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TurnTheGameOn.SimpleTrafficSystem;

public class TrafficLightButton : MonoBehaviour
{
    public Button traficButton;
    //public AITrafficLight trafficLights;

    private bool _isGreen;



    private void OnEnable()
    {
        traficButton.onClick.AddListener(ChangeColourLight);
        //trafficLights.EnableRedLight();
        _isGreen = false;
    }
    private void OnDisable()
    {
        traficButton.onClick.RemoveListener(ChangeColourLight);
    }

    private void ChangeColourLight()
    {
        if (_isGreen)
        {
            //trafficLights.EnableRedLight();
            _isGreen = false;
        }
        else
        {
            //trafficLights.EnableGreenLight();
            _isGreen = true;
        }
    }
}
