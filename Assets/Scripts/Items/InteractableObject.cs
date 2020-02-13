using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
{
    public List<Interaction> interactions;

    [Header("Tools for testing")]
    public ItemData testItemToInteractWith;

    public bool Interact(ItemData otherItem)
    {
        if (interactions != null)
        {
            for (int i = 0; i < interactions.Count; i++)
            {
                bool wasInteractedWith = interactions[i].Interact(otherItem);
                if (wasInteractedWith)
                {
                    return wasInteractedWith;
                }
            }
        }
        return false;
    }

    public bool Interact(Item otherItem)
    {
        return Interact(otherItem.itemData);
    }
}
