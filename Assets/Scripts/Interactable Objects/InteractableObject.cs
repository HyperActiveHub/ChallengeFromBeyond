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
    public bool playAnim;
    public string animTriggerName = "";
    [Tooltip("This value specifies the percentage of the animation at which the interaction should happen.")]
    [Range(0.2f, 0.9f)]
    public float interactAtAnimProgress = 0.5f;
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

    private void OnMouseEnter()
    {
        if (GetComponent<ClickAndDrag>().enabled)
        {
            if (GetComponent<Item>() == null && GetComponent<DoorScript>() == null)
            {
                if (CompareTag("Puzzle"))
                {
                    GameManagerScript.Instance.SetCursor(GameManagerScript.CursorType.puzzleCursor);
                }
                else
                    GameManagerScript.Instance.SetCursor(GameManagerScript.CursorType.inspectCursor);
            }
        }
    }

    private void OnMouseExit()
    {
        GameManagerScript.Instance.SetCursor(GameManagerScript.CursorType.defaultCursor);
    }

    private void OnMouseUpAsButton()
    {
        GameManagerScript.Instance.SetCursor(GameManagerScript.CursorType.defaultCursor);
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
            if (obj.IsPlaying())
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
        col.size = Vector2.Scale(Vector2.one / transform.localScale, col.size); //also take the scale of the object into account

        if (animTriggerName == "")
        {
            playAnim = false;
        }

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
