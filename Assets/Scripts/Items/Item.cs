using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour
{
    public ItemData itemData = null;
    private SpriteRenderer spriteRenderer = null;

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
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);   
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, itemData.displayText);
    }

    public static bool operator ==(Item item1, Item item2)
    {
        return item1.itemData.itemID == item2.itemData.itemID;
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
