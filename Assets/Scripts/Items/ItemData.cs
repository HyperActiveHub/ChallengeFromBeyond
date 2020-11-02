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
    public bool isUsed;

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;    //without this, the asset will be unloaded if the current scene doesnt contian a reference to it, meaning changed data is lost 
                                                        //(isPickedUp is reset to false once the asset is present in the current scene).

    }

    public void Reset()
    {
        isUsed = false;
        Debug.LogWarning("Unregistered as picked up", this);
    }
}