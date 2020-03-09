using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : ButtonFunction
{
    public string scene;
    public override void Press()
    {
        if (scene != null)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
