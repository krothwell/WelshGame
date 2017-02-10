using UnityEngine;
using GameUI;

/// <summary>
/// World item types derived from this class are items that are part of the game 
/// world. Game world items switch appearance between their in-world (sprites)
/// and inventory (UI image icon) displays. A world item has a type which is 
/// checked to see if/where it can be equipped by the player. 
/// </summary>
//TODO: add a new abstract class from this called EquipableWorldItem
//TODO: Move Equip/Unequip functions to this class, derive 
//TODO: WorldItemWithMultipleSprites from this
public abstract class WorldItem : MonoBehaviour {
    public WorldItems.WorldItemTypes itemType;
    protected WorldItems worldItems;
    protected Vector3 inventoryScale;
    protected RectTransform rectTransform;

    protected PlayerInventoryUI playerInventory;

    public void GetPickedUp () {
        playerInventory = FindObjectOfType<PlayerInventoryUI>();
        playerInventory.RecieveItem(this);
        SetInventoryDisplay();
    }

    //TODO: check if this is better staying with player class or implementing
    //here.
    public void OnMouseUp() {
    }

    public WorldItems.WorldItemTypes GetMyItemType() {
        return itemType;
    }

    public abstract void EquipToPlayerModel();
    public abstract void UnequipFromPlayerModel();
    protected abstract void SetWorldDisplay();
    protected abstract void SetInventoryDisplay();
}