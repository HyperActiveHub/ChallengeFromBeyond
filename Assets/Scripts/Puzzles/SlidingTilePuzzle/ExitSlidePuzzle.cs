using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSlidePuzzle : MonoBehaviour
{

   public UnityEngine.Events.UnityEvent onClick;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(mousePos, Vector3.forward, out hit))
            {

                if (hit.collider == GetComponent<Collider>())
                {
                    onClick.Invoke();
                }
            }

        }
    }
}
