using UnityEngine;
using GameUI;
using GameUtilities.Display;
using UnityEngine.UI;

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
    protected void SetWorldDisplay() {
        SetChildrenActive(true);
        ImageLayerOrder.SetOrderOnTranformChildren(transform);
        GetComponent<Image>().enabled = false;
        GetComponent<GameWorldObjectSelector>().enabled = true;
    }

    protected void SetInventoryDisplay() {
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = inventoryScale;
        SetChildrenActive(false);
        GetComponent<Image>().enabled = true;
        GetComponent<GameWorldObjectSelector>().enabled = false;
    }

    protected void SetChildrenActive(bool active) {
        foreach (Transform childTransform in transform) {
            childTransform.gameObject.SetActive(active);
        }
    }

}