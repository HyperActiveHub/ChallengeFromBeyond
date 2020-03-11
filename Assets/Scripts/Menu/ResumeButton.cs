using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : ButtonFunction
{
    [Tooltip("Probably the canvas, it's a reference to where the script is not the menu itself")] public PauseMenu pauseMenu;
    public override void Press()
    {
        pauseMenu.Resume();
    }
}
