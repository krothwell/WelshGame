using UnityEngine;
using System.Collections;

namespace GameUI {
    public abstract class PlayerEquipmentSlot : PlayerSlot {

        protected override void AttemptToPutItemInSlot() {
            if (GetComponent<PlayerEquipmentSlot>().ItemType
                == inventory.SelectedItem.GetComponent<WorldItem>().itemType) {
                inventory.SelectedItem.GetComponent<WorldItem>().EquipToPlayerModel();
                inventory.InsertToEquippedDict(inventory.SelectedItem.GetComponent<WorldItem>());
                base.AttemptToPutItemInSlot();
            }
        }

        public abstract WorldItem GetEquippedItem();
    }

    
}