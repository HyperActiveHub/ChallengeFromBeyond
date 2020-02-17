using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Interaction
{
    public ItemData interactedWithItem;
    public bool consumable = true;
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
