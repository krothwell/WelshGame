using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameUI {
    public class PlayerEquipmentSlots : MonoBehaviour {
        private PlayerEquipmentSlot[] equipmentSlots;
        private Dictionary<WorldItems.WorldItemTypes, EquipableWorldItem> equippedDict;
        private QuestsUI questsUI;
        private void Awake() {
            equippedDict = new Dictionary<WorldItems.WorldItemTypes, EquipableWorldItem>();
            questsUI = FindObjectOfType<QuestsUI>();
        }

        private void Start() {
            Invoke("InitialiseEquippedItemsDict", 0.1f);
            Invoke("EquipAll", 0.1f);
        }

        /// <summary>
        /// Iterates the array of sprites related to the player model and sets them to the
        /// counterparts attached to the world item's children.
        /// </summary>
        /// <param name="item"></param>
        public void EquipToPlayerModel(EquipableWorldItem item) {
            item.SetWorldSpritesActive(true);
            for (int i = 0; i < item.transform.childCount; i++) {
                if (item.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite != null) {
                    item.equipToPlayerParts[i].GetComponent<SpriteRenderer>().sprite =
                        item.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite;
                    item.equipToPlayerParts[i].GetComponent<SpriteRenderer>().material =
                        item.transform.GetChild(i).GetComponent<SpriteRenderer>().material;
                }
                else {
                    item.equipToPlayerParts[i].GetComponent<SpriteRenderer>().sprite = null;
                }
            }
            questsUI.CompleteEquipItemTaskPart(item.gameObject.name);
        }

        public void UnequipFromPlayerModel(EquipableWorldItem item) {
            for (int i = 0; i < item.equipToPlayerParts.Length; i++) {
                item.equipToPlayerParts[i].GetComponent<SpriteRenderer>().sprite = null;
            }
        }

        public void EquipAll() {
            Debug.Log(equippedDict.Count);
            foreach (KeyValuePair<WorldItems.WorldItemTypes, EquipableWorldItem> item in equippedDict) {
                Debug.Log("rubbish" + item.Value.name);
                EquipToPlayerModel(item.Value);
            }
        }

        public void InsertToEquippedDict(EquipableWorldItem worldItem) {
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
                    EquipableWorldItem item = equipmentSlot.transform.GetChild(0).GetComponent<EquipableWorldItem>();
                    Debug.Log(item.name);
                    if (item != null) {
                        InsertToEquippedDict(item);
                    }
                }
            }
        }
    }
}
