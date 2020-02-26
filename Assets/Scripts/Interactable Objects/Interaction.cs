using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Interaction
{
    public ItemData interactedWithItem;
    [Tooltip("If true the other item (interactedWithItem) will be destroyed when interacting with this object. (This box should be ticked for 99.9% of cases.)")]
    public bool consumable = true;
    //public bool invokeInInventory = true;
    public UnityEvent onInteraction;
    

    public bool Interact(Item item)
    {
        if (EvaluateInteraction(item))
        {

            onInteraction.Invoke();
            return true;
        }
        return false;
    }

    private bool EvaluateInteraction(Item item)
    {
        if(interactedWithItem == null && item == null)
        {
            return true;
        }

        if(interactedWithItem == null && item != null)
        {
            return false;
        }

        if(interactedWithItem != null && item == null)
        {
            return false;
        }

        if(interactedWithItem != null)
        {
            if (interactedWithItem.itemID == item.itemData.itemID)
            {
                return true;
            }
        }
        return false;
    }
}
