using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(30.0f,Time.time*35.0f,60.0f));
    }
}
