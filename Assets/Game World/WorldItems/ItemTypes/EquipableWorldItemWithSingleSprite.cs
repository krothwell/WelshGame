using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameUI;
using GameUtilities.Display;

public abstract class EquipableWorldItemWithSingleSprite : WorldItem {
    public GameObject equipToPlayerPart;
    // Use this for initialization
    protected override void Start() {
        base.Start();
        SetPlayerPart();
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

    protected abstract void SetPlayerPart();
}
