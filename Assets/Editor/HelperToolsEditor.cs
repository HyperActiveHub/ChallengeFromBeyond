using UnityEngine;
using UnityEditor;

public class HelperToolsEditor : EditorWindow
{
    private Object obj = null;
    private bool addItemComponent = true;
    private bool addInteractableObject = true;

    [MenuItem("Window/Helper Tools")]
    public static void ShowWindow()
    {
        GetWindow<HelperToolsEditor>("Helper Tools");
    }

    private void OnGUI()
    {

        addItemComponent = GUILayout.Toggle(addItemComponent, "Add Item Component");
        addInteractableObject = GUILayout.Toggle(addInteractableObject, "Add Interactable Object Component");

        if (GUILayout.Button("Add item components to selected objects"))
        {
            Debug.Log(obj);
            GameObject[] gameObjects = Selection.gameObjects;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].GetComponent<Item>();
            }
        }

        obj = EditorGUILayout.ObjectField("Item Data", obj, typeof(ItemData), false);
    }
}
