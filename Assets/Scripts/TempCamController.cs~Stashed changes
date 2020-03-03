using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCamController : MonoBehaviour
{
    public Transform followObj;
    public Vector2 offset;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector2 targetPos = new Vector2(followObj.position.x + offset.x, followObj.position.y + offset.y);

        transform.position = new Vector3(targetPos.x, transform.position.y, transform.position.z);

    }
}
