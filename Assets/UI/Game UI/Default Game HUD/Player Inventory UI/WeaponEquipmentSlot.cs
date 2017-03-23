namespace GameUI {
    public class WeaponEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquipped() {
            return transform.GetComponentInChildren<WeaponItem>();
        }
    }
}