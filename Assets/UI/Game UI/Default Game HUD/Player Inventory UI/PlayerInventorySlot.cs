using UnityEngine;
using System.Collections;

namespace GameUI {
    /// <summary>
    /// Can hold inventory items belonging to the player as Game Objects in the
    /// hierarchy. When clicked, decides whether to try to pick up an item in
    /// the slot or put an item in the slot (if an inventory item has already
    /// been selected).
    /// </summary>
    public class PlayerInventorySlot : MonoBehaviour {
        PlayerInventoryUI inventory;
        // Use this for initialization
        void Start() {
            inventory = FindObjectOfType<PlayerInventoryUI>();
        }

        // Update is called once per frame

        void OnMouseUp() {
            if (inventory.IsItemSelected()) {
                inventory.AttemptToPutItemInSlot(gameObject);
            } else {
                inventory.AttemptToPickUpItemInSlot(gameObject);
            }
            
        }
    }
}