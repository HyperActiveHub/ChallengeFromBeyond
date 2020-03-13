using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ClickAndDrag))]
[RequireComponent(typeof(InteractableObject))]
[RequireComponent(typeof(SpriteOutline))]
public class Item : MonoBehaviour
{
    public ItemData itemData = null;
    private SpriteRenderer spriteRenderer = null;
    [HideInInspector] public bool isInInventory = false;
    [Tooltip("What order this object will be assigned when being added to inventory. A too low value might result in the item not being properly displayed in the inventory.")]
    public InventoryObject inventoryObject = null;

    private void OnValidate()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (itemData == null)
        {
            Debug.LogWarning("Item GameObject has no attached item. Please attach one.", this);
        }

        if (spriteRenderer != null)
        {
            ReloadItem();
        }

        if (itemData.displayText == "")
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
        var items = FindObjectsOfType<Item>();

        //need to keep track of if item is already picked-up, used or others. Prevent items from spawning in scenes if already used/picked up.
        //currently, if items are used, they will spawn in scenes. 
        //Duplicates will NOT spawn if the item exists already, hopefully in inventory.
        foreach (var item in items)
        {
            //unsure of itemID usage...
            if (item.itemData == itemData && gameObject != item.gameObject)  //same itemData, but not same object
            {
                Debug.LogWarning("A copy of item: '" + itemData.displayText + "' was found in the scene, and was removed.");
                if (isInInventory)
                {
                    Destroy(item.gameObject);
                }
                else
                    Destroy(gameObject);
            }

        }

        itemData.itemID = gameObject.GetInstanceID();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ReloadItem()
    {
        if (spriteRenderer != null)
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
            inventoryObject.RemoveFromInventory(gameObject);
        }

        GameObject instantiatedObject = Instantiate(prefab, transform.position, Quaternion.identity);
        instantiatedObject.name = itemData.displayText;
        InsertToInventory(instantiatedObject);

        Destroy(gameObject);
    }

    /// <summary>
    /// Insert GameObject to the inventory.
    /// </summary>
    /// <param name="item">The item to insert to the inventory. If undefined, this object will be added to the inventory.</param>

    public void InsertToInventory(GameObject item = null)
    {
        if (this.gameObject.GetComponent<Item>() == null)
        {
            Debug.LogError("Tried to add non-item GameObject to inventory!");
            return;
        }

        if (inventoryObject == null)
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
            item.GetComponent<SpriteRenderer>().sortingLayerName = "Inventory";    //temp
            inventoryObject.AddToInventory(item);
            item.GetComponent<SpriteOutline>().SetMaterial(false);
        }
    }

    public void AddPrefabToInventory(GameObject prefab = null)
    {
        GameObject itemObj = Instantiate(prefab);

        if (itemObj.gameObject.GetComponent<Item>() == null)
        {
            Debug.LogError("Tried to add non-item GameObject to inventory!");
            return;
        }

        if (inventoryObject == null)
        {
            Debug.LogError("This item does not have an attached inventory object. Can not add item to null-inventory", this);
            return;
        }

        itemObj.SetActive(false);
        itemObj.GetComponent<SpriteRenderer>().sortingLayerName = "Inventory";    //temp
        inventoryObject.AddToInventory(itemObj);
        itemObj.GetComponent<SpriteOutline>().SetMaterial(false);
    }

    public void InsertThisToInventory()
    {
        InsertToInventory(this.gameObject);
    }

    public void ResetTransform()
    {
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.rotation = Quaternion.identity;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, itemData.displayText);
    }
#endif
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
