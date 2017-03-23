namespace GameUI {

    public class LeggingsEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquipped() {
            return transform.GetComponentInChildren<LeggingsItem>();
        }
    }
}