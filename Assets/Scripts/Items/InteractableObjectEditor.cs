using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(InteractableObject))]
public class InteractableObjectEditor : Editor
{
    InteractableObject interactableObject;

    public override void OnInspectorGUI()
    {
        if (interactableObject == null)
        {
            interactableObject = (InteractableObject)target;
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add interaction"))
        {
            AddItemInteraction();
        }
        if (GUILayout.Button("Remove interaction"))
        {
            RemoveItemInteraction();
        }
        GUILayout.EndHorizontal();

        base.OnInspectorGUI();

        //if (GUILayout.Button("Interact"))
        //{
        //    Interact(interactableObject.testItemToInteractWith);
        //}
    }

    private void AddItemInteraction()
    {
        interactableObject.interactions.Add(new Interaction());
    }

    private void RemoveItemInteraction()
    {
        int interactablesCount = interactableObject.interactions.Count;
        if (interactablesCount > 0)
        {
            interactableObject.interactions.RemoveAt(interactableObject.interactions.Count - 1);
        }
    }

    //private void Interact(ItemData item)
    //{
    //    interactableObject.Interact(item);
    //}
}
