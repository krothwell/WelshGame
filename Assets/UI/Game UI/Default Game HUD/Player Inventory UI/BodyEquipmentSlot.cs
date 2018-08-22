namespace GameUI {
    public class BodyEquipmentSlot : PlayerEquipmentSlot {


        public override WorldItem GetEquippedItem() {
            return transform.GetComponentInChildren<BodyItem>();
        }
    }
}