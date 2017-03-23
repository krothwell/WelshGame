namespace GameUI {
    public class BodyEquipmentSlot : PlayerEquipmentSlot {


        public override WorldItem GetEquipped() {
            return transform.GetComponentInChildren<BodyItem>();
        }
    }
}