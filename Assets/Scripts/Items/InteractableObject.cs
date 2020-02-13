using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
{
    public List<Interaction> interactions;

    [Header("Tools for testing")]
    public ItemData testItemToInteractWith;

    public Interaction Interact(ItemData otherItem)
    {
        if (interactions != null)
        {
            for (int i = 0; i < interactions.Count; i++)
            {
                bool wasInteractedWith = interactions[i].Interact(otherItem);
                if (wasInteractedWith)
                {
                    Debug.Log("Interaction ok, interacted with " + interactions[i].interactedWithItem.displayText);
                    return interactions[i];
                }
            }
        }
        return null;
    }

    public Interaction Interact(Item otherItem)
    {
        return Interact(otherItem.itemData);
    }
}
