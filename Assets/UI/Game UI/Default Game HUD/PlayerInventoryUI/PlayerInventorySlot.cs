using UnityEngine;
using System.Collections;

namespace GameUI {
    public class PlayerInventorySlot : MonoBehaviour {
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