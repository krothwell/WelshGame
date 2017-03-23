namespace GameUI {
    public class FeetEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquipped() {
            return transform.GetComponentInChildren<FeetItem>();
        }
    }
}