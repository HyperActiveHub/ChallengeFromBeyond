using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene_Claes : MonoBehaviour
{
        //När knappen trycks så byts scenen till nästa scen.
    public void NextScene()
    {
        SceneManager.LoadScene("Scene 3");
    }
	
}
