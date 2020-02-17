using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
{
    public List<Interaction> interactions;

    [Header("Tools for testing")]
    public ItemData testItemToInteractWith;

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

                //if (otherItem == null)
                //{
                //    bool wasInteractedWith = interactions[i].Interact(null);
                //    if (wasInteractedWith)
                //    {
                //        if (currentInteraction.consumable)
                //        {
                //            Debug.Log("Destroyed was called,");
                //            Destroy(this.gameObject);
                //        }
                //        return interactions[i];
                //    }
                //}
                //else
                //{
                //    bool wasInteractedWith = interactions[i].Interact(otherItem);
                //    if (wasInteractedWith)
                //    {
                //        if (currentInteraction.consumable)
                //        {
                //            Debug.Log("Destroyed was called,");
                //            Destroy(this.gameObject);
                //        }
                //        return interactions[i];
                //    }
                //}
            }
        }
        return null;
    }
}
