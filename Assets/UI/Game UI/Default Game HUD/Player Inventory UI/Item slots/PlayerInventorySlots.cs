using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class makes up part of the code used to manage the Inventory UI. 
/// It is responsible for activities relating to inventory slots.
/// </summary>
public class PlayerInventorySlots : MonoBehaviour {
    /// <summary>
    /// Receives a world item and stores it in an inventory slot.
    /// </summary>
    /// <param name="worldItem"></param>
    public void ReceiveItem(WorldItem worldItem) {
        foreach (Transform inventorySlot in transform) {
            if (inventorySlot.childCount <= 0) {
                worldItem.transform.SetParent(inventorySlot, false);
                break;
            }
        }
    }
}
