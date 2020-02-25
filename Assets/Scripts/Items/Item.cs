using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour
{
    public ItemData itemData = null;
    private SpriteRenderer spriteRenderer = null;
    [HideInInspector] public bool isInInventory = false;
    [Tooltip("What order this object will be assigned when being added to inventory. A too low value might result in the item not being properly displayed in the inventory.")]
    public InventoryObject inventoryObject = null;

    private void OnValidate()
    {
        if(itemData == null)
        {
            Debug.LogWarning("Item GameObject has no attached item. Please attach one.", this);
        }

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if(spriteRenderer != null)
        {
            ReloadItem();
        }

        if(itemData.displayText == "")
        {
            Debug.LogWarning(string.Format("The item \"{0}\" is missing a proper display text.", itemData.name), this);
        }

        if (itemData.itemSprite == null)
        {
            Debug.LogWarning(string.Format("The item \"{0}\" is missing a sprite.", itemData.name), this);
        }
    }

    private void Start()
    {
        itemData.itemID = gameObject.GetInstanceID();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ReloadItem()
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = itemData.itemSprite;
        }
    }

    public void SetItemID(int itemID)
    {
        this.itemData.itemID = itemID;
    }

    public void SetItem(ItemData itemData)
    {
        this.itemData = itemData;
        ReloadItem();
    }

    public void CombineWithItem(GameObject prefab)
    {
        if (isInInventory)
        {
            inventoryObject.RemoveFromInventory(this.gameObject);
        }

        GameObject instantiatedObject = Instantiate(prefab, transform.position, Quaternion.identity);
        instantiatedObject.name = itemData.displayText;
        InsertToInventory(instantiatedObject);
        
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Insert GameObject to the inventory.
    /// </summary>
    /// <param name="item">The item to insert to the inventory. If undefined, this object will be added to the inventory.</param>

    public void InsertToInventory(GameObject item = null)
    {
        if(this.gameObject.GetComponent<Item>() == null)
        {
            Debug.LogError("Tried to add non-item GameObject to inventory!");
            return;
        }

        if(inventoryObject == null)
        {
            Debug.LogError("This item does not have an attached inventory object. Can not add item to null-inventory", this);
            return;
        }

        if (isInInventory)
        {
            Debug.Log("Item already in inventory, can not add item again.", this);
            return;
        }
        else
        {
            item.gameObject.SetActive(false);
            inventoryObject.AddToInventory(item);
        }
    }

    public void InsertThisToInventory()
    {
        InsertToInventory(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, itemData.displayText);
    }

    public static bool operator ==(Item item1, Item item2)
    {
        if (item1 is null || item2 is null)
        {
            return item1 is null;
        }

        return item1.Equals(item2);
    }

    public static bool operator !=(Item item1, Item item2)
    {
        return !(item1 == item2);
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return itemData.displayText;
    }
}
