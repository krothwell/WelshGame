using UnityEngine;
using System.Collections;

namespace GameUI {
    public abstract class PlayerEquipmentSlot : PlayerSlot {
        protected PlayerEquipmentSlots equipmentSlots;
        protected new void Start() {
            base.Start();
            equipmentSlots = FindObjectOfType<PlayerEquipmentSlots>();

        }
        protected override void AttemptToPutItemInSlot() {
            if (GetComponent<PlayerEquipmentSlot>().ItemType
                == inventory.SelectedItem.GetComponent<WorldItem>().itemType) {
                equipmentSlots.EquipToPlayerModel(inventory.SelectedItem.GetComponent<EquipableWorldItem>());
                equipmentSlots.InsertToEquippedDict(inventory.SelectedItem.GetComponent<EquipableWorldItem>());
                base.AttemptToPutItemInSlot();
            }
        }

        public abstract WorldItem GetEquippedItem();
    }

    
}