using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownFunction : ButtonFunction
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] MenuButton menuButton;
    public Dropdown dropdown;
    public virtual void SetValue(int value)
    {
        menuButton.ExitDropDown();
    }

    public override void Press()
    {
        EnterDropDown();
        menuButton.ExitDropDown();
    }

    public void EnterDropDown()
    {
        menuButton.EnterDropDown();
        DisplayDropdownOptions();
    }

    public void DisplayDropdownOptions()
    {
        dropdown.Show();
    }
}
