using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenuButton : ButtonFunction
{
    public GameObject currentMenu;
    public GameObject newMenu;
    public override void Press()
    {
        newMenu.SetActive(true);
        currentMenu.SetActive(false);
    }
}
