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

    Animator transision;
    public float transisionTime = 1;
    [HideInInspector] public bool hasAwakened;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != null)
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

        transision = GameObject.Find("Crossfade").GetComponent<Animator>();
        if (transision == null)
        {
            Debug.LogError("Crossfade object (animator) not found. Please add Crossfade (with that exact name) to the scene.", this);
        }

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
        transision.SetTrigger("Start");

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
