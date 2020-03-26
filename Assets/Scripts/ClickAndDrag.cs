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
    private Transform player;
    private Animator anim;
    const float interactAtPercent = 0.45f;

    [Tooltip("How much the player is allowed to move the gameobject and it will still register as a click.")]
    [SerializeField] private float clickTolerance = 1.0f;

    //[Tooltip("How far the object should move towards the camera while being moved. (Used to prevent unwanted intersections)")]
    //[SerializeField] private float zOffset = -1;

    void Start()
    {
        var playerCntrl = FindObjectOfType<PlayerController>();
        player = playerCntrl.transform;

        if (player == null)
            Debug.LogWarning("No player found. Items and interactables will not work.", this);
        else
            anim = playerCntrl.anim;

        mainCamera = Camera.main;
        rayCollider = GetComponent<BoxCollider2D>();
        interactableObjectComponent = GetComponent<InteractableObject>();
        itemComponent = GetComponent<Item>();
    }

    private void OnEnable()
    {
        GetComponent<SpriteOutline>().enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<SpriteOutline>().enabled = false;
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
        selectedObject = null;
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
        if (selectedObject == this.gameObject)
        {
            rayCollider.enabled = false;

            //May need to re-work GetGameObjectsUnderCursor.
            //Currently, when two or more items are on top of eachother, nothing happens when clicked (they try to interact with eachother)
            //Should pick up the fore-most item, i.e trigger self interaction.
            GameObject otherGameObject = GetGameObjectUnderCursor();    //Check if there's a gameObject under the currently selected object. (Trigger interaction)

            if (otherGameObject == null)    //There's no object under the selected object; trigger self interaction. (Player clicked on the object)
            {
                if (Vector2.Distance((Vector2)initialPosition, (Vector2)transform.position) < clickTolerance)   //Does this MouseUp register as a click?
                {
                    if (interactableObjectComponent != null && player.GetComponent<PlayerController>().CanPlayerInteract())
                    {
                        IEnumerator routine = TriggerWhenClose(interactableObjectComponent, gameObject, null);
                        StartCoroutine(routine);
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

                if (player.GetComponent<PlayerController>().CanPlayerInteract())
                {
                    IEnumerator routine = TriggerWhenClose(otherInteractableObject, otherGameObject, gameObject.GetComponent<Item>());
                    StartCoroutine(routine);
                }
                return; //is this needed?
            }
        }
        rayCollider.enabled = true;
    }

    /// <summary>
    /// Coroutine that:
    /// <para>1. Waits until the player is closer than 'minDist' to 'position' until the pickup-animation begins.</para>  
    /// <para>2. Then waits until the animation has elapsed a certain percentage until the interaction is invoked.</para>
    /// <para>OBS: if the player never reaches the min distance the interaction will never be invoked.</para>
    /// </summary>
    /// <param name="interaction">The interact method to invoke.</param>
    /// <param name="minDist">How small the distance between the player and 'position' can be before invoking the interact-method.</param>
    /// <param name="position">The position from which to check the distance.</param>
    /// <param name="otherItem">The other item that interacts with this interactable-object.</param>
    IEnumerator TriggerWhenClose(InteractableObject otherInteractable, GameObject otherGameObject, Item otherItem)
    {
        Vector2 position = otherGameObject.transform.position;
        Item item = otherGameObject.GetComponent<Item>();

        float minDist = otherInteractable.proximityRange;
        System.Func<Item, Interaction> interaction = otherInteractable.Interact;

        if (otherItem != null)  //if using an item on an interactable object (possibly another item)
        {
            ResetClick();
            if (otherItem.isInInventory == false)    //Shouldnt be able to use items that arent in inventory
            {
                yield break;
            }
        }

        if (item != null && otherItem != null)     //Both objects are items
        {
            if (item.isInInventory && otherItem.isInInventory)   //Both items are in ineventory (nowhere to walk to)
            {
                item.GetComponent<SpriteRenderer>().enabled = false;
                otherItem.GetComponent<SpriteRenderer>().enabled = false;
                anim.SetTrigger("isPickingUp");
            }
            else    //Only walk to the item if atleast one of the items isnt in inventory (if both items are in ineventory, theres nowhere to walk to)
            {
                player.GetComponent<PlayerController>().target.transform.position = position;
                yield return new WaitUntil(() => Vector2.Distance(player.position, position) <= minDist);
                anim.SetTrigger("isPickingUp");
            }
        }

        else    //Atleast one object isnt an item
        {

            player.GetComponent<PlayerController>().target.transform.position = position;
            yield return new WaitUntil(() => Vector2.Distance(player.position, position) <= minDist);

            if (item != null)
            {
                anim.SetTrigger("isPickingUp");
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > interactAtPercent && !anim.IsInTransition(0));

            }
            else if (otherInteractable.playPickupAnim)
            {
                anim.SetTrigger("isPickingUp");
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > interactAtPercent && !anim.IsInTransition(0));
            }
        }

        var action = interaction.Invoke(otherItem);

        if (action == null)
        {
            //The object could not be interacted with, inform the player.
            print("'That wont work..'");

            ResetClick();
        }
        else if (otherItem != null)    //if object was clicked (otherItem == null), nothing should be consumed
            ConsumeItem(action, otherGameObject, otherItem);
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
    ///Will consume the other item if the interaction is consumable
    ///</Summary>
    private void ConsumeItem(Interaction interaction, GameObject consumedItemObject, Item consumedItem)
    {
        if (interaction != null && interaction.consumable)
        {
            //var itemComp = consumedItem.GetComponent<Item>();
            if (itemComponent.isInInventory)
            {
                itemComponent.inventoryObject.RemoveFromInventory(gameObject);
            }
            Destroy(gameObject);
        }
    }
}