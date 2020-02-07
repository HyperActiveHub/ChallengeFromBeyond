﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData = null;

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
    }

    public static bool operator ==(Item item1, Item item2)
    {
        if (item1.itemData.itemID == item2.itemData.itemID)
        {
            return true;
        }
        return false;
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

    public void SetItemID(byte itemID)
    {
        this.itemData.itemID = itemID;
    }

}
