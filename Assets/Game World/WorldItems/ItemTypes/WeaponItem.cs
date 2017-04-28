using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipableWorldItemWithSingleSprite {
    public float WeaponRangeX = 0.1f;

    public Vector2 GetWeaponRange () {
        return new Vector2(WeaponRangeX, WeaponRangeX/2.5f);
    }

    void Awake() {
        equipToPlayerPart = FindObjectOfType<RightHandWearable>().gameObject;
    }
}
