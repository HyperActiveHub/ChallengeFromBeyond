using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The InventoryObject asset that this script will read inventory content from.")]
    [SerializeField] public InventoryObject inventory = null;
    [SerializeField] private GridLayoutGroup gridLayoutGroup = null;
    [Tooltip("What each inventory slot should be instantiated as. This can be a empty game object with a rect transform.")]
    [SerializeField] private GameObject inventorySlotPrefab = null;
    //[Tooltip("This is the content rect transform that is scrolling. It's used to adapt the content area height to the number of inventory slots.")]
    //[SerializeField] private RectTransform content = null;
    private List<GameObject> inventorySlotTransforms = null;

    [Header("Inventory Slots")]
    [Tooltip("How many inventory should initially be displayed. Will increase max if required.")]
    [SerializeField] private int initialInventorySlots = 10;
    [Tooltip("How many slots should be added each time the maximum cap is reached.")]
    [SerializeField] private int inventorySlotIncrements = 5;

    [Header("Animation")]
    [SerializeField] private string animationParameter = "";
    Animator animator = null;
    private bool inventoryIsActive = false;

    private void OnValidate()
    {
        if (gridLayoutGroup == null)
        {
            Debug.LogWarning("Please attach a Grid Layout Group to this Inventory UI", this);
        }

        if (inventory == null)
        {
            Debug.LogWarning("Please attach a Inventory Object to this Inventory UI", this);
        }

        if (inventorySlotPrefab == null)
        {
            Debug.LogWarning("Please attach a Inventory Slot Prefab to this Inventory UI", this);
        }
    }



    private void Awake()
    {
        inventorySlotTransforms = new List<GameObject>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < initialInventorySlots; i++)
        {
            GameObject tempInventorySlot = Instantiate(inventorySlotPrefab, gridLayoutGroup.transform);
            inventorySlotTransforms.Add(tempInventorySlot);
        }
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        inventory.onInventoryUpdate += EnableGameObjects;
        inventory.onInventoryUpdate += SortInventory;
    }



    private void OnDisable()
    {
        inventory.onInventoryUpdate -= SortInventory;
        inventory.onInventoryUpdate -= EnableGameObjects;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        //foreach (var go in inventory.itemsInInventory)
        //{
        //    Destroy(go);
        //}

        //Fucked up combine, itemlists are not updated..

        RePopulateInventory();

        EnableGameObjects();
        SortInventory();
    }

    private void RePopulateInventory()
    {
        inventory.itemsInInventory.Clear();

        var itemDatas = inventory.itemDataInInventory;
        foreach (var item in itemDatas)
        {
            GameObject newItem = Resources.Load<GameObject>("ItemPrefabs/" + item.displayText);
            if (newItem == null)
            {
                Debug.LogError("Item prefab not found. Make sure that the item is prefabbed in Resources/ItemPrefabs, " +
                    "and has the exact same name as 'displayText' on its itemData.", this);
            }

            newItem = Instantiate(newItem);
            newItem.GetComponent<SpriteRenderer>().sortingLayerName = "Inventory";
            newItem.GetComponent<SpriteOutline>().UseLighting = false;

            inventory.AddToInventory(newItem);
            newItem.transform.localScale = Vector3.one;
        }
    }

    private void EnableGameObjects()
    {
        for (int i = 0; i < inventory.itemsInInventory.Count; i++)
        {
            GameObject tempObject = inventory.itemsInInventory[i];
            tempObject.SetActive(true);
        }
    }

    public void ButtonWasPressed()
    {
        if (animator != null)
        {
            //animator.SetBool(animationParameter, inventoryIsActive);
            if (inventoryIsActive)
            {
                animator.SetTrigger("Close");
            }
            else
                animator.SetTrigger("Open");
        }

        inventoryIsActive = !inventoryIsActive;
    }

    public void ExpandInventory()
    {
        if (animator != null)
        {
            inventoryIsActive = true;
            //animator.SetBool(animationParameter, inventoryIsActive);
            animator.SetTrigger("Open");
        }
    }

    public void PlayNewItemAnim()
    {

        if (inventoryIsActive)
        {
            GetComponentsInChildren<FMODUnity.StudioEventEmitter>()[1].Play();
            animator.SetTrigger("ShowNew");
        }
        else
        {
            animator.SetTrigger("Open");
            inventoryIsActive = true;
            animator.SetTrigger("ShowNewAndSound");
        }
    }

    #region Inventory Updates and Sorting
    public void SortInventory()
    {
        ExpandInventoryIfNeeded();

        for (int i = 0; i < inventory.itemsInInventory.Count; i++)
        {
            inventory.itemsInInventory[i].transform.parent = inventorySlotTransforms[i].transform;
            inventory.itemsInInventory[i].transform.localPosition = Vector3.zero;
        }
    }

    private void ExpandInventoryIfNeeded()
    {
        if (inventorySlotTransforms.Count <= inventory.itemsInInventory.Count)
        {
            for (int i = 0; i < inventorySlotIncrements; i++)
            {
                AddInventorySlot();
            }
        }
    }

    private void AddInventorySlot()
    {
        GameObject tempInventorySlot = Instantiate(inventorySlotPrefab, gridLayoutGroup.transform);
        inventorySlotTransforms.Add(tempInventorySlot);
    }
    #endregion
}
