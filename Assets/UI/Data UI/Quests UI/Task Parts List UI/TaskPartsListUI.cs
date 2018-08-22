using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;
using UnityEngine.UI;
using DataUI.ListItems;

namespace DataUI {
    public class TaskPartsListUI : UIController {
        //Task parts
        GameObject partsListUI, partsListUIPanel, partsList,
           addPart, addPartPanel, addPartPanelJoiner, activateAddPartBtn, partOptionSelectedList;

        public GameObject TaskPartEquipItemPrefab, TaskPartPrefabPrefab, TaskPartDefeatCharTagPrefab, TaskPartCompleteQuestPrefab, TaskPartActivateDialogueNodePrefab,
                EquipItemPartOptionBtnPrefab, PrefabPartOptionBtnPrefab, TagPartOptionBtnPrefab, CompleteQuestOptionBtnPrefab, ActivateDialogueNodeOptionBtnPrefab;

        QuestsUI questsUI;

        ListDisplayInfo activateDialogueNodeBtnListInfo;
        ListSearcher optionBtnListSearcher;

        public string selectedTaskPart;
        // Use this for initialization
        void Start() {
            questsUI = FindObjectOfType<QuestsUI>();
            //PART COMPONENTS
            partsListUI = FindObjectOfType<TaskPartsListUI>().gameObject;
            partsListUIPanel = partsListUI.transform.Find("Panel").gameObject;
            partsList = partsListUIPanel.transform.Find("PartsList").gameObject;
            //add
            addPart = partsListUIPanel.transform.Find("AddPart").gameObject;
            addPartPanel = addPart.transform.Find("AddPartPanel").gameObject;
            activateAddPartBtn = addPart.transform.Find("ActivateAddPartBtn").gameObject;
            partOptionSelectedList = addPartPanel.transform.Find("ScrollWindow").Find("OptionSelectedList").gameObject;
            optionBtnListSearcher = addPartPanel.GetComponentInChildren<ListSearcher>();
            selectedTaskPart = "selectedTaskPart";
            CreateSelectionToggleGroup(selectedTaskPart);

            HideNewPartPanel();

            activateDialogueNodeBtnListInfo = new ListDisplayInfo(
                DbQueries.GetTaskPartOptionsActivateDialogueNodeDisplayQry,
                BuildActivateDialogueNodeOptionBtn);
            print(activateDialogueNodeBtnListInfo);
        }

        public void HideNewPartPanel() {
            activateAddPartBtn.GetComponent<Button>().interactable = true;
            EmptyDisplay(partOptionSelectedList.transform);
            addPartPanel.SetActive(false);
        }

        public void DisplayNewPartPanel() {
            activateAddPartBtn.GetComponent<Button>().interactable = false;
            addPartPanel.SetActive(true);
        }

        public void DisplayPartsRelatedToTask(string taskID) {
            partsListUIPanel.SetActive(true);
            DisplayEquipItemPartsRelatedToTask(taskID);
            DisplayPrefabPartsRelatedToTask(taskID);
            DisplayDefeatCharTagPartsRelatedToTask(taskID);
            DisplayCompleteQuestPartsRelatedToTask(taskID);
            DisplayActivateDialoguePartsRelatedToTask(taskID);
        }

        public void DisplayEquipItemPartsRelatedToTask(string taskID) {
            FillDisplayFromDb(DbQueries.GetEquipItemPartsRelatedToTask(taskID), partsList.transform, BuildEquipItemPart, taskID);
        }

        public void DisplayPrefabPartsRelatedToTask(string taskID) {
            AppendDisplayFromDb(DbQueries.GetPrefabPartsRelatedToTask(taskID), partsList.transform, BuildPrefabPart, taskID);
        }

        public void DisplayCompleteQuestPartsRelatedToTask(string taskID) {
            AppendDisplayFromDb(DbQueries.GetCompleteQuestPartsRelatedToTaskQry(taskID), partsList.transform, BuildCompleteQuestTagPart, taskID);
        }

