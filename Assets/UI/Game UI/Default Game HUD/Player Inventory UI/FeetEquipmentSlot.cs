namespace GameUI {
    public class FeetEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquippedItem() {
            return transform.GetComponentInChildren<FeetItem>();
        }
    }
}