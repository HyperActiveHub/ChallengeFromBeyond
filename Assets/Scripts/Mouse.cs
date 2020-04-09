using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
      private SpriteRenderer sr;
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



    public Sprite Default, Bag, Bubble, BubbleText, Magnifier, Up, Left, Right, Puzzle;

    public static mouseSprite mouse;
    public mouseSprite previousMouse;

    public Vector2 defaultOffset, bagOffset, bubbleOffset, bubbleTextOffset, magnifierOffset, upOffset, leftOffset, rightOffset, puzzleOffset;
    private Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
        mouse = previousMouse;
        switch (mouse)
        {
            case mouseSprite.Default:
                sr.sprite = Default;
                offset = defaultOffset;
                break;
            case mouseSprite.Bag:
                sr.sprite = Bag;
                offset = bagOffset;
                break;
            case mouseSprite.Bubble:
                sr.sprite = Bubble;
                offset = bubbleOffset;
                break;
            case mouseSprite.BubbleText:
                sr.sprite = BubbleText;
                offset = bubbleTextOffset;
                break;
            case mouseSprite.Magnifier:
                sr.sprite = Magnifier;
                offset = magnifierOffset;
                break;
            case mouseSprite.Front:
                sr.sprite = Up;
                offset = upOffset;
                break;
            case mouseSprite.Left:
                sr.sprite = Left;
                offset = leftOffset;
                break;
            case mouseSprite.Right:
                sr.sprite = Right;
                offset = rightOffset;
                break;
            case mouseSprite.Puzzle:
                sr.sprite = Puzzle;
                offset = puzzleOffset;
                break;
            default:
                Debug.Log("NOTHING FOR MOUSE");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Cursor.visible)
        //{
        //    Cursor.visible = false;
        //}



        if (previousMouse != mouse)
        {
            switch (mouse)
            {
                case mouseSprite.Default:
                    sr.sprite = Default;
                    break;
                case mouseSprite.Bag:
                    sr.sprite = Bag;
                    break;
                case mouseSprite.Bubble:
                    sr.sprite = Bubble;
                    break;
                case mouseSprite.BubbleText:
                    sr.sprite = BubbleText;
                    break;
                case mouseSprite.Magnifier:
                    sr.sprite = Magnifier;
                    break;
                case mouseSprite.Front:
                    sr.sprite = Up;
                    break;
                case mouseSprite.Left:
                    sr.sprite = Left;
                    break;
                case mouseSprite.Right:
                    sr.sprite = Right;
                    break;
                case mouseSprite.Puzzle:
                    sr.sprite = Puzzle;
                    break;
                default:
                    Debug.Log("NOTHING FOR MOUSE");
                    break;
            }
            previousMouse = mouse;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (offset != null)
        {
            mousePos += offset;
        }
        transform.position = mousePos;
    }
}
