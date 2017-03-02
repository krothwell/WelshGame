using UnityEngine;
using UnityEngine.UI;
using DataUI.ListItems;
using DbUtilities;
using System.Collections.Generic;
using UnityUtilities;


namespace DataUI {
    /// <summary>
    /// Responsible for menus to add new quests and related tasks
    /// to the database and displaying various task types in lists
    /// so that they can be edited or deleted. Quests created here will
    /// be activated by dialogues. Methods in various in game classes will
    /// query the database when approprate to update task statuses and 
    /// update progress. 
    /// </summary>
    public class QuestsUI : UIController {
        private bool editMode;
        public string selectedQuest, selectedTask, selectedTaskPart, selectedTaskResult;
        //Prefabs
        public GameObject 
            QuestPrefab,
            TaskPrefab, 
            TaskPartEquipItemPrefab, 
            EquipItemPartOptionBtnPrefab,
            startDialogueTaskResultOptionBtnPrefab,
            existingStartDialogueTaskResultPrefab;

        //Quests
        GameObject questsListUI, questsListUIPanel, questsList,
                   questDetails, questDetailsPanel, activateQuestDetailsBtn;
        private InputField inputQuestDetailsName, inputQuestDetailsDesc; //new quest components

        //Tasks
        GameObject tasksListUI, tasksListUIPanel, tasksList,
           addTask, addTaskPanel, activateAddTaskBtn;//new task components
        private InputField inputTaskDescription; 

        //Task parts
        GameObject partsListUI, partsListUIPanel, partsList,
           addPart, addPartPanel, addPartPanelJoiner, activateAddPartBtn, partOptionSelectedList;

        //Task results
        GameObject taskResultsListUI, taskResultsListUIPanel, taskResultsList,
            addTaskResult, addTaskResultPanel, activateAddTaskResultBtn, taskResultOptionSelectedList;


        void Start() {

            //QUEST COMPONENTS
            questsListUI = GetPanel().transform.FindChild("QuestsListUI").gameObject;
            questsListUIPanel = questsListUI.transform.FindChild("Panel").gameObject;
            questsList = questsListUIPanel.transform.FindChild("QuestsList").gameObject;
            //add
            questDetails = questsListUIPanel.transform.FindChild("QuestDetails").gameObject;
            questDetailsPanel = questDetails.transform.FindChild("QuestDetailsPanel").gameObject;
            activateQuestDetailsBtn = questDetails.transform.FindChild("ActivateQuestDetailsBtn").gameObject;
            inputQuestDetailsName = questDetailsPanel.transform.FindChild("InputNameText").GetComponent<InputField>();
            inputQuestDetailsDesc = questDetailsPanel.transform.FindChild("InputDescriptionText").GetComponent<InputField>();

            //TASK COMPONENTS
            tasksListUI = GetPanel().transform.FindChild("QuestTasksListUI").gameObject;
            tasksListUIPanel = tasksListUI.transform.FindChild("Panel").gameObject;
            tasksList = tasksListUIPanel.transform.FindChild("TasksList").gameObject;
            //add
            addTask = tasksListUIPanel.transform.FindChild("AddTask").gameObject;
            addTaskPanel = addTask.transform.FindChild("AddTaskPanel").gameObject;
            activateAddTaskBtn = addTask.transform.FindChild("ActivateAddTaskBtn").gameObject;
            inputTaskDescription = addTaskPanel.transform.FindChild("InputDescriptionText").GetComponent<InputField>();

            //PART COMPONENTS
            partsListUI = GetPanel().transform.FindChild("QuestTaskPartsListUI").gameObject;
            partsListUIPanel = partsListUI.transform.FindChild("Panel").gameObject;
            partsList = partsListUIPanel.transform.FindChild("PartsList").gameObject;
            //add
            addPart = partsListUIPanel.transform.FindChild("AddPart").gameObject;
            addPartPanel = addPart.transform.FindChild("AddPartPanel").gameObject;
            activateAddPartBtn = addPart.transform.FindChild("ActivateAddPartBtn").gameObject;
            partOptionSelectedList = addPartPanel.transform.FindChild("OptionSelectedList").gameObject;

            //TASK RESULTS COMPONENTS
            taskResultsListUI = GetPanel().transform.FindChild("QuestTaskCompleteResultsUI").gameObject;
            taskResultsListUIPanel = taskResultsListUI.transform.FindChild("Panel").gameObject;
            taskResultsList = taskResultsListUIPanel.transform.FindChild("ResultsList").gameObject;
            //add
            addTaskResult = taskResultsListUIPanel.transform.FindChild("AddTaskResult").gameObject;
            addTaskResultPanel = addTaskResult.transform.FindChild("AddResultPanel").gameObject;
            activateAddTaskResultBtn = addTaskResult.transform.FindChild("ActivateAddTaskResultBtn").gameObject;
            taskResultOptionSelectedList = addTaskResultPanel.transform.FindChild("OptionSelectedList").gameObject;


            FillDisplayFromDb(DbQueries.GetQuestsDisplayQry(), questsList.transform, BuildQuest);

            selectedQuest = "selectedQuest";
            selectedTask = "selectedTask";
            selectedTaskPart = "selectedTaskPart";
            selectedTaskResult = "selectedTaskResult";

            CreateSelectionToggleGroup(selectedQuest);
            CreateSelectionToggleGroup(selectedTask);
            CreateSelectionToggleGroup(selectedTaskPart);
            CreateSelectionToggleGroup(selectedTaskResult);

            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            string currentScene = sceneLoader.GetCurrentSceneName();
            InsertWorldItemsNotInDbFromScene(currentScene);

            HideAddTaskPanel();
            HideNewPartPanel();
            HidePartsListUI();
            HideTasksListUI();
            HideTaskResultsListUI();
        }

