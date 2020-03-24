using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool conditionMet;

    public void ChangeScene(string sceneName)
    {
        if (conditionMet)
            GameManagerScript.Instance.ChangeScene(sceneName);
        else
        {
            string say = "I think I've missed something...";
            var interact = GetComponent<InteractableObject>();
            interact.inspectText = say;
            interact.InspectDialog();
        }
    }

    public void ConditionMet()
    {
        conditionMet = true;
    }
}
