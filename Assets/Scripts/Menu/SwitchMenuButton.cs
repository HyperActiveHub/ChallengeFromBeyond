using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenuButton : ButtonFunction
{
    public GameObject currentMenu;
    public GameObject newMenu;
    public MenuButton startingButton;
    public override void Press()
    {
        newMenu.SetActive(true);
        startingButton.ExitDropDown();
        newMenu.GetComponent<MenuButtonController>().inDropDown = true;
        currentMenu.SetActive(false);
    }
}
