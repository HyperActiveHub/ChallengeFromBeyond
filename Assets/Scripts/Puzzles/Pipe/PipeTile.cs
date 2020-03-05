using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeTile : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Sprite activeSprite;
    public float rotation = 0;
    //top, right, down, left
    //private PipeTile[] connectedTiles;
    public Vector2Int gridPosition;
    [SerializeField] private bool lockRotation = false;

    [SerializeField] public bool top = false;
    [SerializeField] public bool right = false;
    [SerializeField] public bool bottom = false;
    [SerializeField] public bool left = false;

    public bool activeFlow = false;
    public bool alreadyActive = false;

    SpriteRenderer sr = null;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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

        //bool temp = top;
        //top = right;
        //right = bottom;
        //bottom = left;
        //left = temp;

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
            sr.color = new Color(0, 0, 0, 0.5f);
        }
        else
        {
            sr.color = new Color(0, 0, 0, 1.0f);
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
