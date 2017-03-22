using System;
using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using GameUI.Utilities;

namespace GameUI {
    public class QuestTask : UIController, ISelectableUI {
        private string myID;
        public string MyID {
            get { return myID; }
            set { myID = value; }
        }
        private string myDescription;
        public string MyDescription {
            get { return myDescription; }
            set { myDescription = value; }
        }

        public GameObject partsListPrefab, partPrefab;
        private VerticalLayoutGroup partsList;
        Transform arrowTransform;

        void Awake() {
            arrowTransform = GetComponentInChildren<Image>().GetComponent<Transform>();
        }

        public void SetDescription(string descTxt) {
            myDescription = descTxt;
            GetComponentInChildren<Text>().text = descTxt;
            SetMyHeight();
        }

        public void SetMyHeight() {
            Canvas.ForceUpdateCanvases();
            float myTextHeight = GetComponentInChildren<Text>().preferredHeight;
            GetComponent<LayoutElement>().minHeight = myTextHeight;
            BoxCollider2D myBoxCollider = GetComponent<BoxCollider2D>();
            myBoxCollider.size = new Vector2(myBoxCollider.size.x, myTextHeight);
        }

        public void SetCompleted() {
            GetComponentInChildren<Text>().color = Colours.colorCompletedQuestTask;
        }

        void OnMouseUpAsButton() {
            SelectSelf();
        }

        public void SelectSelf() {
            if (partsList == null) {
                DisplayTaskParts();
            } else {
                HideTaskParts();
            }
        }

        public void DeselectSelf() {

        }

        public void DisplayTaskParts() {
            partsList = (Instantiate(partsListPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<VerticalLayoutGroup>();
            partsList.transform.SetParent(transform.parent, false);
            partsList.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            FillDisplayFromDb(DbQueries.GetEquipItemPartsRelatedToTask(myID), partsList.transform, BuildEquipItemPart, myID);
            arrowTransform.Rotate(0, 0, -90);
        }

        public void HideTaskParts() {
            Destroy(partsList.gameObject);
            partsList = null;
            arrowTransform.Rotate(0, 0, 90);
        }

        private Transform BuildEquipItemPart(string[] strArray) {
            string partID = strArray[0];
            string itemNameStr = strArray[1];
            GameObject taskPartEquipItem = (Instantiate(partPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject);
            taskPartEquipItem.GetComponent<Text>().text = "Equip " + itemNameStr;
            bool completed = Convert.ToBoolean(DbCommands.GetCountFromTable("CompletedQuestTaskParts", "PartIDs = " + partID + " AND SaveIDs = 0"));
            if (completed) {
                print("part complete!");
                taskPartEquipItem.GetComponent<Text>().color = Colours.colorCompletedQuestTaskPart;
            }
            return taskPartEquipItem.transform;
        }
    }
}