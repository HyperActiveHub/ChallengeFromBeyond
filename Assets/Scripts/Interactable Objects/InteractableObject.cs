﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteOutline))]
public class InteractableObject : MonoBehaviour
{
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
        if(gameObject != null)
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
