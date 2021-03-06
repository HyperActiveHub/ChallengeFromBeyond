﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool conditionMet;
    public bool isRightDir = true;
    public string NextRoom;
    public Vector2 spawnOffset = new Vector2(2, -1);


    public void ChangeScene()
    {
        if (conditionMet)
        {
            GameManagerScript.Instance.ChangeScene(NextRoom);
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
            GetComponent<ClickAndDrag>().enabled = false;
        }
        else
        {
            string say = GetComponent<InteractableObject>().inspectText;
            var interact = GetComponent<InteractableObject>();
            interact.inspectText = say;
            interact.InspectDialog();
        }
    }

    public void ConditionMet()
    {
        conditionMet = true;
    }

    public Vector2 GetSpawnPos()
    {
        return (Vector2)transform.position + spawnOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GetSpawnPos(), Vector3.one);
    }

    private void OnMouseEnter()
    {
        if (GetComponent<ClickAndDrag>().enabled)
            if (isRightDir)
                GameManagerScript.Instance.SetCursor(GameManagerScript.CursorType.rightCursor);
            else
                GameManagerScript.Instance.SetCursor(GameManagerScript.CursorType.leftCursor);
    }

    private void OnMouseExit()
    {
        GameManagerScript.Instance.SetCursor(GameManagerScript.CursorType.defaultCursor);
    }
}
