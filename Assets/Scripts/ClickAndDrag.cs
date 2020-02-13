using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ClickAndDrag : MonoBehaviour
{
    // The plane the object is currently being dragged on
    private Plane dragPlane;

    // The difference between where the mouse is on the drag plane and 
    // where the origin of the object is on the drag plane
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 initialPosition;
    private Collider2D raycastCollider;

    void Start()
    {
        raycastCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        dragPlane = new Plane(mainCamera.transform.forward, transform.position);
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        offset = transform.position - camRay.GetPoint(planeDist) + Vector3.back;
        initialPosition = transform.position;
    }

    void OnMouseDrag()
    {
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        transform.position = camRay.GetPoint(planeDist) + offset;
    }

    private void OnMouseUp()
    {
        //When mouse is released
        //Cast ray from camera to game world
        //If ray collides and that collision is an interactible object:
        //Call interactableObject.Interact to see if the receiving item has an interaction-behaviour for the item currently hold by the player.
        //If interaction is valid, destroy this gameobject. The other gameobject is responsible for what is going to happen (instantiating a new object etc).

        raycastCollider.enabled = false;
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint + Vector3.back, worldPoint);

        if (hit.collider != null)
        {
            GameObject otherGameObject = hit.collider.gameObject;
            InteractableObject interactableObject = otherGameObject.GetComponent<InteractableObject>();

            Debug.DrawRay(worldPoint + Vector3.back, hit.point, Color.red, 2.0f);

            if (interactableObject != null)
            {
                if (interactableObject.Interact(GetComponent<Item>()))
                {
                    //Interaction ok, destroying this gameobject. (interactable.Interact() is responsible of what happens now... >:) )
                    Destroy(this.gameObject);
                    return;
                }
            }

            Debug.Log(hit.collider.gameObject);
        }

        raycastCollider.enabled = true;
        transform.position = initialPosition;
    }
}
