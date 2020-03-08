using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFunction : MonoBehaviour
{
    [SerializeField] Slider slider;
    public virtual void ChangeValue(float value)
    {
        SetValue(value);
    }
    public void SetValue(float value)
    {
        value += slider.value;
        Mathf.Clamp(value, 0, 1);
        slider.SetValueWithoutNotify(value);
    }
}
