using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TEMP, REMOVE LATER
[ExecuteInEditMode]
//TEMP, REMOVE LATER
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance
    {
        get
        {
            if (_instance == null && Application.isPlaying)
            {
                GameObject gm = new GameObject("Game Manager");
                _instance = gm.AddComponent<GameManagerScript>();
            }
            return _instance;
        }
    }
    private static GameManagerScript _instance;

    public Color spriteOutlineColor = new Color(1, 1, 0, 1);
    public enum OutlineMode { Off, Outside };
    public OutlineMode outlineMode = OutlineMode.Outside;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public float GetOutlineMode()
    {
        return (float)outlineMode;
    }


    void Update()
    {
        
    }
}
