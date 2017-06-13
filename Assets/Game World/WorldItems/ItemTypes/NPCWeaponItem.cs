using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWeaponItem : MonoBehaviour {
    public float WeaponRangeX = 0.1f;
    public float BaseDamage;
    public DamageModifier[] DamageModifiers;

    public Vector2 GetWeaponRange() {
        return new Vector2(WeaponRangeX, WeaponRangeX * 0.8f);
    }
}
