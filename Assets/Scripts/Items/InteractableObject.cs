using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
{
    public List<Interaction> interactions;
    public ItemData testItemToInteractWith;

    public void Interact(ItemData otherItem)
    {
        if (interactions != null)
        {
            for (int i = 0; i < interactions.Count; i++)
            {
                interactions[i].Interact(otherItem);
            }
        }
    }
}
