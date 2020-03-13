using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraMoveSpeed = 2f;
    public static Transform target;
    public Transform startTarget;
    [Tooltip("Position relative to target")]
    public Transform startCameraFollowPosition;
    [Tooltip("xMin, yMin, xMax, yMax")]
    public Vector4 cameraBorder;
    public static Vector3 cameraFollowPosition = Vector3.zero;
    //[Tooltip("How far away the mouse has to be for the camera to move ")] 
    //public float mouseMovingDistance = 30; 
    private Vector3 followPosition;
    private Vector3 cameraMoveDir;

    private Camera cam;

    [Tooltip("Visual aid to know camera border in scene")]
    public Texture borderTexture;

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawGUITexture(new Rect(cameraBorder.x, cameraBorder.y, cameraBorder.z - cameraBorder.x, cameraBorder.w - cameraBorder.y), borderTexture);
    }

    private void Awake()
    {
        cam = Camera.main;
        if (startTarget)
        {
            target = startTarget;
            cameraFollowPosition = new Vector3(-target.position.x, -target.position.y);
        }
        cameraFollowPosition += new Vector3(startCameraFollowPosition.position.x, startCameraFollowPosition.position.y, transform.position.z);
    }

    // Update is called once per frame 
    void Update()
    {
        followPosition = cameraFollowPosition;

        if (target)
        {
            followPosition += new Vector3(target.position.x, target.position.y);
        }

        followPosition.x = Mathf.Clamp(followPosition.x, cameraBorder.x, cameraBorder.x + cameraBorder.z);
        followPosition.y = Mathf.Clamp(followPosition.y, cameraBorder.y, cameraBorder.y + cameraBorder.w);

        cameraMoveDir = (followPosition - transform.position).normalized;
        float distance = Vector3.Distance(followPosition, transform.position);

        if (distance > 0)
        {
            transform.position += cameraMoveDir * cameraMoveSpeed * distance * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(transform.position, followPosition);

            if (distanceAfterMoving > distance)
            {
                transform.position = followPosition;
            }
        }

        //Vector3 screenEdge = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));    //this...isnt working

        //if (screenEdge.x < cameraBorder.x)
        //{
        //    transform.position += new Vector3(cameraBorder.x - screenEdge.x, 0);
        //}

        //if (screenEdge.y < cameraBorder.y)
        //{
        //    transform.position += new Vector3(0, cameraBorder.y - screenEdge.y);
        //}

        //screenEdge = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        //if (screenEdge.x > cameraBorder.z)
        //{
        //    transform.position -= new Vector3(screenEdge.x - cameraBorder.z, 0);
        //}

        //if (screenEdge.y > cameraBorder.w)
        //{
        //    transform.position -= new Vector3(0, screenEdge.y - cameraBorder.w);
        //}

    }
}
