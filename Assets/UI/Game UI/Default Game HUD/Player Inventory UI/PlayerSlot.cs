using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI {
    /// <summary>
    /// Can hold items belonging to the player as Game Objects in the
    /// hierarchy. When clicked, decides whether to try to pick up an item in
    /// the slot or put an item in the slot if an inventory item has already
    /// been selected.
    /// </summary>
    public abstract class PlayerSlot : MonoBehaviour {
        public WorldItems.WorldItemTypes ItemType;
        protected PlayerInventoryUI inventory;
        // Use this for initialization
        protected void Start() {
            inventory = FindObjectOfType<PlayerInventoryUI>();
            print(inventory);
        }
        private void OnMouseUpAsButton() {
            SelectSlot();
        }

        /// <summary>
        /// Decides whether to try to pick up an item already in the slot or put an item 
        /// in the slot if an inventory item has already been selected.
        /// </summary>
        private void SelectSlot() {
            print(inventory);
            if (inventory.IsInventoryItemSelected()) {
                if (transform.childCount <= 0) {
                    AttemptToPutItemInSlot();
                }
            }
            else {
                if (transform.childCount > 0) {
                    AttemptToPickUpItemInSlot();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void AttemptToPutItemInSlot() {
            inventory.SelectedItem.transform.SetParent(transform, false);
            inventory.SelectedItem.transform.localPosition = new Vector3(0f, 0f, 0f);
            inventory.SelectedItem = null;
        }

        /// <summary>
        /// If there is an item found in the slot, the item will be passed to the parent UI
        /// so that it can be put into other slots or used for another purpose.
        /// </summary>
        public void AttemptToPickUpItemInSlot() {
                inventory.SelectItem(gameObject);
        }

    }
}