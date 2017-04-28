using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggingsItem : EquipableWorldItemWithMultiSprites {
    new void Start() {
        equipToPlayerParts[0] = FindObjectOfType<LowerTorsoWearable>().gameObject;
        equipToPlayerParts[1] = FindObjectOfType<UpperRightLegWearable>().gameObject;
        equipToPlayerParts[2] = FindObjectOfType<UpperLeftLegWearable>().gameObject;
        equipToPlayerParts[3] = FindObjectOfType<LowerRightLegWearable>().gameObject;
        equipToPlayerParts[4] = FindObjectOfType<LowerLeftLegWearable>().gameObject;
        base.Start();
    }
}
