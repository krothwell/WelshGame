using UnityEngine;
using System.Collections;

namespace GameUI {
    public abstract class PlayerEquipmentSlot : MonoBehaviour {
        public WorldItems.WorldItemTypes ItemType;
        PlayerInventoryUI inventory;
        // Use this for initialization
        void Start() {
            inventory = FindObjectOfType<PlayerInventoryUI>();
        }

        void OnMouseUpAsButton() {
            print(inventory);
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