        public void DisplayDefeatCharTagPartsRelatedToTask(string taskID) {
            AppendDisplayFromDb(DbQueries.GetDefeatCharTagPartsRelatedToTaskQry(taskID), partsList.transform, BuildDefeatCharTagPart, taskID);
        }

        public void DisplayActivateDialoguePartsRelatedToTask(string taskID) {
            AppendDisplayFromDb(DbQueries.GetActivateDialoguePartsRelatedToTaskQry(taskID), partsList.transform, BuildActivateDialogueNodePart, taskID);
        }

        public void DisplayPartOptionsRelatedToEquipItem() {
            FillDisplayFromDb(DbQueries.GetTaskPartOptionsEquipItemDisplayQry(), partOptionSelectedList.transform, BuildEquipItemPartOptionBtn);
            optionBtnListSearcher.DeactivateSelf();
        }

        public void DisplayPartOptionsRelatedToCompleteQuest() {
            AppendDisplayFromDb(DbQueries.GetTaskPartOptionsCompleteQuestDisplayQry(), partOptionSelectedList.transform, BuildCompleteQuestOptionBtn);
            optionBtnListSearcher.DeactivateSelf();
        }

        public void DisplayPartOptionsRelatedToActivateDialogueNode() {
            print(activateDialogueNodeBtnListInfo);
            AppendDisplayFromDb(activateDialogueNodeBtnListInfo.GetMyDefaultQuery(), partOptionSelectedList.transform, activateDialogueNodeBtnListInfo.GetMyBuildMethod());
            optionBtnListSearcher.SetSearchInfo(activateDialogueNodeBtnListInfo);
        }

        public void DisplayPartOptionsRelatedToDefeatCharacterTag() {
            NPCs npcChars = FindObjectOfType<NPCs>();

            EmptyDisplay(partOptionSelectedList.transform);
            foreach (string charTag in npcChars.GetCharTagsList()) {
                string[] strArray = new string[1];
                if (charTag != "") {
                    strArray[0] = charTag;
                    Transform tagOption = BuildDefeatCharTagPartOptionBtn(strArray);
                    tagOption.SetParent(partOptionSelectedList.transform, false);
                }
            }
            optionBtnListSearcher.DeactivateSelf();
        }

        public void DisplayPrefabOptions() {
            EmptyDisplay(partOptionSelectedList.transform);
            GameObject[] questPrefabs = Resources.LoadAll<GameObject>("QuestParts");
            //GameObject[] questPrefabs = (GameObject[])Resources.LoadAll("QuestParts");
            print(questPrefabs.Length);
            foreach (GameObject questPrefab in questPrefabs) {
                string[] strArray = new string[2];
                strArray[0] = "QuestParts/" + questPrefab.name;
                strArray[1] = questPrefab.GetComponent<TaskPartGetSkillPoint>().GetMyLabel();
                Transform questOption = BuildPrefabPartOptionBtn(strArray);
                questOption.SetParent(partOptionSelectedList.transform, false);
            }
        }

        public void InsertPart(string partID) {
            string currentTaskID = (questsUI.GetSelectedItemFromGroup(questsUI.selectedTask) as Task).MyID;

            DbCommands.InsertTupleToTable("QuestTaskParts",
                                            partID,
                                            currentTaskID
                                         );
        }

        public void InsertPartEquipItem(string itemName) {
            string currentTaskID = (questsUI.GetSelectedItemFromGroup(questsUI.selectedTask) as Task).MyID;
            string partID = DbCommands.GenerateUniqueID("QuestTaskParts", "PartIDs", "PartID");
            InsertPart(partID);
            DbCommands.InsertTupleToTable("QuestTaskPartsEquipItem",
                                            itemName,
                                            partID
                                         );
            DisplayPartsRelatedToTask(currentTaskID);
        }

