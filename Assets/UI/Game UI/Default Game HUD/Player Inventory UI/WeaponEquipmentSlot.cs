namespace GameUI {
    public class WeaponEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquippedItem() {
            return transform.GetComponentInChildren<WeaponItem>();
        }
    }
}