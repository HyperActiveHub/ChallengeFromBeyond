using UnityEngine;
using UnityEditor;

public class HelperToolsEditor : EditorWindow
{
    private Object userSelectedItemData = null;
    private bool addItemComponent = true;
    private bool addInteractableObject = true;
    private bool overrideItemData = false;
    [SerializeField] private InventoryObject inventoryObject = null;

    [MenuItem("Window/Helper Tools")]
    public static void ShowWindow()
    {
        GetWindow<HelperToolsEditor>("Helper Tools");
    }

    private void OnGUI()
    {

        addItemComponent = GUILayout.Toggle(addItemComponent, "Add Item Component");
        addInteractableObject = GUILayout.Toggle(addInteractableObject, "Add Interactable Object Component");
        overrideItemData = GUILayout.Toggle(overrideItemData, "Override ItemData");

        if (overrideItemData)
        {
            addItemComponent = true;
            userSelectedItemData = EditorGUILayout.ObjectField("Override with Item:", userSelectedItemData, typeof(ItemData), false);
        }
        else
        {
            userSelectedItemData = null;
        }

        if (GUILayout.Button("Add item components to selected objects"))
        {
            GameObject[] gameObjects = Selection.gameObjects;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                //If "Add Item Component" is ticked and the object does not have an item component attached.
                if(addItemComponent)
                {
                    Item selectedItem;
                    bool hasItemComponent = gameObjects[i].TryGetComponent<Item>(out selectedItem);

                    if (!hasItemComponent)
                    {
                        selectedItem = gameObjects[i].AddComponent<Item>();
                        if (userSelectedItemData != null)
                        {
                            selectedItem.SetItem((ItemData)userSelectedItemData);
                        }
                    }
                    else //Else, if the gameobject has an item component, replace the itemdata with selected itemdata.
                    {
                        gameObjects[i].GetComponent<Item>().SetItem((ItemData)userSelectedItemData);
                    }
                }

                //If "Add Interactable Object Component" is ticked and the object does not have an interactable object component attached.
                if (addInteractableObject)
                {
                    InteractableObject selectedInteractableObject = gameObjects[i].GetComponent<InteractableObject>();
                    if (selectedInteractableObject == null)
                    {
                        gameObjects[i].AddComponent<BoxCollider2D>();
                        gameObjects[i].AddComponent<InteractableObject>();
                    }
                }
            }
        }

        GUILayout.FlexibleSpace();

        inventoryObject = (InventoryObject)EditorGUILayout.ObjectField("Inventory Object", inventoryObject, typeof(InventoryObject), true);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add to inventory"))
        {
            if(inventoryObject == null)
            {
                Debug.LogError("No Inventory Object selected, please attach one and try again.");
                return;
            }

            GameObject[] selectedObjects = Selection.gameObjects;
            for (int i = 0; i < selectedObjects.Length; i++)
            {
                inventoryObject.AddToInventory(selectedObjects[i]);
            }
        }
        if (GUILayout.Button("Remove from inventory"))
        {
            if (inventoryObject == null)
            {
                Debug.LogError("No Inventory Object selected, please attach one and try again.");
                return;
            }

            GameObject[] selectedObjects = Selection.gameObjects;
            for (int i = 0; i < selectedObjects.Length; i++)
            {
                inventoryObject.RemoveFromInventory(selectedObjects[i]);
            }
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Force inventory update"))
        {
            if (inventoryObject == null)
            {
                Debug.LogError("No Inventory Object selected, please attach one and try again.");
                return;
            }
            inventoryObject.onInventoryUpdate();
        }
    }
}
