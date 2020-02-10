using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData = null;

    private SpriteRenderer spriteRenderer = null;

    private void OnValidate()
    {
        if(itemData == null)
        {
            Debug.LogWarning("Item GameObject has no attached item. Please attach one.", this);
        }
    }

    private void Start()
    {
        itemData.itemID = gameObject.GetInstanceID();

        spriteRenderer = GetComponent<SpriteRenderer>();

        //TODO: Should be done in editor and not in game. Fix when adding custom editor.
        //if(itemData.itemSprite != null && spriteRenderer != null)
        //{
        //    spriteRenderer.sprite = itemData.itemSprite;
        //}
    }

    public void SetItemID(int itemID)
    {
        this.itemData.itemID = itemID;
    }

    public void SetItem(ItemData item)
    {
        this.itemData = item;
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

    public void PublicPrint(string printString)
    {
        Debug.Log(printString);
    }

}
