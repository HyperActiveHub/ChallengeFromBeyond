using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory = null;
    [SerializeField] private GridLayoutGroup gridLayoutGroup = null;
    [SerializeField] private GameObject inventorySlotPrefab = null;
    private List<GameObject> inventorySlotTransforms = null;

    [Header("Inventory Slots")]
    [Tooltip("How many inventory should initially be displayed. Will increase max if required.")]
    [SerializeField] private int initialInventorySlots = 10;
    [Tooltip("How many slots should be added each time the maximum cap is reached.")]
    [SerializeField] private int inventorySlotIncrements = 5;

    [Header("Inventory Button")]
    [SerializeField] private GameObject disableOnButtonPress = null;

    private void OnValidate()
    {
        if(gridLayoutGroup == null)
        {
            Debug.LogWarning("Please attach a Grid Layout Group to this Inventory UI", this);
        }

        if(inventory == null)
        {
            Debug.LogWarning("Please attach a Inventory Object to this Inventory UI", this);
        }

        if(inventorySlotPrefab == null)
        {
            Debug.LogWarning("Please attach a Inventory Slot Prefab to this Inventory UI", this);
        }
    }

    private void Awake()
    {
        inventorySlotTransforms = new List<GameObject>();
        for (int i = 0; i < initialInventorySlots; i++)
        {
            GameObject tempInventorySlot = Instantiate(inventorySlotPrefab, gridLayoutGroup.transform);
            inventorySlotTransforms.Add(tempInventorySlot);
        }
    }

    private void OnEnable()
    {
        inventory.onInventoryUpdate += EnableGameObjects;
        inventory.onInventoryUpdate += SortInventory;

        EnableGameObjects();
        SortInventory();
    }

    private void OnDisable()
    {
        inventory.onInventoryUpdate -= SortInventory;
        inventory.onInventoryUpdate -= EnableGameObjects;
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
        if(disableOnButtonPress != null)
        {
            disableOnButtonPress.SetActive(!disableOnButtonPress.activeSelf);
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
        Debug.Log(string.Format("inventorySlotTransforms.Count: {0}, inventory.itemsInInventoryCount: {1}", inventorySlotTransforms.Count, inventory.itemsInInventory.Count));
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
