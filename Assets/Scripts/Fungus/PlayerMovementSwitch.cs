using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Player", "Switch movement", "Switches the player's movement speed to bool, other values are optional")]
public class PlayerMovementSwitch : Command
{
    public bool switchTo;

    public override void OnEnter()
    {
        if (switchTo)
        {
            GameManagerScript.Instance.SetPlayerMovement(true);
        }
        else
        {
            GameManagerScript.Instance.SetPlayerMovement(false);
        }
        Continue();
    }
}
