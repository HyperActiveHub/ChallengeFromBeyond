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

    const string litMatPath = "Materials/LitSpriteOutline";
    const string unlitMatPath = "Materials/UnlitSpriteOutline";
    [HideInInspector]
    public Material litOutlineMat, unlitOutlineMat;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if(_instance != null)
        {
            Destroy(this.gameObject);
        }

        litOutlineMat = Resources.Load<Material>(litMatPath);
        unlitOutlineMat = Resources.Load<Material>(unlitMatPath);

        ///Make a new item (temp for testing purposes)
        //GameObject item = new GameObject("someItem");
        //item.AddComponent<Item>().SetItem(Resources.Load<ItemData>("Items/I_Lamp_Stand"));
        //item.GetComponent<Item>().inventoryObject = Resources.FindObjectsOfTypeAll<InventoryObject>()[0];
        //item.GetComponent<SpriteRenderer>().sortingOrder = 5;
        
    }

    public float GetOutlineMode()
    {
        return (float)outlineMode;
    }

    private void Start()
    {
    }

    void Update()
    {
        
    }
}
