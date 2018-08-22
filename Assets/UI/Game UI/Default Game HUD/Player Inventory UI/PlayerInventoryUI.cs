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
    public class PlayerInventoryUI : UIController {
        private GameObject selectedItem;
        public GameObject SelectedItem {
            get { return selectedItem; }
            set { selectedItem = value; }
        }
        private PlayerEquipmentSlot[] equipmentSlots;
        private Dictionary<WorldItems.WorldItemTypes, WorldItem> equippedDict;
        GameObject ui;
        public GameObject inventoryItemPrefab;

        void Awake() {
            equippedDict = new Dictionary<WorldItems.WorldItemTypes, WorldItem>();
        }
        void Start() {
            ui = GameObject.Find("UI");
        }

        void Update() {
            if (selectedItem != null) {
                SetSelectedItemToCursor();
            }
        }

        public void OpenInventory() {
            GetPanel().SetActive(true);
        }

        public void CloseInventory() {
            HideComponents();
        }

        public void EquipAll() {
            foreach(KeyValuePair<WorldItems.WorldItemTypes, WorldItem> item in equippedDict) {
                item.Value.EquipToPlayerModel();
            }
        }



        /// <summary>
        /// Detect if player has picked up item in inventory
        /// </summary>
        public bool IsInventoryItemSelected() {
            return (selectedItem != null);
        }

        private void SetSelectedItemToCursor() {
            Vector2 mousePos = new Vector2(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            selectedItem.GetComponent<RectTransform>().position = mousePos;
        }

        /// <summary>
        /// assigns the game object found in an inventory slot to the slectedItem parameter
        /// disables box collider while selected so that it won't interfere with moving to
        /// another slot or other actions.
        /// </summary>
        /// <param name="slot"></param>
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
            //OpenInventory();
            equipmentSlots = FindObjectsOfType<PlayerEquipmentSlot>();
            foreach (PlayerEquipmentSlot equipmentSlot in equipmentSlots) {
                if (equipmentSlot.transform.childCount > 0) {
                    WorldItem item = equipmentSlot.transform.GetChild(0).GetComponent<WorldItem>();
                    if (item != null) {
                        InsertToEquippedDict(item);
                    }
                }
            }
            //print("PRINTING DICTIONARY");
            //foreach(KeyValuePair<WorldItems.WorldItemTypes, WorldItem> pair in equippedDict) {
            //    print(pair.Key + ":" + pair.Value);
            //}
        }

    }
}
