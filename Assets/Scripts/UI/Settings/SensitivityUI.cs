using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityUI : MonoBehaviour {

    public Slider slider;
    const string AccelerateScale = "AccelerateScale";

    private void OnEnable()
    {
        slider.value = InputHandler.current.m_AccelerateScale;   
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AccelerateScale, slider.value);
        PlayerPrefs.Save();
    }
}
