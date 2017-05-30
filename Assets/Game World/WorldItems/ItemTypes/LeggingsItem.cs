using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeggingsItem : EquipableWorldItemWithMultiSprites {
    new void Start() {
        PlayerCharacter pc = FindObjectOfType<PlayerCharacter>();
        equipToPlayerParts[0] = pc.GetComponentInChildren<LowerTorsoWearable>().gameObject;
        equipToPlayerParts[1] = pc.GetComponentInChildren<UpperRightLegWearable>().gameObject;
        equipToPlayerParts[2] = pc.GetComponentInChildren<UpperLeftLegWearable>().gameObject; 
        equipToPlayerParts[3] = pc.GetComponentInChildren<LowerRightLegWearable>().gameObject; 
        equipToPlayerParts[4] = pc.GetComponentInChildren<LowerLeftLegWearable>().gameObject; 
        base.Start();
    }
}
