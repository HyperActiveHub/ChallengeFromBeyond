using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool conditionMet;

    public void ChangeScene(string sceneName)
    {
        if (conditionMet)
        {
            GameManagerScript.Instance.ChangeScene(sceneName);
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }
        else
        {
            string say = GetComponent<InteractableObject>().inspectText;
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
