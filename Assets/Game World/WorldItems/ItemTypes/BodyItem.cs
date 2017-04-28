
public class BodyItem : EquipableWorldItemWithMultiSprites {
    new void Start() {
        
        equipToPlayerParts[0] = FindObjectOfType<UpperTorsoWearable>().gameObject;
        equipToPlayerParts[1] = FindObjectOfType<UpperRightArmWearable>().gameObject;
        equipToPlayerParts[2] = FindObjectOfType<UpperLeftArmWearable>().gameObject;
        equipToPlayerParts[3] = FindObjectOfType<LowerRightArmWearable>().gameObject;
        equipToPlayerParts[4] = FindObjectOfType<LowerLeftArmWearable>().gameObject;
        base.Start();
        //EquipToPlayerModel();

    }
}
