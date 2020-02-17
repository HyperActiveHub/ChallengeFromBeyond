using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour
{
    public ItemData itemData = null;
    private SpriteRenderer spriteRenderer = null;
    [HideInInspector] public bool isInInventory = false;

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
            InventoryManager.Instance.RemoveItem(this.gameObject);
        }

        GameObject instantiatedObject = Instantiate(prefab, transform.position, Quaternion.identity);
        instantiatedObject.name = itemData.displayText;
        InventoryManager.Instance.AddItem(instantiatedObject);
        
        Destroy(this.gameObject);
    }

    public void InsertToInventory()
    {
        if (!isInInventory)
        {
            //Debug.Log(string.Format("Inserting \"{0}\" to inventory", this.gameObject.name));
            InventoryManager.Instance.AddItem(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, itemData.displayText);
    }

    public static bool operator ==(Item item1, Item item2)
    {
        if (object.ReferenceEquals(item1, null))
        {
            return object.ReferenceEquals(item1, null);
        }

        if (object.ReferenceEquals(null, item2))
        {
            return object.ReferenceEquals(item1, null);
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
