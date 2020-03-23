using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        GameManagerScript.Instance.ChangeScene(sceneName);
    }
}
