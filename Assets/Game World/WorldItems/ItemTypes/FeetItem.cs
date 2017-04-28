public class FeetItem : EquipableWorldItemWithMultiSprites {
    new void Start() {
        equipToPlayerParts[0] = FindObjectOfType<RightFootWearable>().gameObject;
        equipToPlayerParts[1] = FindObjectOfType<LeftFootWearable>().gameObject;
        base.Start();
    }
}
