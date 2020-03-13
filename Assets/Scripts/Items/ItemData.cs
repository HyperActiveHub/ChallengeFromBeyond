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
    //need to keep track of if item is already picked-up, used or others. Prevent items from spawning in scenes if already used/picked up.

}