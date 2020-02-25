using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBullshit : MonoBehaviour
{

    public Transform target;
    public Vector2 size;
    public float speed;

    private void Start()
    {
        
    }

    void Update()
    {
        Vector3 pos = new Vector3(
            Mathf.RoundToInt(target.position.x / size.x)*size.x, Mathf.RoundToInt(target.position.y / size.y) * size.y, transform.position.z
           );
        transform.position = Vector3.Lerp(transform.position, pos, speed);
    }
}
