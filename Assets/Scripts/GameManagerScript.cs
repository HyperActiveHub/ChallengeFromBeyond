using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    InventoryUI inventoryUI = null;
    PlayerController playerController = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            inventoryUI = FindObjectOfType<InventoryUI>();

            if(inventoryUI == null)
            {
                Debug.LogWarning("Missing inventory in scene.", this);
            }
            else
            {
                inventoryUI.inventory.itemDataInInventory.Clear();
                print("Cleared saved inventory");
            }
        }
        else if (_instance != null)
        {
            Destroy(this.gameObject);
        }

        litOutlineMat = Resources.Load<Material>(litMatPath);
        unlitOutlineMat = Resources.Load<Material>(unlitMatPath);

        playerController = FindObjectOfType<PlayerController>();

        if(playerController == null)
        {
            Debug.LogWarning("Player controller not found, scene is missing the player object.");
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
        //Need to get animator each time LoadLevel call, since its difference animator objects in each scene.
        Animator transision = GameObject.Find("Crossfade").GetComponent<Animator>();
        if (transision == null)
        {
            Debug.LogError("Crossfade object (animator) not found. Please add Crossfade (with that exact name) to the scene.", this);
        }
        transision.SetTrigger("Start");

        inventoryUI.inventory.itemDataInInventory.Clear();
        foreach (var item in inventoryUI.inventory.itemsInInventory)
        {
            inventoryUI.inventory.itemDataInInventory.Add(item.GetComponent<Item>().itemData);
        }

        yield return new WaitForSeconds(transisionTime);

        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void SetPlayerMovement(bool value)
    {
        playerController.canMove = value;
    }

    //need to read these bool on scene load, to set if puzzle should be interactable or not
    public void PipePuzzleDone()
    {
        //set bool true
    }

    public void SlidePuzzleDone()
    {
        //set bool true
    }

    public void CodeLockDone()
    {
        //set bool true
    }

    private void Start()
    {

    }

    void Update()
    {

    }

}
