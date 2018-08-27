public class FeetItem : EquipableWorldItem {
    protected override void SetPlayerParts() {
        PlayerCharacter pc = FindObjectOfType<PlayerCharacter>();
        equipToPlayerParts[0] = pc.GetComponentInChildren<RightFootWearable>().gameObject;
        equipToPlayerParts[1] = pc.GetComponentInChildren<LeftFootWearable>().gameObject;
    }
}
