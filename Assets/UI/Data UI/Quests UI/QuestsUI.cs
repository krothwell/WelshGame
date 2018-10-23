using UnityEngine;
using UnityEngine.UI;
using DataUI.ListItems;
using DbUtilities;
using System;
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
        TaskPartsListUI taskPartsListUI;
        private bool editMode;
        public string selectedQuest, selectedTask, selectedTaskResult;
        //Prefabs
        public GameObject
            QuestPrefab,
            TaskPrefab,
            StartDialogueTaskResultOptionBtnPrefab, ActivateDialogueTaskResultOptionBtnPrefab, EndCombatWithCharTaskResultOptionBtnPrefab,
            ExistingStartDialogueTaskResultPrefab, ExistingEndCombatWithCharTaskResultPrefab, ExistingActivateDialogueTaskResultPrefab;

        //Quests
        GameObject questsListUI, questsListUIPanel, questsList,
                   questDetails, questDetailsPanel, activateQuestDetailsBtn;
        private InputField inputQuestDetailsName, inputQuestDetailsDesc; //new quest components

        //Tasks
        GameObject tasksListUI, tasksListUIPanel, tasksList,
           addTask, addTaskPanel, activateAddTaskBtn;//new task components
        private InputField inputTaskDescription;
        private Toggle taskActiveAtStart;

        //Task results
        GameObject taskResultsListUI, taskResultsListUIPanel, taskResultsList,
            addTaskResult, addTaskResultPanel, activateAddTaskResultBtn, taskResultOptionSelectedList;

        ListDisplayInfo newStartDialogueOptionListInfo, newActivateDialogueOptionListInfo, newEndCombatOptionListInfo;
        ListSearcher listSearcher;

        void Start() {
            taskPartsListUI = GetPanel().transform.Find("TaskPartsListUI").GetComponent<TaskPartsListUI>();
            //QUEST COMPONENTS
            questsListUI = GetPanel().transform.Find("QuestsListUI").gameObject;
            questsListUIPanel = questsListUI.transform.Find("Panel").gameObject;
            questsList = questsListUIPanel.transform.Find("QuestsList").gameObject;
            //add
            questDetails = questsListUIPanel.transform.Find("QuestDetails").gameObject;
            questDetailsPanel = questDetails.transform.Find("QuestDetailsPanel").gameObject;
            activateQuestDetailsBtn = questDetails.transform.Find("ActivateQuestDetailsBtn").gameObject;
            inputQuestDetailsName = questDetailsPanel.transform.Find("InputNameText").GetComponent<InputField>();
            inputQuestDetailsDesc = questDetailsPanel.transform.Find("InputDescriptionText").GetComponent<InputField>();

            //TASK COMPONENTS
            tasksListUI = GetPanel().transform.Find("QuestTasksListUI").gameObject;
            tasksListUIPanel = tasksListUI.transform.Find("Panel").gameObject;
            tasksList = tasksListUIPanel.transform.Find("TasksList").gameObject;
            //add
            addTask = tasksListUIPanel.transform.Find("AddTask").gameObject;
            addTaskPanel = addTask.transform.Find("AddTaskPanel").gameObject;
            activateAddTaskBtn = addTask.transform.Find("ActivateAddTaskBtn").gameObject;
            inputTaskDescription = addTaskPanel.transform.Find("InputDescriptionText").GetComponent<InputField>();
            taskActiveAtStart = addTaskPanel.GetComponentInChildren<Toggle>();

            //TASK RESULTS COMPONENTS
            taskResultsListUI = GetPanel().transform.Find("QuestTaskCompleteResultsUI").gameObject;
            taskResultsListUIPanel = taskResultsListUI.transform.Find("Panel").gameObject;
            taskResultsList = taskResultsListUIPanel.transform.Find("ResultsList").gameObject;
            //add
            addTaskResult = taskResultsListUIPanel.transform.Find("AddTaskResult").gameObject;
            addTaskResultPanel = addTaskResult.transform.Find("AddResultPanel").gameObject;
            activateAddTaskResultBtn = addTaskResult.transform.Find("ActivateAddTaskResultBtn").gameObject;
            taskResultOptionSelectedList = addTaskResultPanel.transform.Find("ScrollWindow").Find("OptionSelectedList").gameObject;
            listSearcher = addTaskResultPanel.transform.GetComponentInChildren<ListSearcher>();


            FillDisplayFromDb(DbQueries.GetQuestsDisplayQry(), questsList.transform, BuildQuest);

            selectedQuest = "selectedQuest";
            selectedTask = "selectedTask";
            selectedTaskResult = "selectedTaskResult";

            CreateSelectionToggleGroup(selectedQuest);
            CreateSelectionToggleGroup(selectedTask);
            CreateSelectionToggleGroup(selectedTaskResult);

            newStartDialogueOptionListInfo = new ListDisplayInfo(
                DbQueries.GetTaskResultOptionsToStartDialogueQry,
                BuildStartDialogueTaskResultOptionBtn
            );

            newActivateDialogueOptionListInfo = new ListDisplayInfo(
                DbQueries.GetTaskResultOptionsToStartDialogueQry,
                BuildActivateDialogueTaskResultOptionBtn
            );

            newEndCombatOptionListInfo = new ListDisplayInfo(
                DbQueries.GetTaskResultOptionsToEndCombatWithCharQry,
                BuildEndCombatWithCharTaskResultOptionBtn
            );

            SceneLoader sceneLoader = new SceneLoader();
            string currentScene = sceneLoader.GetCurrentSceneName();
            InsertWorldItemsNotInDbFromScene(currentScene);

            HideAddTaskPanel();
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

        public void DisplayTasksRelatedToDialogue(string questName) {
            tasksListUIPanel.SetActive(true);
            FillDisplayFromDb(DbQueries.GetTasksDisplayQry(questName), tasksList.transform, BuildTask, questName);
        }

        public void DisplayResultsRelatedToTaskCompletion(string taskID) {
            taskResultsListUIPanel.SetActive(true);
            DisplayStartDialogueResultRelatedToTask(taskID);
            DisplayActivateDialogueResultRelatedToTask(taskID);
            DisplayEndCombatWithCharResultRelatedToTask(taskID);
        }

        public void DisplayStartDialogueResultRelatedToTask(string taskID) {
            print("THIS IS GETTING CALLED");
            Debugging.PrintDbQryResults(DbQueries.GetStartDialogueResultsRelatedToTaskQry(taskID), taskID);
            FillDisplayFromDb(DbQueries.GetStartDialogueResultsRelatedToTaskQry(taskID), taskResultsList.transform, BuildCurrentStartDialogueTaskResult, taskID);
        }

        public void DisplayActivateDialogueResultRelatedToTask(string taskID) {
            AppendDisplayFromDb(DbQueries.GetActivateDialogueResultsRelatedToTaskQry(taskID), taskResultsList.transform, BuildCurrentActivateDialogueTaskResult, taskID);
        }

        public void DisplayEndCombatWithCharResultRelatedToTask(string taskID) {
            AppendDisplayFromDb(DbQueries.GetEndCombatWithCharResultsRelatedToTaskQry(taskID), taskResultsList.transform, BuildCurrentEndCombatWithCharTaskResult, taskID);
        }

        public void DisplayNewTaskResultPanel() {
            activateAddTaskResultBtn.GetComponent<Button>().interactable = false;
            addTaskResultPanel.SetActive(true);
        }

        public void DisplayTaskResultOptionsRelatedToStartDialogue() {
            FillDisplayFromDb(newStartDialogueOptionListInfo.GetMyDefaultQuery(), taskResultOptionSelectedList.transform, newStartDialogueOptionListInfo.GetMyBuildMethod());
            listSearcher.SetSearchInfo(newStartDialogueOptionListInfo);
        }

        public void DisplayTaskResultOptionsRelatedToActivateDialogue() {
            FillDisplayFromDb(newActivateDialogueOptionListInfo.GetMyDefaultQuery(), taskResultOptionSelectedList.transform, newActivateDialogueOptionListInfo.GetMyBuildMethod());
            listSearcher.SetSearchInfo(newActivateDialogueOptionListInfo);
        }

        public void DisplayTaskResultOptionsRelatedToEndCombatWithChar() {
            FillDisplayFromDb(newEndCombatOptionListInfo.GetMyDefaultQuery(), taskResultOptionSelectedList.transform, newEndCombatOptionListInfo.GetMyBuildMethod());
            listSearcher.SetSearchInfo(newEndCombatOptionListInfo);
        }

        public void HideTasksListUI() {
            tasksListUIPanel.SetActive(false);
        }

        public void HidePartsListUI() {
            taskPartsListUI.HideComponents();
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
                        UpdateQuestInDb(selectedQuestObj.MyName, inputQuestDetailsName.text, inputQuestDetailsDesc.text);
                        selectedQuestObj.UpdateSelf(inputQuestDetailsName.text, inputQuestDetailsDesc.text);
                    }
                    else { InsertQuest(); }
                }
                else { InsertQuest(); };
            }
        }

        private void InsertQuest() {
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
            if (taskActiveAtStart.isOn) {
                DbCommands.InsertTupleToTable("QuestTasksActivated", taskID, "-1", "0");
            }
            DisplayTasksRelatedToDialogue(currentQuestName);
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
            HideNewTaskResultPanel();
        }

        public void InsertTaskResultActivateDialogue(string dialogueID) {
            string resultID = DbCommands.GenerateUniqueID("QuestTaskResults", "ResultIDs", "ResultID");
            InsertTaskResult(resultID);
            string currentTaskID = (GetSelectedItemFromGroup(selectedTask) as Task).MyID;
            DbCommands.InsertTupleToTable("QuestTaskActivateDialogueResults",
                                resultID,
                                currentTaskID,
                                dialogueID
                             );
            DisplayResultsRelatedToTaskCompletion(currentTaskID);
            HideNewTaskResultPanel();
        }

        public void UpdateQuestInDb(string questName, string newName, string questDescription) {
            string[,] fields = new string[,]{
                                                { "QuestNames", newName },
                                                { "QuestDescriptions", questDescription }
                                             };
            DbCommands.UpdateTableTuple("Quests",
                                     "QuestNames = " + DbCommands.GetParameterNameFromValue(questName),
                                     fields,
                                     questName);
        }

        public void UpdateTaskInDb(string taskID, string taskDescription, bool activeAtStart) {
            string[,] fields = new string[,]{
                                            { "TaskDescriptions", taskDescription }
                                            };
            DbCommands.UpdateTableTuple("QuestTasks",
                                     "TaskIDs = " + taskID,
                                     fields);
            if (activeAtStart) {
                DbCommands.InsertTupleToTable("QuestTasksActivated", taskID, "-1", "0");
            }
            else {
                string[,] activeTaskfields = { { "TaskIDs", taskID }, { "SaveIDs", "-1" } };
                DbCommands.DeleteTupleInTable("QuestTasksActivated", activeTaskfields); //Removes the task in activated tasks if it is marked as inactive.
            }
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
            bool taskActiveAtStart = Convert.ToBoolean(DbCommands.GetCountFromTable("QuestTasksActivated", "TaskIDs = " + idStr + " AND SaveIDs = -1"));
            Task task = (Instantiate(TaskPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<Task>();
            task.SetInputText(descStr);
            task.SetActiveAtStartToggle(taskActiveAtStart);
            task.MyID = idStr;
            task.MyDescription = descStr;

            return task.transform;
        }

        private Transform BuildStartDialogueTaskResultOptionBtn(string[] strArray) {
            string dialogueID = strArray[0];
            string dialogueDesc = strArray[1];
            string speakerName = strArray[2];
            NewStartDialogueTaskResultOptionBtn startDialogueTaskResultOptionBtn = (
                Instantiate(StartDialogueTaskResultOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewStartDialogueTaskResultOptionBtn>();
            startDialogueTaskResultOptionBtn.MyID = dialogueID;
            startDialogueTaskResultOptionBtn.SetMyText(dialogueID, dialogueDesc, speakerName);
            return startDialogueTaskResultOptionBtn.transform;
        }

        private Transform BuildActivateDialogueTaskResultOptionBtn(string[] strArray) {
            string dialogueID = strArray[0];
            string dialogueDesc = strArray[1];
            string speakerName = strArray[2];
            NewTaskResultActivateDialogue ActivateDialogueTaskResultOptionBtn = (
                Instantiate(ActivateDialogueTaskResultOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewTaskResultActivateDialogue>();
            ActivateDialogueTaskResultOptionBtn.MyID = dialogueID;
            ActivateDialogueTaskResultOptionBtn.InitialiseMe(dialogueID, dialogueDesc, speakerName);
            return ActivateDialogueTaskResultOptionBtn.transform;
        }

        private Transform BuildEndCombatWithCharTaskResultOptionBtn(string[] strArray) {
            string charName = strArray[0];
            string sceneName = strArray[1];
            NewEndCombatWithCharTaskResultOptionBtn endCombatWithCharTaskResultOptionBtn = (
                Instantiate(EndCombatWithCharTaskResultOptionBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewEndCombatWithCharTaskResultOptionBtn>();
            endCombatWithCharTaskResultOptionBtn.InitialiseMe(charName, sceneName);
            return endCombatWithCharTaskResultOptionBtn.transform;
        }

        private Transform BuildCurrentStartDialogueTaskResult(string[] strArray) {
            string resultID = strArray[0];
            string dialogueID = strArray[1];
            string dialogueDesc = strArray[2];
            ExistingStartDialogueTaskResult existingStartDialogueTaskResult = (
                Instantiate(ExistingStartDialogueTaskResultPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<ExistingStartDialogueTaskResult>();
            existingStartDialogueTaskResult.InitialiseMe(resultID, dialogueID, dialogueDesc);
            return existingStartDialogueTaskResult.transform;
        }

        private Transform BuildCurrentActivateDialogueTaskResult(string[] strArray) {
            string resultID = strArray[0];
            string dialogueID = strArray[1];
            string dialogueDesc = strArray[2];
            ExistingActivateDialogueTaskResult existingActivateDialogueTaskResult = (
                Instantiate(ExistingActivateDialogueTaskResultPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<ExistingActivateDialogueTaskResult>();
            existingActivateDialogueTaskResult.InitialiseMe(resultID, dialogueID, dialogueDesc);
            return existingActivateDialogueTaskResult.transform;
        }

        private Transform BuildCurrentEndCombatWithCharTaskResult(string[] strArray) {
            string resultID = strArray[0];
            string charName = strArray[1];
            string sceneName = strArray[2];
            ExistingEndCombatWithCharTaskResult existingEndCombatWithCharTaskResult = (
                Instantiate(ExistingEndCombatWithCharTaskResultPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<ExistingEndCombatWithCharTaskResult>();
            print(existingEndCombatWithCharTaskResult);
            existingEndCombatWithCharTaskResult.InitialiseMe(resultID, charName, sceneName);
            return existingEndCombatWithCharTaskResult.transform;
        }

        public void DeleteTaskFromDb(string taskID) {
            string[,] fields = { { "TaskIDs", taskID } };
            DbCommands.DeleteTupleInTable("QuestTasks", fields);
        }

        public void DeleteTaskPartFromDb(string taskPartID) {
            string[,] fields = { { "PartIDs", taskPartID } };
            DbCommands.DeleteTupleInTable("QuestTaskParts", fields);
        }

        public void DeleteTaskResultFromDB(string resultID) {
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
            foreach (string[] worldItemDetailsArray in worldItems.WorldItemList) {
                bool itemExists = DbCommands.IsRecordInTable("PremadeWorldItems",
                    "StartingLocationX = " + DbCommands.GetParameterNameFromValue(worldItemDetailsArray[0]) + " AND " +
                    "StartingLocationY = " + DbCommands.GetParameterNameFromValue(worldItemDetailsArray[1]) + " AND " +
                    "StartingLocationZ = " + DbCommands.GetParameterNameFromValue(worldItemDetailsArray[2]) + " AND " +
                    "StartingParentPath = " + DbCommands.GetParameterNameFromValue(worldItemDetailsArray[3]) + " AND " +
                    "StartingSceneNames = " + DbCommands.GetParameterNameFromValue(scene),
                    worldItemDetailsArray[0],
                    worldItemDetailsArray[1],
                    worldItemDetailsArray[2],
                    worldItemDetailsArray[3],
                    scene
                    );
                if (!itemExists) {
                    DbCommands.InsertTupleToTable("PremadeWorldItems",
                                                    worldItemDetailsArray[0], //x
                                                    worldItemDetailsArray[1], //y
                                                    worldItemDetailsArray[2], //z
                                                    worldItemDetailsArray[3], //parent
                                                    scene,
                                                    worldItemDetailsArray[4]); //item name
                }
            }


        }

        public string GetCurrentTaskID() {
            return (GetSelectedItemFromGroup(selectedTask) as Task).MyID; 
        } 
    }
}