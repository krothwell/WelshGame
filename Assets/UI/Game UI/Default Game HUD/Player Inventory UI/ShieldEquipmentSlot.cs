namespace GameUI {
    public class ShieldEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquipped() {
            return transform.GetComponentInChildren<ShieldItem>();
        }
    }
}