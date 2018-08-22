using UnityEngine;

using GameUI;

/// <summary>
/// Implements some methods derived from WorldItem to deal with items that deal
/// with multiple sprites (e.g. body armour can be made of upper torso, upper 
/// arms, and lower arms parts). 
/// </summary>

public abstract class EquipableWorldItemWithMultiSprites : WorldItem {
    public GameObject[] equipToPlayerParts;

    protected override void Start() {
        base.Start();
        SetPlayerParts();
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

    protected abstract void SetPlayerParts();

    
}