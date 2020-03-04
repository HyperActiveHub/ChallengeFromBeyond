using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteOutline))]
[RequireComponent(typeof(ClickAndDrag))]
public class InteractableObject : MonoBehaviour
{
    [Tooltip("How close the player must be to interact with this object")]
    [Range(0.5f, 5)] public float proximityRange = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityRange);
    }


    public List<Interaction> interactions = new List<Interaction>();

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

    private void Start()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.size = GetComponent<SpriteRenderer>().bounds.size;      //Set the box collider to be the same as the sr bounds
    }

    public void DisableGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    public void EnableGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }

    public void ToggleGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
