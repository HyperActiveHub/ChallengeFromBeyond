using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : ButtonFunction
{
    public override void Press()
    {
        Application.Quit();
        Debug.Log("WIll quit if game is built but not in editor");
    }
}
