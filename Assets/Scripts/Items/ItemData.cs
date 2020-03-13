using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Custom Assets/Item")]
public class ItemData : ScriptableObject
{
    [HideInInspector] public int itemID = 0;
    public Sprite itemSprite;
    [Tooltip("Will change to this sprite when item is added to inventory. Will not replace sprite if left empty.")]
    public Sprite itemSpriteInventory;
    public string displayText;
    [HideInInspector] public InteractableObject interactableData;


}