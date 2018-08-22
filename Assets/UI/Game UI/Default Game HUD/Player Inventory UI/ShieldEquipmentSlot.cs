namespace GameUI {
    public class ShieldEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquippedItem() {
            return transform.GetComponentInChildren<ShieldItem>();
        }
    }
}