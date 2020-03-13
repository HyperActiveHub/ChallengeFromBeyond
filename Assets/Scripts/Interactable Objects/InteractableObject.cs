using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteOutline))]
[RequireComponent(typeof(ClickAndDrag))]
public class InteractableObject : MonoBehaviour
{
    [Header("Inspect")]
    [Tooltip("How close the player must be to interact with this object")]
    [Range(0.5f, 5)] public float proximityRange = 1;
    [Tooltip("Drag the flowchart here. It needs to contain a block with a 'Say' command.")]
    [SerializeField] Fungus.Say sayObject;
    [TextArea] public string inspectText;
    [Tooltip("How much insight is gained when inspecting this object (first level).")]
    [SerializeField] float insightWorth;
    bool hasAddedInsight;

    [Header("Interactions")]
    public List<Interaction> interactions = new List<Interaction>();


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityRange);
    }

    public void SetInspectText(string newInspectText)
    {
        inspectText = newInspectText;
    }

    public void InspectDialog()
    {
        sayObject.SetStandardText(inspectText);
        sayObject.Execute();
    }

    public void AddInsight()
    {
        if (!hasAddedInsight)
        {
            InsightGlobal.AddInsight(insightWorth, this);
            Debug.Log("insight: " + InsightGlobal.InsightValue * 100 + "%");
            hasAddedInsight = true;
        }
    }

    public void SetInsightWorth(float newValue)
    {
        insightWorth = newValue;
        hasAddedInsight = false;
    }

    public void PlaySoundIfEnabled()
    {
        var obj = GetComponent<FMODUnity.StudioEventEmitter>();
        if (obj.enabled)
        {
            if(obj.IsPlaying())
            {
                obj.Stop();
            }
            obj.Play();
        }
    }

    public Interaction Interact(Item otherItem)
    {
        if (interactions != null)
        {
            for (int i = 0; i < interactions.Count; i++)
            {
                Interaction currentInteraction = interactions[i];

                bool wasInteractedWith = interactions[i].Interact(otherItem);
                if (wasInteractedWith)
                {
                    return interactions[i];
                }
            }
        }
        return null;
    }

    private void Start()
    {
        SetInspectText(inspectText);
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.size = GetComponent<SpriteRenderer>().bounds.size;      //Set the box collider to be the same as the sr bounds
    }

    public void DisableGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    public void EnableGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }

    public void ToggleGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