        public void EditSelectedQuest() {
            editMode = true;
            activateQuestDetailsBtn.GetComponent<Text>().text = "Edit quest";
            DisplayQuestDetails();
            activateQuestDetailsBtn.GetComponent<Button>().interactable = false;
            Quest quest = (GetSelectedItemFromGroup(selectedQuest) as Quest);
            string questName = quest.MyName;
            string[] questsData = DbCommands.GetTupleFromTable("Quests",
            "QuestNames = " + DbCommands.GetParameterNameFromValue(questName), "QuestNames", questName);
            inputQuestDetailsName.text = questsData[0];
            inputQuestDetailsDesc.text = questsData[1];
        }
        
        public void AddNewQuest() {
            DisplayQuestDetails();
            inputQuestDetailsName.text = "";
            inputQuestDetailsDesc.text = "";
        }

        public void AddNewTask() {
            DisplayNewTaskPanel();
            inputQuestDetailsDesc.text = "";
        }

        public void AddNewPart() {
            DisplayNewPartPanel();
        }

        public void AddNewTaskResult() {
            DisplayNewTaskResultPanel();
        }

        public void DisplayQuestDetails() {
            activateQuestDetailsBtn.GetComponent<Button>().interactable = false;
            questDetailsPanel.SetActive(true);
        }

        public void DisplayNewTaskPanel() {
            activateAddTaskBtn.GetComponent<Button>().interactable = false;
            addTaskPanel.SetActive(true);
        }

        public void DisplayNewPartPanel() {
            activateAddPartBtn.GetComponent<Button>().interactable = false;
            addPartPanel.SetActive(true);
        }

        public void DisplayTasksRelatedToDialogue(string questName) {
            tasksListUIPanel.SetActive(true);
            FillDisplayFromDb(DbQueries.GetTasksDisplayQry(questName), tasksList.transform, BuildTask, questName);
        }

        public void DisplayPartsRelatedToTask(string taskID) {
            partsListUIPanel.SetActive(true);
            DisplayEquipItemPartsRelatedToTask(taskID);
        }

        public void DisplayResultsRelatedToTaskCompletion(string taskID) {
            taskResultsListUIPanel.SetActive(true);
            DisplayStartDialogueResultRelatedToTask(taskID);
        }

        public void DisplayNewTaskResultPanel() {
            activateAddTaskResultBtn.GetComponent<Button>().interactable = false;
            addTaskResultPanel.SetActive(true);
        }


        public void DisplayEquipItemPartsRelatedToTask(string taskID) {
            FillDisplayFromDb(DbQueries.GetEquipItemPartsRelatedToTask(taskID), partsList.transform, BuildEquipItemPart, taskID);
        }

        public void DisplayStartDialogueResultRelatedToTask(string taskID) {
            FillDisplayFromDb(DbQueries.GetStartDialogueResultsRelatedToTaskQry(taskID), taskResultsList.transform, BuildCurrentStartDialogueTaskResult, taskID);
        }

        public void DisplayPartOptionsRelatedToEquipItem() {
            FillDisplayFromDb(DbQueries.GetTaskPartOptionsEquipItemDisplayQry(), partOptionSelectedList.transform, BuildEquipItemPartOptionBtn);
        }

        public void DisplayTaskResultOptionsRelatedToStartDialogue() {
            FillDisplayFromDb(DbQueries.GetTaskResultOptionsToStartDialogueQry(), taskResultOptionSelectedList.transform, BuildStartDialogueTaskResultOptionBtn);
        }

