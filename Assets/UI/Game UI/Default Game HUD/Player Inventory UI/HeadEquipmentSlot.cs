namespace GameUI {
    public class HeadEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquippedItem() {
            return transform.GetComponentInChildren<HeadItem>();
        }
    }
}