
using System;

public class BodyItem : EquipableWorldItemWithMultiSprites {


    protected override void SetPlayerParts() {
        PlayerCharacter pc = FindObjectOfType<PlayerCharacter>();
        equipToPlayerParts[0] = pc.GetComponentInChildren<UpperTorsoWearable>().gameObject;
        equipToPlayerParts[1] = pc.GetComponentInChildren<UpperRightArmWearable>().gameObject;
        equipToPlayerParts[2] = pc.GetComponentInChildren<UpperLeftArmWearable>().gameObject;
        equipToPlayerParts[3] = pc.GetComponentInChildren<LowerRightArmWearable>().gameObject;
        equipToPlayerParts[4] = pc.GetComponentInChildren<LowerLeftArmWearable>().gameObject;
        //EquipToPlayerModel();

    }
}
