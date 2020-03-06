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
        ExitDropDown();
    }

    public override void Press()
    {
        EnterDropDown();
    }

    public void EnterDropDown()
    {
        menuButtonController.inDropDown = true;
        menuButton.countUp = false;
        DisplayDropdownOptions();
        Debug.Log("Entered");
    }
    
    public void ExitDropDown()
    {
        menuButton.sinceDroppedDown = 0;
        menuButton.countUp = true;
        Debug.Log("Exited");
    }

    public void DisplayDropdownOptions()
    {
        dropdown.Show();
    }
}
