using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraMoveSpeed = 1f;
    public Transform player;
    [Tooltip("Positionen relativt spelaren")]
    public Vector3 cameraFollowPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraMoveDir = (cameraFollowPosition + player.position - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);

        transform.position += cameraMoveDir * cameraMoveSpeed * distance * Time.deltaTime;
    }
}
