using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraMoveSpeed = 50f;
    public static Transform target;
    public Transform startTarget;
    [Tooltip("Position relative to target")]
    public Transform startCameraFollowPosition;
    [Tooltip("xPos, yPos, xLength, yLength")]
    public Vector4 cameraBorder;
    public static Vector3 cameraFollowPosition = Vector3.zero;
    //[Tooltip("How far away the mouse has to be for the camera to move ")]
    //public float mouseMovingDistance = 30;
    private Vector3 followPosition;
    private Vector3 cameraMoveDir;

    [Tooltip("Visual aid to know camera border in scene")]
    public Texture borderTexture;

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawGUITexture(new Rect(cameraBorder.x, cameraBorder.y, cameraBorder.z, cameraBorder.w), borderTexture);
    }

    private void Awake()
    {
        if (startTarget)
        {
            target = startTarget;
            cameraFollowPosition = new Vector3(-target.position.x, -target.position.y);
        }
        cameraFollowPosition += new Vector3 (startCameraFollowPosition.position.x, startCameraFollowPosition.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        followPosition = cameraFollowPosition;

        if (target)
        {
            followPosition += new Vector3(target.position.x, target.position.y);
        }

        /*Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Vector3.Distance())
        {

        }*/

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
        
    }
}
