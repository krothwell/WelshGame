using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI.ListItems;
using DbUtilities;
using UnityEngine.UI;

namespace GameUI {
    public class QuestsUI : UIController {
        public GameObject questPrefab;
        public string selectedQuest;
        Dictionary<string, Quest> questDict;
        GameObject questsList;
        List<string> questNotifierQueue;
        // Use this for initialization
        void Start() {
            selectedQuest = "selectedQuest";
            CreateSelectionToggleGroup(selectedQuest);
            questDict = new Dictionary<string, Quest>();
            questsList = GetPanel().transform.FindChild("QuestsScroller").FindChild("QuestsList").gameObject;
            DisplayQuests();
            FillQuestDictionary();
            questNotifierQueue = new List<string>();
        }

        public void DisplayQuests() {
            FillDisplayFromDb(DbQueries.GetActivatedQuestsInCurrentGame(), questsList.transform, BuildQuest);
        }

        public Transform BuildQuest(string[] questData) {
            string questName = questData[0];
            print(questName);
            GameObject quest = Instantiate(questPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            //print(quest);
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
            DbCommands.InsertTupleToTable("QuestsActivated", questName, "0");
            QueueQuestNotifier(questName);
            string[] questData = new string[1];
            questData[0] = questName;
            GameObject newQuest = BuildQuest(questData).gameObject;
            newQuest.transform.SetParent(questsList.transform, false);
            AddQuestToDictionary(newQuest);
        }

        public void QueueQuestNotifier(string questName) {
            questNotifierQueue.Add(questName);
        }

        public void DisplayQuestNotifications() {
            foreach(string qn in questNotifierQueue) {
                print("Quest notifier: " + qn);
            }
        }

        public void AddQuestToDictionary(GameObject newQuest) {
            questDict.Add(newQuest.GetComponent<Quest>().MyName, newQuest.GetComponent<Quest>());
        }

        public void SelectQuest(string questName) {
            Quest quest = questDict[questName];
            quest.SelectSelf();
        }
    }

}