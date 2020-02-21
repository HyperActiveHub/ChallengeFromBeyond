using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene_Title : MonoBehaviour
{
        //När knappen trycks så byts scenen till nästa scen.
    public void NextScene()
    {
        SceneManager.LoadScene("NameScene");
    }
	
}
