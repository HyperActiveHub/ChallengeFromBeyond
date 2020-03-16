using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    //this exists so that the GM is not missing in the interaction, after the first scene-change.
    public void ChangeScene(string sceneName)
    {
        GameManagerScript.Instance.ChangeScene(sceneName);
    }
}
