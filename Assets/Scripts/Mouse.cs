﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private SpriteRenderer renderer;
    [System.Serializable]
    public enum mouseSprite
    {
        Default,
        Bag,
        Bubble,
        BubbleText,
        Magnifier,
        Front,
        Left,
        Right,
        Puzzle
    }

    public Sprite Default, Bag, Bubble, BubbleText, Front, Magnifier, Left, Right, Puzzle;

    public static mouseSprite mouse;
    public mouseSprite previousMouse;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
        mouse = previousMouse;
        switch (mouse)
        {
            case mouseSprite.Default:
                renderer.sprite = Default;
                break;
            case mouseSprite.Bag:
                renderer.sprite = Bag;
                break;
            case mouseSprite.Bubble:
                renderer.sprite = Bubble;
                break;
            case mouseSprite.BubbleText:
                renderer.sprite = BubbleText;
                break;
            case mouseSprite.Magnifier:
                renderer.sprite = Magnifier;
                break;
            case mouseSprite.Front:
                renderer.sprite = Front;
                break;
            case mouseSprite.Left:
                renderer.sprite = Left;
                break;
            case mouseSprite.Right:
                renderer.sprite = Right;
                break;
            case mouseSprite.Puzzle:
                renderer.sprite = Puzzle;
                break;
            default:
                Debug.Log("NOTHING FOR MOUSE");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (previousMouse != mouse)
        {
            switch (mouse)
            {
                case mouseSprite.Default:
                    renderer.sprite = Default;
                    break;
                case mouseSprite.Bag:
                    renderer.sprite = Bag;
                    break;
                case mouseSprite.Bubble:
                    renderer.sprite = Bubble;
                    break;
                case mouseSprite.BubbleText:
                    renderer.sprite = BubbleText;
                    break;
                case mouseSprite.Magnifier:
                    renderer.sprite = Magnifier;
                    break;
                case mouseSprite.Front:
                    renderer.sprite = Front;
                    break;
                case mouseSprite.Left:
                    renderer.sprite = Left;
                    break;
                case mouseSprite.Right:
                    renderer.sprite = Right;
                    break;
                case mouseSprite.Puzzle:
                    renderer.sprite = Puzzle;
                    break;
                default:
                    Debug.Log("NOTHING FOR MOUSE");
                    break;
            }
            previousMouse = mouse;
        }
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }
}