        private Transform BuildEquipItemPart(string[] strArray) {
            string itemNameStr = strArray[1];
            string partID = strArray[0];
            TaskPartEquipItem taskPartEquipItem = (
                Instantiate(TaskPartEquipItemPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<TaskPartEquipItem>();
            taskPartEquipItem.SetItemNameText(itemNameStr);
            taskPartEquipItem.MyItemName = itemNameStr;
            taskPartEquipItem.MyID = partID;
            return taskPartEquipItem.transform;

        }

        private Transform BuildPrefabPart(string[] strArray) {
            string partID = strArray[0];
            string prefabLbl = strArray[1];
            TaskPartPrefab taskPartPrefab = (
                Instantiate(TaskPartPrefabPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<TaskPartPrefab>();
            taskPartPrefab.InitialiseMe(prefabLbl, partID);
            return taskPartPrefab.transform;
        }

        private Transform BuildCompleteQuestTagPart(string[] strArray) {
            string partID = strArray[0];
            string questName = strArray[1];
            string description = strArray[2];
            TaskPartCompleteQuest taskPartCompleteQuest = (
                Instantiate(TaskPartCompleteQuestPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<TaskPartCompleteQuest>();
            taskPartCompleteQuest.InitialiseMe(questName, description, partID);
            return taskPartCompleteQuest.transform;
        }

        private Transform BuildDefeatCharTagPart(string[] strArray) {
            string partID = strArray[0];
            string charTag = strArray[1];
            TaskPartDefeatCharTag taskPartPrefab = (
                Instantiate(TaskPartDefeatCharTagPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<TaskPartDefeatCharTag>();
            taskPartPrefab.InitialiseMe(charTag, partID);
            return taskPartPrefab.transform;

        }

        private Transform BuildActivateDialogueNodePart(string[] strArray) {
            string partID = strArray[0];
            string dialogueDesc = strArray[1];
            string nodeDesc = strArray[2];
            string nodeID = strArray[3];
            TaskPartActivateDialogueNode taskPartPrefab = (
                Instantiate(TaskPartActivateDialogueNodePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<TaskPartActivateDialogueNode>();
            taskPartPrefab.InitialiseMe(dialogueDesc, nodeDesc, nodeID, partID);
            return taskPartPrefab.transform;
        }

        private Transform BuildEquipItemPartOptionBtn(string[] strArray) {
            string itemNameStr = strArray[0];
            EquipItemPartOptionBtn equipItemPartOptionBtn = (
                Instantiate(EquipItemPartOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<EquipItemPartOptionBtn>();
            equipItemPartOptionBtn.SetText(itemNameStr);
            equipItemPartOptionBtn.MyName = itemNameStr;
            return equipItemPartOptionBtn.transform;

        }

        private Transform BuildPrefabPartOptionBtn(string[] strArray) {
            string prefabPath = strArray[0];
            string label = strArray[1];
            PrefabPartOptionBtn prefabPartOptionBtn = (
                Instantiate(PrefabPartOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<PrefabPartOptionBtn>();
            prefabPartOptionBtn.InitialiseMe(prefabPath, label);
            return prefabPartOptionBtn.transform;
        }

        private Transform BuildDefeatCharTagPartOptionBtn(string[] strArray) {
            string tagname = strArray[0];
            DefeatCharTagOptionBtn tagPartOptionBtn = (
                Instantiate(TagPartOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<DefeatCharTagOptionBtn>();
            tagPartOptionBtn.InitialiseMe(tagname);
            return tagPartOptionBtn.transform;
        }

        private Transform BuildCompleteQuestOptionBtn(string[] strArray) {
            string questName = strArray[0];
            string description = strArray[1];
            CompleteQuestPartOptionBtn completeQuestOptionBtn = (
                Instantiate(CompleteQuestOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<CompleteQuestPartOptionBtn>();
            completeQuestOptionBtn.InitialiseMe(questName, description);
            return completeQuestOptionBtn.transform;
        }

        private Transform BuildActivateDialogueNodeOptionBtn(string[] strArray) {
            string dialogueDescription = strArray[0];
            string nodeDescription = strArray[1];
            string nodeID = strArray[2];
            ActivateDialogueNodePartOptionBtn activateDialogueNodeOption = (
                Instantiate(ActivateDialogueNodeOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<ActivateDialogueNodePartOptionBtn>();
            activateDialogueNodeOption.InitialiseMe(dialogueDescription, nodeDescription, nodeID);
            return activateDialogueNodeOption.transform;
        }
    }
}