        public void HideTasksListUI() {
            tasksListUIPanel.SetActive(false);
        }

        public void HidePartsListUI() {
            partsListUIPanel.SetActive(false);
        }

        public void HideTaskResultsListUI() {
            taskResultsListUIPanel.SetActive(false);
        }

        public void HideQuestDetails() {
            activateQuestDetailsBtn.GetComponent<Button>().interactable = true;
            questDetailsPanel.SetActive(false);
            if (IsItemSelectedInGroup(selectedQuest)) {
                (GetSelectedItemFromGroup(selectedQuest) as Quest).StopEditing();
            }
            editMode = false;
            activateQuestDetailsBtn.GetComponent<Text>().text = "New quest";
        }

        public void HideAddTaskPanel() {
            activateAddTaskBtn.GetComponent<Button>().interactable = true;
            addTaskPanel.SetActive(false);
        }

        public void HideNewPartPanel() {
            activateAddPartBtn.GetComponent<Button>().interactable = true;
            EmptyDisplay(partOptionSelectedList.transform);
            addPartPanel.SetActive(false);
        }

        public void HideNewTaskResultPanel() {
            activateAddTaskResultBtn.GetComponent<Button>().interactable = true;
            EmptyDisplay(taskResultOptionSelectedList.transform);
            addTaskResultPanel.SetActive(false);
        }

        public void SaveQuestDetails() {
            Quest selectedQuestObj = (GetSelectedItemFromGroup(selectedQuest) as Quest);
            if (inputQuestDetailsName.text != null) {
                print("quest details name not null, checking selected object is not null");
                if (selectedQuestObj != null) {
                    print("selectedQuestObj not null, checking selected object is in edit mode");
                    if (selectedQuestObj.IsInEditMode()) {
                        print("selectedQuestObj is in edit mode, attempting to update quest in db with name " + selectedQuestObj.MyName + " and description " + inputQuestDetailsDesc.text);
                        UpdateQuestInDb (selectedQuestObj.MyName, inputQuestDetailsName.text, inputQuestDetailsDesc.text);
                        selectedQuestObj.UpdateSelf(inputQuestDetailsName.text, inputQuestDetailsDesc.text);
                    } else { InsertQuest(); }
                } else { InsertQuest(); };
            }
        }

        private void InsertQuest () {
            DbCommands.InsertTupleToTable("Quests",
                                            inputQuestDetailsName.text,
                                            inputQuestDetailsDesc.text);
            FillDisplayFromDb(DbQueries.GetQuestsDisplayQry(), questsList.transform, BuildQuest);
        }

        public void InsertTask() {
            string currentQuestName = (GetSelectedItemFromGroup(selectedQuest) as Quest).MyName;
            string taskID = DbCommands.GenerateUniqueID("QuestTasks", "TaskIDs", "TaskID");
            DbCommands.InsertTupleToTable("QuestTasks",
                                            taskID,
                                            inputTaskDescription.text,
                                            currentQuestName);
            DisplayTasksRelatedToDialogue(currentQuestName);
        }

        public void InsertPart(string partID) {
            string currentTaskID = (GetSelectedItemFromGroup(selectedTask) as Task).MyID;
            
            DbCommands.InsertTupleToTable("QuestTaskParts",
                                            partID,
                                            currentTaskID
                                         );
        }

        public void InsertPartEquipItem(string itemName) {
            string currentTaskID = (GetSelectedItemFromGroup(selectedTask) as Task).MyID;
            string partID = DbCommands.GenerateUniqueID("QuestTaskParts", "PartIDs", "PartID");
            InsertPart(partID);
            DbCommands.InsertTupleToTable("QuestTaskPartsEquipItem",
                                            itemName, 
                                            partID
                                         );
            DisplayPartsRelatedToTask(currentTaskID);
        }

        public void InsertTaskResult(string resultID) {
            string currentTaskID = (GetSelectedItemFromGroup(selectedTask) as Task).MyID;
            DbCommands.InsertTupleToTable("QuestTaskResults",
                                            resultID,
                                            currentTaskID
                                         );
        }

        public void InsertTaskResultStartDialogue(string dialogueID) {
            string resultID = DbCommands.GenerateUniqueID("QuestTaskResults", "ResultIDs", "ResultID");
            InsertTaskResult(resultID);
            string currentTaskID = (GetSelectedItemFromGroup(selectedTask) as Task).MyID;
            DbCommands.InsertTupleToTable("QuestTaskStartDialogueResults",
                                resultID,
                                currentTaskID,
                                dialogueID
                             );
            DisplayResultsRelatedToTaskCompletion(currentTaskID);
        }

