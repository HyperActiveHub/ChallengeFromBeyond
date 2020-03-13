using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TEMP, REMOVE LATER
//[ExecuteInEditMode]
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
    public float transisionTime = 1;
    [HideInInspector] public bool hasAwakened;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            FindObjectOfType<InventoryUI>().inventory.itemDataInInventory.Clear();
            print("Cleared saved inventory");
        }
        else if (_instance != null)
        {
            Destroy(this.gameObject);
        }

        

        //if (/*game started.*/)
        //{
        //FindObjectOfType<InventoryUI>().inventory.itemDataInInventory.Clear();
        //print("ree");
        //}

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

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string name)
    {
        //Need to get animator each time LoadLevel call, since its difference animator objects in each scene.
        Animator transision = GameObject.Find("Crossfade").GetComponent<Animator>();
        if (transision == null)
        {
            Debug.LogError("Crossfade object (animator) not found. Please add Crossfade (with that exact name) to the scene.", this);
        }
        transision.SetTrigger("Start");

        var inv = FindObjectOfType<InventoryUI>().inventory;

        inv.itemDataInInventory.Clear();
        foreach (var item in inv.itemsInInventory)  //Save itemData to re-initialize inventory on next scene.
        {
            inv.itemDataInInventory.Add(item.GetComponent<Item>().itemData);
        }

        yield return new WaitForSeconds(transisionTime);

        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    private void Start()
    {

    }

    void Update()
    {

    }
}
