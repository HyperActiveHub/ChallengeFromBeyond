using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Interaction
{
    public ItemData interactedWithItem;
    public UnityEvent onInteraction;

    public void Interact(ItemData item)
    {
        if (EvaluateInteraction(item))
        {
            onInteraction.Invoke();
        }
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
