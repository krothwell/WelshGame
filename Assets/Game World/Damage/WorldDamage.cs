using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDamage {
    private float baseWeaponDamage;
    public float BaseWeaponDamage {
        get { return baseWeaponDamage; }
        set { baseWeaponDamage = value; }
    }

    public float CalculateDamage() {
        return baseWeaponDamage;
    }
}
