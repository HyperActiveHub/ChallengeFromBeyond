using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Player", "Switch movement", "Switches the player's movement speed to bool, other values are optional")]
public class PlayerMovementSwitch : Command
{
    [Tooltip("Attach this for faster loading (optional)")] public GameManagerScript managerScript = null;
    [Tooltip("If it's not GM")] public string Name = null;
    public bool switchTo;
    // Start is called before the first frame update
    void Start()
    {
        if (managerScript == null)
        {
            if (name == null)
            {
                managerScript = GameObject.Find("GM").GetComponent<GameManagerScript>();
            }
            else
            {
                managerScript = GameObject.Find(name).GetComponent<GameManagerScript>();
            }
        }
    }

    public override void OnEnter()
    {
        if (switchTo)
        {
            managerScript.SetPlayerMovement(true);
        }
        else
        {
            managerScript.SetPlayerMovement(false);
        }
        Continue();
    }
}
