using UnityEngine;
using System.Collections;

namespace GameUI {
    public abstract class PlayerEquipmentSlot : MonoBehaviour {
        public WorldItems.WorldItemTypes itemType;
        PlayerInventoryUI inventory;
        // Use this for initialization
        void Start() {
            inventory = gameObject.transform.parent.parent.parent.GetComponent<PlayerInventoryUI>();
        }

        void OnMouseUp() {
            if (inventory.IsItemSelected()) {
                inventory.AttemptToPutItemInSlot(gameObject);
            }
            else {
                inventory.AttemptToPickUpItemInSlot(gameObject);
            }
        }

        public abstract WorldItem GetEquipped();
    }

    
}