using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    [HideInInspector] public int itemID = 0;
    public Sprite itemSprite;
    public string displayText;

    public ItemData combinedResult;

}