using System;
using System.Collections.Generic;
using UnityEngine;
using GameUI.ListItems;
using DbUtilities;
using UnityEngine.UI;
using UnityUtilities;

namespace GameUI {
    public class QuestsUI : UIController {
        public GameObject questPrefab, taskPrefab;
        public string selectedQuest;
        Dictionary<string, Quest> questDict;
        GameObject questsList, questDetailsList, questTasksList;
        NotificationQueue notificationQueue;
        CombatUI combatUI;
        Text selectedQuestTitleLbl, selectedQuestDescriptionLbl;
        DialogueUI dialogueUI;

        // Use this for initialization
        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
            combatUI = FindObjectOfType<CombatUI>();
            notificationQueue = FindObjectOfType<NotificationQueue>();
            selectedQuest = "selectedQuest";
            CreateSelectionToggleGroup(selectedQuest);
            questDict = new Dictionary<string, Quest>();
            questsList = GetPanel().transform.Find("QuestsScroller").Find("QuestsList").gameObject;
            questDetailsList = GetPanel().transform.Find("QuestDetailsScroller").Find("QuestDetailsList").gameObject;
            questTasksList = questDetailsList.transform.Find("SelectedQuestTasksList").gameObject;
            selectedQuestTitleLbl = questDetailsList.transform.Find("SelectedQuestTitleLbl").GetComponent<Text>();
            selectedQuestDescriptionLbl = questDetailsList.transform.Find("SelectedQuestDescriptionLbl").GetComponent<Text>();
            DisplayQuests();
            FillQuestDictionary();
        }

        public void DisplayQuests() {
            FillDisplayFromDb(DbQueries.GetActivatedQuestsInCurrentGame(), questsList.transform, BuildQuest);
        }

