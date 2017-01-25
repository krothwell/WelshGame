using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    GameObject items;
    GameObject closeBtn;
    GameObject panel;
    GameObject selectedItem;
    public GameObject[] equippedItemRigSlots = new GameObject[15];
    Transform selectedItemParent;
    GameObject ui;
    bool followCursor = false;
    public GameObject inventoryItemPrefab;
	// Use this for initialization
	void Start () {
        ui = GameObject.Find("UI");
        panel = transform.FindChild("Panel").gameObject;
        items = panel.transform.FindChild("Items").gameObject;
    }

    void Update() {
        if (followCursor == true && selectedItem != null) {
            SelectedFollowCursor();
        }
    }

    public void OpenInventory() {
        panel.SetActive(true);
    }

    public void CloseInventory() {
        Invoke("CloseInventoryNow", 0.1f);//delayed so that character doesn't begin walking to button press
    }

    public void CloseInventoryNow() {
        panel.SetActive(false);

    }

    public void RecieveItem(GameObject worldItem) {
        GameObject newItem = CopyWorldItemAsInventoryItem(worldItem);
        foreach (Transform inventorySlot in items.transform) {
            if (inventorySlot.childCount <= 0) {
                newItem.transform.SetParent(inventorySlot,false);
                break;
            }
        }
    }

    public void ActionSlotItem(GameObject slot) {
        print(slot.transform.childCount);
        if (selectedItem != null) {
            if (slot.transform.childCount <= 0) {
                InsertSelectedItemToSlot(slot);
            }
        } else {
            if (slot.transform.childCount > 0) {
               SelectItem(slot);
            }
        }
    }

    private void SelectedFollowCursor() {
        Vector2 mousePos = new Vector2(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        selectedItem.GetComponent<RectTransform>().position = mousePos;
    }

    public void SelectItem(GameObject slot) {
        if (selectedItem == null) {
            selectedItem = slot.transform.GetChild(0).gameObject;
            selectedItemParent = slot.transform;
            if (IsEquipmentSlot(slot)) {
                WorldItems.itemTypes selectedSlotItemType = slot.GetComponent<EquippedSlot>().itemType;
                if (selectedSlotItemType == WorldItems.itemTypes.HeadGear) {
                    equippedItemRigSlots[0].GetComponent<SpriteRenderer>().sprite = null;
                }
            }
            followCursor = true;
        }
        selectedItem.transform.SetParent(ui.transform);
    }

    public void CancelSelectItem() {
        selectedItem.transform.SetParent(selectedItemParent);
        followCursor = true;
    }

    public void InsertSelectedItemToSlot(GameObject selectedSlot) {
        GameObject newItem = CopyInventoryItemAsInventoryItem(selectedItem);
        if (IsEquipmentSlot(selectedSlot)) {
            WorldItems.itemTypes selectedSlotItemType = selectedSlot.GetComponent<EquippedSlot>().itemType;
            if (selectedSlotItemType == WorldItems.itemTypes.HeadGear) {
                equippedItemRigSlots[0].GetComponent<SpriteRenderer>().sprite = selectedItem.GetComponent<Image>().sprite;
            } else {
                return;
            }
        }
        DestroySelectedItem();
        followCursor = false;
        newItem.transform.SetParent(selectedSlot.transform, false);

    }

    public void DestroySelectedItem () {
        Destroy(selectedItem.gameObject);
        selectedItem = null;
        selectedItemParent = null;
    }

    private GameObject CopyWorldItemAsInventoryItem(GameObject item) {
        GameObject copy;
        Sprite itemSprite = item.GetComponent<SpriteRenderer>().sprite;
        copy = GameObject.Instantiate(inventoryItemPrefab) as GameObject;
        copy.GetComponent<Image>().sprite = itemSprite;
        copy.GetComponent<InventoryItem>().itemType = item.GetComponent<WorldItem>().itemType;
        return copy;
    }

    private GameObject CopyInventoryItemAsInventoryItem(GameObject item) {
        GameObject copy;
        Sprite itemSprite = item.GetComponent<Image>().sprite;
        copy = GameObject.Instantiate(inventoryItemPrefab) as GameObject;
        copy.GetComponent<Image>().sprite = itemSprite;
        copy.GetComponent<InventoryItem>().itemType = item.GetComponent<InventoryItem>().itemType;
        return copy;
    }

    private bool IsEquipmentSlot(GameObject selectedSlot) {
        if (selectedSlot.GetComponent<EquippedSlot>() != null) {
            return true;
        } else { return false; }
    }


}

