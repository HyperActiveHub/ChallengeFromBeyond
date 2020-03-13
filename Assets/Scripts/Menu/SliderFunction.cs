using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFunction : MonoBehaviour
{
    [SerializeField] Slider slider;
    [Range(0.0f, 1.0f)] public float sensitivity = 0.1f;
    public Text text;
    public string sliderText;

    void Start()
    {
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        string sliderMessage = sliderText + slider.value * 100;
        text.text = sliderMessage;
    }
    public virtual void ChangeValue(float value)
    {
        ShowSliderValue();
    }



    public void SetValue(float value)
    {
        value *= sensitivity;
        value += slider.value;
        Mathf.Clamp(value, 0, 1);
        slider.SetValueWithoutNotify(value);
        ChangeValue(value);
    }
}
