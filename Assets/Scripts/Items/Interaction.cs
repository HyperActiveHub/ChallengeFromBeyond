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
    

    public bool Interact(ItemData item)
    {
        if (EvaluateInteraction(item))
        {
            onInteraction.Invoke();
            return true;
        }
        return false;
    }

    public bool EvaluateInteraction(ItemData item)
    {
        if (interactedWithItem == item)
        {
            return true;
        }
        return false;
    }
}
