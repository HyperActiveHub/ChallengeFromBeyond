using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PipeTile : MonoBehaviour
{
    public Sprite sprite;
    public Sprite activeSprite;
    private SpriteRenderer sr = null;

    [SerializeField] private bool lockRotation = false;
    [SerializeField] private float rotation = 0;

    [HideInInspector] public bool top = false;
    [HideInInspector] public bool right = false;
    [HideInInspector] public bool bottom = false;
    [HideInInspector] public bool left = false;

    //if this tile is currently part of the flow
    [HideInInspector] public bool activeFlow = false;
    //if this tile was activated this validation pass
    [HideInInspector] public bool alreadyActive = false;

    [Tooltip("The required insight level in order to trigger win condition. This will only be applied if this pipe is attached to the \"Pipe Puzzle component\".")]
    [Range(0.0f,1.0f)]
    [SerializeField] public float requiredInsightLevel = 0.0f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rotation = transform.rotation.eulerAngles.z;
    }

    public void Rotate()
    {
        if (lockRotation)
        {
            return;
        }

        bool temp = top;
        top = left;
        left = bottom;
        bottom = right;
        right = temp;

        rotation = (rotation - 90.0f) % 360.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }

    public bool ValidateConnection(string direction)
    {
        if (alreadyActive)
        {
            return true;
        }

        if(top & direction == "top")
        {
            SetFlowState(true);
            return top;
        }

        if (right & direction == "right")
        {
            SetFlowState(true);
            return right;
        }

        if (bottom & direction == "bottom")
        {
            SetFlowState(true);
            return bottom;
        }

        if (left & direction == "left")
        {
            SetFlowState(true);
            return left;
        }
        return false;
    }

    public void SetFlowState(bool newState)
    {
        activeFlow = newState;

        if (activeFlow)
        {
            alreadyActive = true;
            if(sr != null && activeSprite != null)
            {
                sr.sprite = activeSprite;
            }
        }
        else
        {
            if(sr != null && activeSprite != null)
            {
                sr.sprite = sprite;
            }
        }
    }

    public void SetRotationMask(bool top, bool right, bool bottom, bool left)
    {
        this.top = top;
        this.right = right;
        this.bottom = bottom;
        this.left = left;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PipeTile))]
public class PipeTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Rotate"))
        {
            PipeTile pipeTile = (PipeTile)target;
            pipeTile.Rotate();
        }
    }
}
#endif