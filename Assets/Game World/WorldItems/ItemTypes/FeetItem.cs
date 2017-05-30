public class FeetItem : EquipableWorldItemWithMultiSprites {
    new void Start() {
        PlayerCharacter pc = FindObjectOfType<PlayerCharacter>();
        equipToPlayerParts[0] = pc.GetComponentInChildren<RightFootWearable>().gameObject;
        equipToPlayerParts[1] = pc.GetComponentInChildren<LeftFootWearable>().gameObject; 
        base.Start();
    }
}
