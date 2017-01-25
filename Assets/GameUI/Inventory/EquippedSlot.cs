using UnityEngine;
using System.Collections;

public class EquippedSlot : MonoBehaviour {
    public WorldItems.itemTypes itemType;
    Inventory inventory;
    // Use this for initialization
    void Start() {
        inventory = gameObject.transform.parent.parent.parent.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update() {

    }
    void OnMouseUp() {
        inventory.ActionSlotItem(gameObject);
    }
}
