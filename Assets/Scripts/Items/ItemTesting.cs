using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTesting : MonoBehaviour
{
    [SerializeField] private Item item1 = null;
    [SerializeField] private Item item2 = null;

    void Start()
    {
        if(item1 == item2)
        {
            Debug.Log(item1);
        }
    }
    public void TestFunctionCall()
    {

    }

}
