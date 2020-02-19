using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Custom Assets/Inventory")]
public class InventoryObject : ScriptableObject
{
    [SerializeField] private bool clearInventoryOnEnable = true;
    public List<GameObject> itemsInInventory;

    public UnityAction onInventoryUpdate = null;

    public void AddToInventory(GameObject item)
    {

        Item itemComponent = item.GetComponent<Item>();
        if (itemComponent == null)
        {
            Debug.LogError("Tried to add non-item object to inventory, this is not allowed.", this);
            return;
        }

        if (itemsInInventory.Contains(item))
        {
            Debug.Log("Tried to add already added item to inventory, this is not allowed.", this);
            return;
        }

        itemComponent.isInInventory = true;
        itemsInInventory.Add(item);

        if (onInventoryUpdate != null)
        {
            onInventoryUpdate.Invoke();
        }        
    }

    public void RemoveFromInventory(GameObject item)
    {
        if (!itemsInInventory.Contains(item))
        {
            Debug.Log(string.Format("Tried to remove {0} from inventory, it wasn't in the inventory to begin with.", item.name));
            return;
        }
        Item itemComponent = item.GetComponent<Item>();
        itemComponent.isInInventory = false;
        itemsInInventory.Remove(item);

        onInventoryUpdate.Invoke();
    }

    private void OnEnable()
    {
        if (clearInventoryOnEnable)
        {
            itemsInInventory = new List<GameObject>();
        }
    }
}