using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Mono.Data.Sqlite;
using DataUI.Utilities;
using DataUI.ListItems;
using DbUtilities;
using System.Data;
using System.Collections.Generic;


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
        public string selectedQuest;
        public GameObject QuestPrefab;
        //Quests
        GameObject questsListUI, questsListUIPanel, questsList,
                   questDetails, questDetailsPanel, activateQuestDetailsBtn;
        private InputField inputQuestDetailsNameInput, inputQuestDetailsDescInput; //new quest components
        Toggle dialogueActive;


        void Start() {

            //QUEST COMPONENTS
            //add
            print(GetPanel());
            print(GetPanel().transform.GetChild(0));
            questsListUI = GetPanel().transform.FindChild("QuestsListUI").gameObject;
            questsListUIPanel = questsListUI.transform.FindChild("Panel").gameObject;
            questsList = questsListUIPanel.transform.FindChild("QuestsList").gameObject;
            questDetails = questsListUIPanel.transform.FindChild("QuestDetails").gameObject;
            questDetailsPanel = questDetails.transform.FindChild("QuestDetailsPanel").gameObject;
            activateQuestDetailsBtn = questDetails.transform.FindChild("ActivateQuestDetailsBtn").gameObject;
            inputQuestDetailsNameInput = questDetailsPanel.transform.FindChild("InputNameText").GetComponent<InputField>();
            inputQuestDetailsDescInput = questDetailsPanel.transform.FindChild("InputDescriptionText").GetComponent<InputField>();
            
            FillDisplayFromDb(DBqueries.GetQuestsDisplayQry(), questsList.transform, BuildQuest);

            selectedQuest = "selectedQuest";

            CreateSelectionToggleGroup(selectedQuest);

        }

        public void EditSelectedQuest() {
            editMode = true;
            activateQuestDetailsBtn.GetComponent<Text>().text = "Edit quest";
            DisplayQuestDetails();
            activateQuestDetailsBtn.GetComponent<Button>().interactable = false;
            string[] questsData = DbCommands.GetTupleFromTable("Quests",
            "QuestNames = '" + ((GetSelectedItemFromGroup(selectedQuest)) as Quest).MyName + "'");
            inputQuestDetailsNameInput.text = questsData[0];
            inputQuestDetailsDescInput.text = questsData[1];
        }

        public void DisplayQuestDetails() {
            activateQuestDetailsBtn.GetComponent<Button>().interactable = false;
            questDetailsPanel.SetActive(true);
            if (!IsEditingDetails()) {
                inputQuestDetailsNameInput.text = null;
                inputQuestDetailsDescInput.text = null;
            }
        }

        public void HideQuestDetails() {
            activateQuestDetailsBtn.GetComponent<Button>().interactable = true;
            questDetailsPanel.SetActive(false);
            editMode = false;
            activateQuestDetailsBtn.GetComponent<Text>().text = "New quest";
        }

        public void SaveQuestDetails() {
            Quest selectedQuestObj = (GetSelectedItemFromGroup(selectedQuest) as Quest);
            if (inputQuestDetailsNameInput != null) {
                if (selectedQuestObj != null) {
                    if (selectedQuestObj.IsInEditMode()) {
                        string[,] fields = new string[,]{
                                                { "QuestNames", inputQuestDetailsNameInput.text },
                                                { "QuestDescriptions", inputQuestDetailsDescInput.text }
                                             };
                        DbCommands.UpdateTableTuple("Quests",
                                                 "QuestNames = " 
                                                    + DbCommands.GetParameterNameFromValue(inputQuestDetailsNameInput.text),
                                                 fields,
                                                 inputQuestDetailsNameInput.text);
                        selectedQuestObj.MyName = inputQuestDetailsNameInput.text;
                        selectedQuestObj.MyDescription = inputQuestDetailsDescInput.text;
                        selectedQuestObj.GetInputField().text = inputQuestDetailsNameInput.text;
                        //ToggleSelectionTo(GetComponent<Quest>(), selectedQuest);
                    }
                }
                else {
                    //inputQuestDetailsNameInput.text = null;
                    //inputQuestDetailsDescInput.text = null;
                    DbCommands.InsertTupleToTable("Quests",
                                            inputQuestDetailsNameInput.text,
                                            inputQuestDetailsDescInput.text);
                    FillDisplayFromDb(DBqueries.GetQuestsDisplayQry(), questsList.transform, BuildQuest);
                    
                }
            }

        }

        private Transform BuildQuest(string[] strArray) {
            string nameStr = strArray[0];
            string detailsStr = strArray[1];
            GameObject quest = Instantiate(QuestPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            quest.transform.GetComponentInChildren<InputField>().text = nameStr;
            quest.GetComponent<Quest>().MyName = nameStr;
            quest.GetComponent<Quest>().MyDescription = detailsStr;
            return quest.transform;
        }

        public bool IsEditingDetails() {
            return editMode;
        }
    }
}