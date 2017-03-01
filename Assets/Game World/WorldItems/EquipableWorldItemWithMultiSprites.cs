using UnityEngine;
using UnityEngine.UI;
using GameUtilities.Display;
using GameUI;

/// <summary>
/// Implements some methods derived from WorldItem to deal with items that deal
/// with multiple sprites (e.g. body armour can be made of upper torso, upper 
/// arms, and lower arms parts). 
/// </summary>

public class EquipableWorldItemWithMultiSprites : WorldItem {
    private QuestsUI questsUI;
    public GameObject[] equipToPlayerParts;

    void Start() {
        inventoryScale = new Vector3(1f, 1f, 1f);
        rectTransform = GetComponent<RectTransform>();
        worldItems = FindObjectOfType<WorldItems>();
        if (transform.parent == worldItems.transform) {
            SetWorldDisplay();
        } else {
            SetInventoryDisplay();
        }

        questsUI = FindObjectOfType<QuestsUI>();
    }

    protected override void SetWorldDisplay() {
        SetChildrenActive(true);
        ImageLayerOrder.SetOrderOnTranformChildren(transform);
        GetComponent<Image>().enabled = false;
        GetComponent<GameWorldObjectSelector>().enabled = true;
    }

    protected override void SetInventoryDisplay() {
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = inventoryScale;
        SetChildrenActive(false);
        GetComponent<Image>().enabled = true;
        GetComponent<GameWorldObjectSelector>().enabled = false;
    }

    public override void EquipToPlayerModel() {
        SetChildrenActive(true);
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>().sprite != null) {
                equipToPlayerParts[i].GetComponent<SpriteRenderer>().sprite = transform.GetChild(i).GetComponent<SpriteRenderer>().sprite;
            } else {
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

    //TODO: put in namespace:
    private void SetChildrenActive(bool active) {
        foreach (Transform childTransform in transform) {
            childTransform.gameObject.SetActive(active);
        }
    }
}