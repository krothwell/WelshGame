namespace GameUI {

    public class LeggingsEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquippedItem() {
            return transform.GetComponentInChildren<LeggingsItem>();
        }
    }
}