        public Transform BuildQuest(string[] questData) {
            Debugging.PrintDbQryResults(DbQueries.GetActivatedQuestsInCurrentGame());
            string questName = questData[0];
            bool completed = Convert.ToBoolean(int.Parse(questData[2]));
            print(questName);
            GameObject quest = Instantiate(questPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            //print(quest);
            if (completed) {
                quest.GetComponent<Quest>().SetCompleted();
            }
            quest.GetComponent<Quest>().MyName = questName;
            quest.GetComponentInChildren<Text>().text = questName;

            return quest.transform;
        }

        public void FillQuestDictionary() {
            foreach (Transform questGO in questsList.transform) {
                Quest quest = questGO.GetComponent<Quest>();
                string questName = quest.MyName;
                if (quest.MyName != null) {
                    if (!questDict.ContainsKey(questName)) {
                        questDict.Add(quest.MyName, quest);
                    }
                }
            }
        }

        public void InsertActivatedQuest(string questName) {
            DbCommands.InsertTupleToTable("QuestsActivated", questName, "0", "0");
            notificationQueue.QueueNewQuestNotifier(questName);
            string[] questData = new string[3];
            questData[0] = questName;
            questData[1] = "0"; //current game
            questData[2] = "0"; //uncompleted
            GameObject newQuest = BuildQuest(questData).gameObject;
            newQuest.transform.SetParent(questsList.transform, false);
            AddQuestToDictionary(newQuest);
        }

        public void InsertActivatedTask (string taskID, string questName, string taskDesc) {
            DbCommands.InsertTupleToTable("QuestTasksActivated", taskID, "0", "0");
            notificationQueue.QueueUpdatedQuestNotifier(questName);
        }

        public void AddQuestToDictionary(GameObject newQuest) {
            string questName = newQuest.GetComponent<Quest>().MyName;
            if (!questDict.ContainsKey(questName)) {
                questDict.Add(newQuest.GetComponent<Quest>().MyName, newQuest.GetComponent<Quest>());
            }
        }

        public void SelectQuest(string questName) {
            Quest quest = questDict[questName];
            SetQuestDetails(questName);
        }

        public void ToggleToQuestNotified (string questName) {
            DisplayComponents();
            ToggleSelectionTo(questDict[questName], selectedQuest);
        }

        public void SetQuestDetails(string questName) {
            string[] questData = DbCommands.GetTupleFromTable("Quests", "QuestNames = " + DbCommands.GetParameterNameFromValue(questName), "QuestNames", questName);
            selectedQuestTitleLbl.text = questData[0];
            selectedQuestDescriptionLbl.text = questData[1];
            DisplayTasksRelatedToQuest(questName);
        }

        public void DisplayTasksRelatedToQuest(string questName) {
            print("DISPLAYING TASKS RELATED TO QUEST");
            FillDisplayFromDb(DbQueries.GetTasksRelatedToQuest(questName), questTasksList.transform, BuildQuestTask, questName);
        }

        public Transform BuildQuestTask(string[] taskData) {
            string taskID = taskData[0];
            string taskDesc = taskData[1];
            print(DbCommands.GetFieldValueFromTable("QuestTasksActivated", "Completed", "TaskIDs = " + taskID + " AND SaveIDs = 0"));
            bool completed = Convert.ToBoolean(int.Parse(DbCommands.GetFieldValueFromTable("QuestTasksActivated", "Completed", "TaskIDs = " + taskID + " AND SaveIDs = 0")));
            GameObject task = Instantiate(taskPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            //print(quest);
            if (completed) {
                task.GetComponent<QuestTask>().SetCompleted();
            }
            task.GetComponent<QuestTask>().SetDescription(taskDesc);
            task.GetComponent<QuestTask>().MyID = taskID;

            return task.transform;
        }

        public void SelectFirstQuest() {
            if (GetSelectedItemFromGroup(selectedQuest) == null) {
                if (questsList.transform.childCount > 0) {
                    ToggleSelectionTo(questsList.transform.GetChild(0).GetComponent<Quest>(), selectedQuest);
                }
            }
            else {
                print("NEED TO SELECT SELECTED QUEST");
                ToggleSelectionTo(GetSelectedItemFromGroup(selectedQuest),selectedQuest);
            }
        }

        public void CompleteEquipItemTaskPart(string itemName) {
            print("Complete equip item task part");
            Debugging.PrintDbQryResults(DbQueries.GetEquipItemTasksData(itemName, "0"), itemName);
            List<string[]> partsToComplete = new List<string[]>();
            DbCommands.GetDataStringsFromQry(DbQueries.GetEquipItemTasksData(itemName, "0"), out partsToComplete, itemName);
            if (partsToComplete.Count > 0) {
                foreach (string[] tuple in partsToComplete) {
                    print("completing task part " + itemName);
                    CompleteTaskPart(tuple[0], tuple[1], tuple[2]);
                    print("task part completed");
                }
            }
        }

        public void CompleteTaskPart(string partID, string taskID, string questName) {
            
            DbCommands.InsertTupleToTable("CompletedQuestTaskParts", partID, "0");
            //print("inserted tuple to completed task parts");
            int taskPartsCount = DbCommands.GetCountFromTable("QuestTaskParts", "TaskIDs = " + taskID);
            //print("task parts with id " + taskID + " = " + taskPartsCount);
            int taskPartsCompletedCount = DbCommands.GetCountFromQry(DbQueries.GetTaskPartsCompleteFromTaskID(taskID, "0"));
            //print("task parts completed with id " + taskID + " = " + taskPartsCompletedCount);
            //Debugging.PrintDbTable("CompletedQuestTaskParts");
            if (taskPartsCount == taskPartsCompletedCount) {
                DbCommands.UpdateTableField("QuestTasksActivated", "Completed", "1", "TaskIDs = " + taskID + " AND SaveIDs = 0");
                int tasksCount = DbCommands.GetCountFromTable(
                    "QuestTasks", 
                    "QuestNames = " + DbCommands.GetParameterNameFromValue(questName),
                    questName);
                string[] taskResults = DbCommands.GetTupleFromQry(DbQueries.GetAllTaskResultsQry(taskID));
                //string startDialogueID = DbCommands.GetFieldValueFromTable("QuestTaskStartDialogueResults", "DialogueIDs", "TaskIDs = " + taskID);
                string endCombatWithCharName = taskResults[0];
                string startDialogueID = taskResults[1];
                if (endCombatWithCharName != "") {
                    combatUI.EndCombatWithCharacter(endCombatWithCharName);
                }
                if (startDialogueID != "") {
                    print(startDialogueID);
                    dialogueUI.StartNewDialogue(startDialogueID);
                }
                int tasksCompletedCount = DbCommands.GetCountFromQry(
                    DbQueries.GetTasksCompleteFromQuestName(questName, "0"),
                    questName);
                if (tasksCount == tasksCompletedCount) {
                    DbCommands.UpdateTableField("QuestsActivated", 
                        "Completed", "1", 
                        "SaveIDs = 0 AND QuestNames = " + DbCommands.GetParameterNameFromValue(questName), 
                        questName);
                }
            }
            SetQuestDetails(questName);
        }
    }

}