using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    PipePuzzle pipePuzzle;
    string lastSceneName;
    bool restart;

    Texture2D defaultMouse, bag, bubble, bubbleText, click, inspect, left, right, puzzle;
    public enum CursorType { defaultCursor, bagCursor, bubbleCursor, bubbleTextCursor, clickCursor, inspectCursor, leftCursor, rightCursor, puzzleCursor };



    void LoadMouseTextures()
    {
        var mouseTextures = Resources.LoadAll<Texture2D>("MouseTextures");

        foreach (var texture in mouseTextures)
        {
            switch (texture.name)
            {
                case "Mouse":
                    defaultMouse = texture;
                    break;

                case "Mouse_bag":
                    bag = texture;
                    break;

                case "Mouse_bubble":
                    bubble = texture;
                    break;

                case "Mouse_bubble_text":
                    bubbleText = texture;
                    break;

                case "Mouse_front":
                    click = texture;
                    break;

                case "Mouse_glass":
                    inspect = texture;
                    break;

                case "Mouse_left":
                    left = texture;
                    break;

                case "Mouse_puzzle":
                    puzzle = texture;
                    break;

                case "Mouse_right":
                    right = texture;
                    break;

                default:
                    Debug.LogError("Texture name didnt match any texture in GM.", this);
                    break;
            }
        }
    }

    public void SetCursor(CursorType cursor)
    {
        Vector2 offset = new Vector2(20, 10);
        switch (cursor)
        {
            case CursorType.defaultCursor:
                Cursor.SetCursor(defaultMouse, offset, CursorMode.Auto);
                break;

            case CursorType.bagCursor:
                Cursor.SetCursor(bag, offset, CursorMode.Auto);
                break;

            case CursorType.bubbleCursor:
                Cursor.SetCursor(bubble, offset, CursorMode.Auto);
                break;

            case CursorType.bubbleTextCursor:
                Cursor.SetCursor(bubbleText, offset, CursorMode.Auto);
                break;

            case CursorType.clickCursor:
                Cursor.SetCursor(click, offset, CursorMode.Auto);
                break;

            case CursorType.inspectCursor:
                Cursor.SetCursor(inspect, offset, CursorMode.Auto);
                break;

            case CursorType.leftCursor:
                Cursor.SetCursor(left, offset, CursorMode.Auto);
                break;

            case CursorType.rightCursor:
                Cursor.SetCursor(right, offset, CursorMode.Auto);
                break;

            case CursorType.puzzleCursor:
                Cursor.SetCursor(puzzle, offset, CursorMode.Auto);
                break;

            default:
                Debug.LogError("Cursor Type not set-up.", this);
                break;
        }
    }

    public InventoryUI GetInventoryUI()
    {
        return FindObjectOfType<InventoryUI>();

    }

    public void RestartGame()
    {
        restart = true;

        var inv = Resources.Load<InventoryObject>("Player Inventory");
        inv.itemDataInInventory.Clear();

        var items = Resources.LoadAll<ItemData>("ItemsData");
        foreach (var item in items)
        {
            item.Reset();
        }

        ResetStatics();
    }

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

        LoadMouseTextures();
    }

    void GetPlayer()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogWarning("Player controller not found, scene is missing the player object.");
        }
    }

    public float GetOutlineMode()
    {
        return (float)outlineMode;
    }

    public void Quit()
    {
        Application.Quit();
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

        inventoryUI = GetInventoryUI();

        if (inventoryUI != null)
        {
            inventoryUI.inventory.itemDataInInventory.Clear();
            foreach (var item in inventoryUI.inventory.itemsInInventory)
            {
                inventoryUI.inventory.itemDataInInventory.Add(item.GetComponent<Item>().itemData);
            }
        }
        else
            Debug.LogWarning("inventory missing", this);

        yield return new WaitForSeconds(transisionTime);

        lastSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);

        if (restart)
        {
            restart = false;
            Destroy(gameObject);
        }
    }

    public void SetPlayerMovement(bool value)
    {
        playerController.SetCanMove(value);
    }

    Vector2 GetEntryPoint(out Vector2 doorPos)
    {
        if (lastSceneName != "")
        {
            var doors = FindObjectsOfType<DoorScript>();

            foreach (var door in doors)
            {
                if (door.NextRoom == lastSceneName)
                {
                    doorPos = door.transform.position;
                    return door.GetSpawnPos();
                }
            }

            Debug.LogWarning("Entry-door not found.", this);
        }
        else
            Debug.LogWarning("No last scene name set.", this);

        doorPos = Vector2.zero;
        return playerController.transform.position;
    }

    void MovePlayerToEntry()
    {
        Vector2 doorPos;
        Vector2 offset = Vector2.zero;
        Vector2 spawnPoint = GetEntryPoint(out doorPos);
        playerController.transform.position = spawnPoint;

        if (doorPos != Vector2.zero)
        {
            offset = (spawnPoint - doorPos) * 0.25f;

            if (Mathf.Abs(offset.x) > 0.01f)     //Essentially spawnPoint.x != 0
            {
                offset.y = 0;
            }
        }

        playerController.target.position = spawnPoint + offset;  //added offset makes the player walk outward from door.

        Camera.main.transform.position = new Vector3(spawnPoint.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        SetCursor(CursorType.defaultCursor);

        if (!scene.name.Contains("Start") && !scene.name.Contains("End"))
        {
            GetPlayer();
            MovePlayerToEntry();
        }

        if (scene.name.Contains("Boiler"))
        {
            BoilerLoaded();
        }
        else if (scene.name.Contains("Lounge"))
        {
            LoungeLoaded();
        }
        else if (scene.name.Contains("Study"))
        {

        }
        else if (scene.name.Contains("Museum"))
        {
            MuseumLoaded();
        }

    }

    //need to read these bools on scene load, to set puzzles and interactables to their correct states
    #region scene-specific helper functions

    #region Boiler Room
    public void PipePuzzleDone()
    {
        //set bool true
        pipePzleDone = true;
    }
    public void AssembledLamp()
    {
        isLampAssembled = true;
    }

    public static bool pipePzleDone = false;
    public static bool isLampAssembled = false;
    void BoilerLoaded()
    {
        GameObject.Find("Assembled Lamp").SetActive(isLampAssembled);

        if (pipePzleDone)
        {
            Debug.Log("puzzle already done.");
            FindObjectOfType<PipePuzzle>().onWin.Invoke();
        }

        var bucket = Resources.Load<ItemData>("ItemsData/I_Bucket_w");
        if (bucket.isUsed)
        {
            FindObjectOfType<DoorScript>().ConditionMet();
            Debug.Log("bucket is in inventory, door is open.");
        }
    }
    #endregion

    #region Lounge
    public static bool firePutOut = false;

    void LoungeLoaded()
    {
        var loungeDoors = FindObjectsOfType<DoorScript>();
        DoorScript museumDoor = null;

        foreach (DoorScript door in loungeDoors)
        {
            if (door.name.Contains("Museum"))
            {
                museumDoor = door;
            }
        }

        if (museumDoor == null)
        {
            Debug.LogError("Museum Door not found in Lounge.", this);
        }
        else
        {
            var museumKey = Resources.Load<ItemData>("ItemsData/I_Key");
            if (museumKey == null)
            {
                Debug.LogError("Museum key not found in resources.");
            }
            else
            {

                inventoryUI = GetInventoryUI();
                if (inventoryUI == null)
                {
                    Debug.LogError("inventory not found", this);
                }
                else if (inventoryUI.inventory.itemDataInInventory.Contains(museumKey))
                {
                    Debug.Log("Inventory contained the museum key, museum door is now open.");
                    museumDoor.ConditionMet();
                }
            }

        }

        GameObject fireplace = GameObject.Find("Fireplace");
        if (fireplace != null)
        {
            if (firePutOut)
            {
                fireplace.GetComponent<InteractableObject>().interactions[1].onInteraction.Invoke();
                fireplace.GetComponentInChildren<Animator>().SetTrigger("Die");

                //avoid playing extinguish sound on Die-animation
                for (int i = 0; i < fireplace.transform.childCount; i++)
                {
                    var child = fireplace.transform.GetChild(i);
                    if (child.name == "Fireplace_Fire")
                    {
                        for (int j = 0; j < child.transform.childCount; j++)
                        {
                            var fireChild = child.transform.GetChild(i);
                            if (fireChild.name == "Extinguish Sound")
                                Destroy(fireChild.gameObject);
                        }
                    }
                }
            }
        }
        else
            Debug.LogError("fireplace wasnt found.", this);

    }
    #endregion

    #region Museum Room
    public static bool slidePuzzleDone = false;
    public static bool scarabInserted = false;

    void MuseumLoaded()
    {
        var slidingPuzzle = FindObjectOfType<ST_PuzzleDisplay>();
        slidingPuzzle.gameObject.SetActive(false);

        if (slidePuzzleDone)
        {
            slidingPuzzle.Complete = true;
            slidingPuzzle.triggeredComplete = true;
            slidingPuzzle.OnComplete.Invoke();

            var sDone = GameObject.Find("SlidingDone");

            if (sDone != null)
                sDone.SetActive(false);
            else
                Debug.LogError("SlidingDone object not found.", this);

            if (scarabInserted)
            {
                //sDone.GetComponent<InteractableObject>().interactions[1].interactedWithItem = null;
                sDone.GetComponent<InteractableObject>().interactions[1].onInteraction.Invoke();    //when scarab is inserted.
            }
        }


    }

    void ResetStatics()     //OBS: dont forget to add all static varibles here
    {
        pipePzleDone = false;
        isLampAssembled = false;
        firePutOut = false;
        slidePuzzleDone = false;
        scarabInserted = false;
    }

    public void SlidePuzzleDone()
    {
        slidePuzzleDone = true;
    }
    #endregion

    #region Study Room
    public static bool codeLockDone;
    public void StudyLoaded()
    {
        //if(codeLockDone)
        //set codelock win?
    }
    public void CodeLockDone()
    {
        codeLockDone = true;
    }
    #endregion

    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoad;
    }
}
