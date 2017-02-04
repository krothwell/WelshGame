using UnityEngine;
using System.Collections;

namespace GameUI {
    public class PlayerEquipmentSlot : MonoBehaviour {
        public WorldItems.itemTypes itemType;
        PlayerInventoryUI inventory;
        // Use this for initialization
        void Start() {
            inventory = gameObject.transform.parent.parent.parent.GetComponent<PlayerInventoryUI>();
        }

        // Update is called once per frame
        void Update() {

        }
        void OnMouseUp() {
            inventory.ActionSlotItem(gameObject);
        }
    }
}