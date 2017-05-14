using UnityEngine;

using GameUI;

/// <summary>
/// Implements some methods derived from WorldItem to deal with items that deal
/// with multiple sprites (e.g. body armour can be made of upper torso, upper 
/// arms, and lower arms parts). 
/// </summary>

public class EquipableWorldItemWithMultiSprites : WorldItem {
    private QuestsUI questsUI;
    public GameObject[] equipToPlayerParts;

    protected void Start() {
        inventoryScale = new Vector3(1f, 1f, 1f);
        rectTransform = GetComponent<RectTransform>();
        worldItems = FindObjectOfType<WorldItems>();
        if (transform.parent == worldItems.transform) {
            SetWorldDisplay();
        } else {
            SetInventoryDisplay();
        }
        questsUI = FindObjectOfType<QuestsUI>();
        if (transform.parent.GetComponent<PlayerEquipmentSlot>() != null) {
            EquipToPlayerModel();
        }
    }

    public override void EquipToPlayerModel() {
        
        SetChildrenActive(true);
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>().sprite != null) {
                equipToPlayerParts[i].GetComponent<SpriteRenderer>().sprite = transform.GetChild(i).GetComponent<SpriteRenderer>().sprite;
                equipToPlayerParts[i].GetComponent<SpriteRenderer>().material = transform.GetChild(i).GetComponent<SpriteRenderer>().material;
            }
            else {
                equipToPlayerParts[i].GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        questsUI.CompleteEquipItemTaskPart(gameObject.name);
        
    }

    public override void UnequipFromPlayerModel() {
        for (int i = 0; i < equipToPlayerParts.Length; i++) {
            equipToPlayerParts[i].GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    
}