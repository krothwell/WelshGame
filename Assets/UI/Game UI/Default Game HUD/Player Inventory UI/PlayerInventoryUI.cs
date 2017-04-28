using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityUtilities;
using System.Collections.Generic;


namespace GameUI {
    /// <summary>
    /// Responsible for displaying inventory items and how they should be moved
    /// to and from UI slots, and the game world. 
    /// </summary>
    public class PlayerInventoryUI : MonoBehaviour {
        GameObject items;
        GameObject panel;
        GameObject selectedItem;
        private PlayerEquipmentSlot[] equipmentSlots;
        private Dictionary<WorldItems.WorldItemTypes, WorldItem> equippedDict;
        GameObject ui;
        public GameObject inventoryItemPrefab;

        void Awake() {
            equippedDict = new Dictionary<WorldItems.WorldItemTypes, WorldItem>();
            panel = transform.FindChild("Panel").gameObject;
            items = panel.transform.FindChild("ItemSlots").gameObject;
        }
        void Start() {
            ui = GameObject.Find("UI");
            InitialiseEquippedItemsDict();
        }

        void Update() {
            if (selectedItem != null) {
                SetSelectedItemToCursor();
            }
        }

        public void OpenInventory() {
            panel.SetActive(true);
        }

        public void CloseInventory() {
            //TODO: Check if delay click select from MouseSelection.cs can be used to delay selecting world isntead.
            Invoke("CloseInventoryNow", 0.1f);//delayed so that character doesn't begin walking to button press
        }

        public void CloseInventoryNow() {
 
            panel.SetActive(false);

        }

        public void RecieveItem(WorldItem worldItem) {
            foreach (Transform inventorySlot in items.transform) {
                if (inventorySlot.childCount <= 0) {
                    worldItem.transform.SetParent(inventorySlot, false);
                    break;
                }
            }

        }

        public bool IsItemSelected() {
            return (selectedItem != null);
        }

        public void AttemptToPickUpItemInSlot(GameObject slot) {
            if (slot.transform.childCount > 0) {
                    SelectItem(slot);
            }
        }

        public void AttemptToPutItemInSlot(GameObject slot) {
            if (slot.transform.childCount <= 0) {
                if (slot.HasComponent<PlayerEquipmentSlot>()) {
                    if (slot.GetComponent<PlayerEquipmentSlot>().ItemType
                        == selectedItem.GetComponent<WorldItem>().itemType) {
                        InsertSelectedItemToSlot(slot, true);
                    }
                }
                else if (slot.HasComponent<PlayerInventorySlot>()) {
                    InsertSelectedItemToSlot(slot);
                }
            }
        }

        private void SetSelectedItemToCursor() {
            Vector2 mousePos = new Vector2(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            selectedItem.GetComponent<RectTransform>().position = mousePos;
        }

        public void SelectItem(GameObject slot) {
            if (selectedItem == null) {
                selectedItem = slot.transform.GetChild(0).gameObject;
                if (slot.HasComponent<PlayerEquipmentSlot>()) {
                    selectedItem.GetComponent<WorldItem>().UnequipFromPlayerModel();
                }
                selectedItem.GetComponent<BoxCollider2D>().enabled = false;
                selectedItem.transform.SetParent(ui.transform);
            }
        }

        public void InsertSelectedItemToSlot(GameObject selectedSlot, bool equip = false) {
            if (equip) {
                selectedItem.GetComponent<WorldItem>().EquipToPlayerModel();
                InsertToEquippedDict(selectedItem.GetComponent<WorldItem>());
            }
            selectedItem.transform.SetParent(selectedSlot.transform, false);
            selectedItem.transform.localPosition = new Vector3(0f, 0f, 0f);
            selectedItem = null;
        }

        public void InsertToEquippedDict(WorldItem worldItem) {
            if (equippedDict.ContainsKey(worldItem.GetMyItemType())) {
                equippedDict[worldItem.GetMyItemType()] = worldItem;
            }
            else {
                equippedDict.Add(worldItem.GetMyItemType(), worldItem);
            }
        }

        public WorldItem GetItemFromEquippedDict(WorldItems.WorldItemTypes itemType) {
            if (equippedDict.ContainsKey(itemType)) {
                return equippedDict[itemType];
            }
            else {
                return null;
            }
        }

        public void InitialiseEquippedItemsDict() {
            OpenInventory();
            equipmentSlots = FindObjectsOfType<PlayerEquipmentSlot>();
            foreach (PlayerEquipmentSlot equipmentSlot in equipmentSlots) {
                if (equipmentSlot.transform.childCount > 0) {
                    WorldItem item = equipmentSlot.transform.GetChild(0).GetComponent<WorldItem>();
                    if (item != null) {
                        InsertToEquippedDict(item);
                    }
                }
            }
            CloseInventory();
            //print("PRINTING DICTIONARY");
            //foreach(KeyValuePair<WorldItems.WorldItemTypes, WorldItem> pair in equippedDict) {
            //    print(pair.Key + ":" + pair.Value);
            //}
        }

    }
}
