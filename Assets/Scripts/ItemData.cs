using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 1)]
public class ItemData : ScriptableObject
{
    [SerializeField] public int itemID = 0;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private string displayText;
}