using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameUI;
using GameUtilities.Display;

public class EquipableWorldItemWithSingleSprite : WorldItem {
    private QuestsUI questsUI;
    public GameObject equipToPlayerPart;
    // Use this for initialization
    void Start() {
        inventoryScale = new Vector3(1f, 1f, 1f);
        rectTransform = GetComponent<RectTransform>();
        worldItems = FindObjectOfType<WorldItems>();
        if (transform.parent == worldItems.transform) {
            SetWorldDisplay();
        }
        else {
            SetInventoryDisplay();
        }

        questsUI = FindObjectOfType<QuestsUI>();
        if (transform.parent.GetComponent<PlayerEquipmentSlot>() != null) {
            EquipToPlayerModel();
        }
    }

    public override void EquipToPlayerModel() {
        SetChildrenActive(true);
        if (transform.GetChild(0).GetComponent<SpriteRenderer>().sprite != null) {
            equipToPlayerPart.GetComponent<SpriteRenderer>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        }
        else {
            equipToPlayerPart.GetComponent<SpriteRenderer>().sprite = null;
        }
        questsUI.CompleteEquipItemTaskPart(gameObject.name);
    }

    public override void UnequipFromPlayerModel() {
        equipToPlayerPart.GetComponent<SpriteRenderer>().sprite = null;
    }
}
