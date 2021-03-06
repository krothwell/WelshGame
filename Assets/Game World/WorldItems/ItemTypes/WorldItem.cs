﻿using UnityEngine;
using GameUI;
using GameUtilities.Display;
using UnityEngine.UI;

/// <summary>
/// World item types derived from this class are items that are part of the game 
/// world. Game world items switch appearance between their in-world (sprites)
/// and inventory (UI image icon) displays. A world item has a type which is 
/// checked to see if/where it can be equipped by the player. 
/// </summary>
public abstract class WorldItem : MonoBehaviour {
    public WorldItems.WorldItemTypes itemType;
    protected WorldItems worldItems;
    protected Vector3 inventoryScale;
    protected RectTransform rectTransform;
    protected QuestsUI questsUI;
    protected PlayerInventorySlots playerInventorySlots;

    private void Awake() {
        playerInventorySlots = FindObjectOfType<PlayerInventorySlots>();
    }

    protected virtual void Start() {
        inventoryScale = new Vector3(1f, 1f, 1f);
        rectTransform = GetComponent<RectTransform>();
        worldItems = FindObjectOfType<WorldItems>();
        if (transform.parent == worldItems.transform) {
            SetItemModeForWorld();
        }
        else {
            SetItemModeForInventory();
        }
        questsUI = FindObjectOfType<QuestsUI>();
    }

    public void GetPickedUp () {
        Debug.Log(playerInventorySlots);
        playerInventorySlots.ReceiveItem(this);
        SetItemModeForInventory();
    }

    public WorldItems.WorldItemTypes GetMyItemType() {
        return itemType;
    }

    /// <summary>
    /// changed the properties of the item so that it displays appropriately with game
    /// world scenery.
    /// </summary>
    protected void SetItemModeForWorld() {
        SetWorldSpritesActive(true);
        ImageLayerOrder.SetOrderOnTranformChildren(transform);
        GetComponent<Image>().enabled = false;
        GetComponent<GameWorldSelector>().enabled = true;
    }

    /// <summary>
    /// Changes the properties of the item so that it displays appropriately in the 
    /// inventory.
    /// </summary>
    protected void SetItemModeForInventory() {
        rectTransform.sizeDelta = new Vector2(20f, 20f);
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = inventoryScale;
        SetWorldSpritesActive(false);
        GetComponent<Image>().enabled = true;
        GetComponent<GameWorldSelector>().enabled = false;
    }

    /// <summary>
    /// Activates / deactivates the game objects that contain sprites attached to child 
    /// objects which will be used to display in the world
    /// </summary>
    /// <param name="active"></param>
    public void SetWorldSpritesActive(bool active) {
        foreach (Transform childTransform in transform) {
            childTransform.gameObject.SetActive(active);
        }
    }



}