        public void UpdateQuestInDb(string questName, string newName, string questDescription) {
            Task selectedTaskObj = (GetSelectedItemFromGroup(selectedQuest) as Task);
            string[,] fields = new string[,]{
                                                { "QuestNames", newName },
                                                { "QuestDescriptions", questDescription }
                                             };
            DbCommands.UpdateTableTuple("Quests",
                                     "QuestNames = " + DbCommands.GetParameterNameFromValue(questName),
                                     fields,
                                     questName);
        }

        public void UpdateTaskInDb (string taskID, string taskDescription) {
            Task selectedTaskObj = (GetSelectedItemFromGroup(selectedQuest) as Task);
            string[,] fields = new string[,]{
                                                { "TaskDescriptions", taskDescription }
                                             };
            DbCommands.UpdateTableTuple("QuestTasks",
                                     "TaskIDs = " + taskID,
                                     fields);
        }



        private Transform BuildQuest(string[] strArray) {
            string nameStr = strArray[0];
            string detailsStr = strArray[1];
            Quest quest = (Instantiate(QuestPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<Quest>();
            quest.SetInputText(nameStr);
            quest.MyName = nameStr;
            quest.MyDescription = detailsStr;
            return quest.transform;
        }

        private Transform BuildTask(string[] strArray) {
            string idStr = strArray[0];
            string descStr = strArray[1];
            Task task = (Instantiate(TaskPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<Task>();
            task.SetInputText(descStr);
            task.GetComponent<Task>().MyID = idStr;
            task.GetComponent<Task>().MyDescription = descStr;
            return task.transform;
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

        private Transform BuildEquipItemPartOptionBtn(string [] strArray) {
            string itemNameStr = strArray[0];
            EquipItemPartOptionBtn equipItemPartOptionBtn = (
                Instantiate(EquipItemPartOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<EquipItemPartOptionBtn>();
            equipItemPartOptionBtn.SetText(itemNameStr);
            equipItemPartOptionBtn.MyName = itemNameStr;
            return equipItemPartOptionBtn.transform;
            
        }

        private Transform BuildStartDialogueTaskResultOptionBtn (string[] strArray) {
            string dialogueID = strArray[0];
            string dialogueDesc = strArray[1];
            string speakerName = strArray[2];
            NewStartDialogueTaskResultOptionBtn startDialogueTaskResultOptionBtn = (
                Instantiate(startDialogueTaskResultOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewStartDialogueTaskResultOptionBtn>();
            startDialogueTaskResultOptionBtn.MyID = dialogueID;
            startDialogueTaskResultOptionBtn.SetMyText(dialogueID, dialogueDesc, speakerName);
            return startDialogueTaskResultOptionBtn.transform;
        }

        private Transform BuildCurrentStartDialogueTaskResult (string [] strArray) {
            string resultID = strArray[0];
            string dialogueID = strArray[1];
            string dialogueDesc = strArray[2];
            ExistingStartDialogueTaskResult existingStartDialogueTaskResult = (
                Instantiate(existingStartDialogueTaskResultPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<ExistingStartDialogueTaskResult>();
            existingStartDialogueTaskResult.ResultID = resultID;
            existingStartDialogueTaskResult.SetMyText(dialogueID, dialogueDesc);
            return existingStartDialogueTaskResult.transform;
        }

        public void DeleteTaskFromDb(string taskID) {
            string[,] fields = { { "TaskIDs", taskID } };
            DbCommands.DeleteTupleInTable("QuestTasks", fields);
        }

        public void DeleteTaskPartFromDb(string taskPartID) {
            string[,] fields = { { "PartIDs", taskPartID } };
            DbCommands.DeleteTupleInTable("QuestTaskParts", fields);
        }

        public void DeleteStartDialogueTaskResultFromDB(string resultID) {
            string[,] fields = { { "ResultIDs", resultID } };
            DbCommands.DeleteTupleInTable("QuestTaskResults", fields);
        }

        /// <summary>
        /// Used to check if a list item is being edited using a details menu
        /// and stop other list items being selected 
        /// </summary>
        public bool IsEditingDetails() {
            return editMode;
        }

        private void InsertWorldItemsNotInDbFromScene(string scene) {
            WorldItems worldItems = FindObjectOfType<WorldItems>();
            foreach (string[] worldItemDetailsArray in worldItems.GetWorldItemsList()) {
                DbCommands.InsertTupleToTable(  "WorldItems",
                                                worldItemDetailsArray[0], //x
                                                worldItemDetailsArray[1], //y
                                                worldItemDetailsArray[2], //z
                                                worldItemDetailsArray[3], //parent
                                                scene,
                                                worldItemDetailsArray[4]); //item name
            }
            //Debugging.PrintDbTable("WorldItems");
        }


    }
}