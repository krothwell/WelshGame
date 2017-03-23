namespace GameUI {
    public class HeadEquipmentSlot : PlayerEquipmentSlot {

        public override WorldItem GetEquipped() {
            return transform.GetComponentInChildren<HeadItem>();
        }
    }
}