﻿using System;
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
            AppendDisplayFromDb(DbQueries.GetPrefabPartsRelatedToTask(myID), partsList.transform, BuildPrefabPart, myID);
            AppendDisplayFromDb(DbQueries.GetActivateDialoguePartsRelatedToGameTaskQry(myID), partsList.transform, BuildActivateDialogueNodePart, myID);
            AppendDisplayFromDb(DbQueries.GetCompleteQuestPartsRelatedToTaskQry(myID), partsList.transform, BuildCompleteQuest, myID);
            AppendDisplayFromDb(DbQueries.GetDefeatCharTagPartsRelatedToTaskQry(myID), partsList.transform, BuildDefeatCharTagPart, myID);
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
            SetPartCompleted(partID, taskPartEquipItem);
            return taskPartEquipItem.transform;
        }

        private Transform BuildPrefabPart(string[] strArray) {
            string partID = strArray[0];
            string prefabLbl = strArray[1];
            GameObject taskPartPrefab = (Instantiate(partPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject);
            taskPartPrefab.GetComponent<Text>().text = prefabLbl;
            SetPartCompleted(partID, taskPartPrefab);
            return taskPartPrefab.transform;
        }

        private Transform BuildActivateDialogueNodePart(string[] strArray) {
            string partID = strArray[0];
            string charName = strArray[1];
            GameObject taskPartPrefab = (Instantiate(partPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject);
            taskPartPrefab.GetComponent<Text>().text = "Speak to " + charName;
            SetPartCompleted(partID, taskPartPrefab);
            return taskPartPrefab.transform;
        }

        private Transform BuildDefeatCharTagPart(string[] strArray) {
            string partID = strArray[0];
            string tag = strArray[1];
            GameObject taskPartPrefab = (Instantiate(partPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject);
            taskPartPrefab.GetComponent<Text>().text = "Defeat " + tag;
            SetPartCompleted(partID, taskPartPrefab);
            return taskPartPrefab.transform;
        }

        private Transform BuildCompleteQuest(string[] strArray) {
            string partID = strArray[0];
            string questName = strArray[1];
            GameObject taskPartPrefab = (Instantiate(partPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject);
            taskPartPrefab.GetComponent<Text>().text = "Complete quest: " + questName;
            SetPartCompleted(partID, taskPartPrefab);
            return taskPartPrefab.transform;
        }

        private void SetPartCompleted(string partID, GameObject partTransform) {
            bool completed = Convert.ToBoolean(DbCommands.GetCountFromTable("CompletedQuestTaskParts", "PartIDs = " + partID + " AND SaveIDs = 0"));
            if (completed) {
                print("part complete!");
                partTransform.GetComponent<Text>().color = Colours.colorCompletedQuestTaskPart;
            }
        }
    }
}