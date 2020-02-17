using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    private static InventoryManager _instance = null;
    public static InventoryManager Instance { get { return _instance; } }

    [SerializeField] private List<Transform> slotTransforms;
    [SerializeField] private List<GameObject> inventorySlots = new List<GameObject>();

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        SortInventory();
    }

    public void AddItem(GameObject item)
    {
        if(item == null)
        {
            Debug.LogError("Tried to add null-item to inventory.");
            return;
        }

        if(inventorySlots.Count < slotTransforms.Count)
        {
            Item itemComponent = item.GetComponent<Item>();
            if(itemComponent != null)
            {
                itemComponent.isInInventory = true;
                inventorySlots.Add(item);
            }
        }
        else
        {
            Debug.LogWarning("Inventory full, can't add more items.");
        }

        //if (inventorySlots.Count > 0)
        //{
        //    inventorySlots[0].transform.position = slotTransforms[0].position;
        //}

        SortInventory();

    }

    public void RemoveItem(GameObject item)
    {
        inventorySlots.Remove(item);
        item.GetComponent<Item>().isInInventory = false;
        SortInventory();
    }

    public void SortInventory()
    {
  
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            //Debug.Log(string.Format("Sorting {0} to position {1}.", inventorySlots[i], slotTransforms[i].position));

            //TODO this is still broken, rotation is being applied but not the translation.
            inventorySlots[i].transform.position = slotTransforms[i].position;
            //inventorySlots[i].GetComponent<Rigidbody2D>().MovePosition(Vector3.zero);
            inventorySlots[i].transform.rotation = slotTransforms[i].rotation;
            inventorySlots[i].transform.localScale = new Vector3(3, 3, 3);

            //Debug.Break();
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < slotTransforms.Count; i++)
        {
            if(slotTransforms[i] != null)
            {
                Gizmos.DrawCube(slotTransforms[i].position, Vector3.one);
            }
        }
    }
}
