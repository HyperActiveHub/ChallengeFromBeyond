using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TEMP, REMOVE LATER
[ExecuteInEditMode]
//TEMP, REMOVE LATER
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public Color spriteOutlineColor = new Color(1, 1, 0, 1);
    public enum OutlineMode { Off, Outside, Inside };
    public OutlineMode outlineMode = OutlineMode.Inside;

    public float GetOutlineMode()
    {
        return (float)outlineMode;
    }

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(outlineMode == (OutlineMode)2)
            {
                outlineMode = (OutlineMode)1;
            }
            else
                outlineMode++;
        }
    }
}
