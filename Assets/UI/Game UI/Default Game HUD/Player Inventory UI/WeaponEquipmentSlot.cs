using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI {
    public class WeaponEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquipped() {
            return transform.GetComponentInChildren<WeaponItem>();
        }
    }
}