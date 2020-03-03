using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class ClickAndDrag : MonoBehaviour
{
    // The difference between where the mouse is on the drag plane and 
    // where the origin of the object is on the drag plane
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 initialPosition;
    private GameObject selectedObject = null;
    private BoxCollider2D rayCollider = null;
    private InteractableObject interactableObjectComponent = null;
    private Item itemComponent = null;

    [Tooltip("How much the player is allowed to move the gameobject and it will still register as a click.")]
    [SerializeField] private float clickTolerance = 13.0f;

    //[Tooltip("How far the object should move towards the camera while being moved. (Used to prevent unwanted intersections)")]
    //[SerializeField] private float zOffset = -1;

    void Start()
    {
        mainCamera = Camera.main;
        rayCollider = GetComponent<BoxCollider2D>();
        interactableObjectComponent = GetComponent<InteractableObject>();
        itemComponent = GetComponent<Item>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown();
        }

        if (Input.GetMouseButton(0))
        {
            MouseGrab();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
    }

    private void MouseDown()
    {
        //Discard future actions if selectedObject is this gameObject.
        selectedObject = GetGameObjectUnderCursor();
        initialPosition = transform.position;

        if (selectedObject == this.gameObject)
        {
            offset = new Vector3(0, 0, (transform.position.z - mainCamera.transform.position.z));//+ zOffset);  
            //Cant add zOffset since it will move the object by the offset on each mouse click. (eventually it wont be visible anymore).
        }
    }

    private void MouseGrab()
    {
        if (selectedObject == this.gameObject && GetComponent<Item>() != null && GetComponent<Item>().isInInventory)
        {
            transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }

    private void MouseUp()
    {
        Interaction interaction = null;
        if (selectedObject == this.gameObject)
        {
            rayCollider.enabled = false;

            //This needs to be re-worked. Interactions with other objects should only happen if the selected one is in inventory.
            //Currently, when two or more items are on top of eachother, nothing happens when clicked(/or they probably try to interact with eachother?)
            //Should pick up the fore-most item, i.e trigger self interaction.
            
            //Check if there's a gameObject under the currently selected object. (Trigger interaction)
            GameObject otherGameObject = GetGameObjectUnderCursor();

            if (otherGameObject == null) //There's no object under the selected object; trigger self interaction. (Player clicked on the object)
            {
                if (Vector2.Distance((Vector2)initialPosition, (Vector2)transform.position) < clickTolerance)
                {
                    //Trigger interaction with self.
                    if (interactableObjectComponent != null)
                    {
                        interaction = interactableObjectComponent.Interact(null);
                        ConsumeItem(interaction);   //Should never consume if interaction is pickup... 
                                                    //and should anything ever be consumed when otherItem is null? i.e clicked on something
                        ResetClick();
                    }
                }
                else
                {
                    ResetClick();
                }
            }
            else //Trigger interaction with other gameobject.
            {
                InteractableObject otherInteractableObject = otherGameObject.GetComponent<InteractableObject>();
                if (otherInteractableObject != null)
                {
                    interaction = otherInteractableObject.Interact(gameObject.GetComponent<Item>());
                    if (interaction == null)
                    {
                        ResetClick();
                        return;
                    }
                    ConsumeItem(interaction);
                }
                return;
            }
        }
        rayCollider.enabled = true;
    }

    /// <summary>
    /// Resets the selected object after being dragged by the player.
    /// </summary>
    private void ResetClick()
    {
        selectedObject = null;
        rayCollider.enabled = true;

        if (GetComponent<Item>() != null && GetComponent<Item>().isInInventory)
        {
            transform.localPosition = Vector3.zero;
        }
        else
            transform.position = initialPosition;
    }

    /// <summary>
    /// Return the GameObject that the cursor is hovering over. Returns null if no object was found.
    /// </summary>
    private GameObject GetGameObjectUnderCursor()   //This needs changes..
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit)
        {
            if (hit.collider.GetComponent<InteractableObject>() != null)
            {
                return hit.collider.gameObject;
            }
            else
                return null;
        }
        else
        {
            return null;
        }

        #region proposed rework, WIP
        //RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //int frontObjectIndex = 0, lastSortOrder = int.MaxValue;

        //for (int i = 0; i < hits.Length; i++)
        //{
        //    SpriteRenderer sr = hits[i].collider.GetComponent<SpriteRenderer>();

        //    if (sr.sortingOrder < lastSortOrder)
        //    {
        //        lastSortOrder = sr.sortingOrder;
        //        frontObjectIndex = i;
        //    }
        //}

        //if (hits.Length != 0)
        //{
        //    //Outline this object (GetComponent<SpriteOutline>())
        //    return hits[frontObjectIndex].collider.gameObject;
        //}
        //else
        //{
        //    return null;
        //}
        #endregion

    }

    ///<Summary>
    ///Will consume the item if it is consumable, will also remove this item from the Inventory (if possible).
    ///</Summary>
    private void ConsumeItem(Interaction interaction)
    {
        if (interaction != null && interaction.consumable)
        {
            if (itemComponent.isInInventory)
            {
                itemComponent.inventoryObject.RemoveFromInventory(this.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}