using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Custom Assets/Inventory")]
public class InventoryObject : ScriptableObject
{
    [SerializeField] private bool clearInventoryOnEnable = true;
    public List<GameObject> itemsInInventory;
    public List<ItemData> itemDataInInventory;
    public UnityAction onInventoryUpdate = null;

    [Tooltip("What order the items should be rendered at. -1 means no change.")]
    [Range(-1, 100)]
    [SerializeField] private int sortingOrder = -1;

    public void AddToInventory(GameObject item)
    {

        Item itemComponent = item.GetComponent<Item>();
        if (itemComponent == null)
        {
            Debug.LogError("Tried to add non-item object to inventory, this is not allowed.", this);
            return;
        }

        if (itemsInInventory.Contains(item))
        {
            Debug.Log("Tried to add already added item to inventory, this is not allowed.", this);
            return;
        }

        itemComponent.isInInventory = true;
        itemComponent.itemData.isUsed = true;
        itemsInInventory.Add(item);
        //itemDataInInventory.Add(item.GetComponent<Item>().itemData);    //adds itself again..

        if (onInventoryUpdate != null)
        {
            onInventoryUpdate.Invoke();
            item.SendMessageUpwards("PlayNewItemAnim", SendMessageOptions.RequireReceiver);
        }

        if(sortingOrder != -1)  //better to set sortingLayer to an item-specific one
        {
            SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = sortingOrder;
            }
        }


    }

    public void RemoveFromInventory(GameObject item)
    {
        if (!itemsInInventory.Contains(item))
        {
            Debug.Log(string.Format("Tried to remove {0} from inventory, it wasn't in the inventory to begin with.", item.name));
            return;
        }
        Item itemComponent = item.GetComponent<Item>();
        itemComponent.isInInventory = false;
        itemsInInventory.Remove(item);

        onInventoryUpdate.Invoke();
    }

    private void OnEnable()
    {
        if (clearInventoryOnEnable)
        {
            itemsInInventory = new List<GameObject>();
            //itemDataInInventory = new List<ItemData>();
        }
    }
}