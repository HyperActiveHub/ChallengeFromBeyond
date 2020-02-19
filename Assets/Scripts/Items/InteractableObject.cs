using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
{
    public List<Interaction> interactions;

    public Interaction Interact(Item otherItem)
    {
        if (interactions != null)
        {
            for (int i = 0; i < interactions.Count; i++)
            {
                Interaction currentInteraction = interactions[i];

                bool wasInteractedWith = interactions[i].Interact(otherItem);
                if (wasInteractedWith)
                {
                    return interactions[i];
                }
            }
        }
        return null;
    }
}
