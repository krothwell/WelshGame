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
    /// Responsible for menus to add new dialogues and related npcs, nodes, 
    /// choices and choice results to the database and displaying those in lists
    /// so that they can be edited or deleted. Dialogues created here will
    /// automatically start in game (the dialogue processed using the GameUI.
    /// DialogueUI class) where the player engages with an NPC related to the 
    /// dialogue. 
    /// </summary>
    public class DialogueUI : UIController {
        UIController ui;
        private enum ChoiceResultOptions {
            DialogueNodes
        };

        public string selectedDialogue,
                       selectedNode,
                       selectedChoice,
                       selectedChoiceResult;
        //DIALOGUES
        GameObject dialoguesListUI, dialoguesPanel, dialogueList,
                   submitNewDialogue, newDialoguePanel, activateNewDialogueBtn;
        public GameObject dialoguePrefab;
        InputField inputShortDescriptionText; //new dialogue components
        Toggle dialogueActive;

        //CHARACTERS
        GameObject charDialoguesPanel,
                   characterList,
                   addCharDialoguesUI, addCharDialoguesPanel, addCharDialogueList, activateAddCharBtn, 
                   selectedCharLink;
        public GameObject characterLinkPrefab, charDialoguePrefab;
        GameObject charDialoguesListUI;

        //NODES
        GameObject dialogueNodesListUI, dialogueNodesPanel, dialogueNodesScrollView, dialogueNodesList,
                   nodeDetailsUI, nodeDetailsUIPanel, activateNodeDetailsBtn,
                   selectNodeSpeakersUI, selectNodeSpeakersUIpanel, nodeSpeakerBtn, charOverrideList, selectedCharOverride;
        InputField inputNodeText;
        Toggle endDialogueOptionToggle;
        public GameObject dialogNodePrefab, charOverridePrefab;
        bool editingNode = false;

        //PLAYER CHOICES
        GameObject playerChoicesListUI, playerChoicesPanel, playerChoicesList,
                   playerChoiceDetails, newPlayerChoicePanel, displayPlayerChoiceDetailsBtn;
        InputField inputChoiceText;
        public GameObject playerChoicePrefab;
        bool editingChoice = false;

        //PLAYER CHOICE RESULTS
        GameObject playerChoiceResultsListUI, playerChoiceResultsPanel, playerChoicesResultsList,
                   newChoiceResultUI, displayNewChoiceResultBtn, newChoiceResultPanel, selectedResultTypeList,
                   selectedChoiceResultOption;
        ChoiceResultOptions choiceResultOptions;
        ScrollRect choiceResultOptionsScrollView;
        public GameObject pChoiceResultTitlePrefab,
                          pChoiceResultPrefab,
                          optionChoiceResultPrefab;
        

        void Start() {
            
            UpdateCharactersTableFromGame();
            choiceResultOptions = new ChoiceResultOptions();

            ui = FindObjectOfType<UIController>();
            //panel = transform.FindChild("Panel").gameObject;

            //DIALOGUE COMPONENTS
            dialoguesListUI = GetPanel().transform.FindChild("DialoguesListUI").gameObject;
            dialoguesPanel = dialoguesListUI.transform.FindChild("Panel").gameObject;
            dialogueList = dialoguesPanel.transform.FindChild("DialoguesList").gameObject; 
             //add
             submitNewDialogue = dialoguesPanel.transform.FindChild("SubmitNewDialog").gameObject;
            newDialoguePanel = submitNewDialogue.transform.FindChild("NewDialogPanel").gameObject;
            activateNewDialogueBtn = submitNewDialogue.transform.FindChild("ActivateNewDialogBtn").gameObject;
            inputShortDescriptionText = newDialoguePanel.transform.FindChild("InputShortDescriptionText").GetComponent<InputField>();
            dialogueActive = newDialoguePanel.transform.GetComponentInChildren<Toggle>();

            //CHARACTER DIALOGUES COMPONENTS
            charDialoguesListUI = GetPanel().transform.FindChild("CharDialoguesListUI").gameObject;
            charDialoguesPanel = charDialoguesListUI.transform.FindChild("CharDialoguesPanel").gameObject;
            characterList = charDialoguesPanel.transform.FindChild("CharDialoguesList").gameObject;
            //add
            addCharDialoguesUI = charDialoguesPanel.transform.FindChild("AddCharDialoguesUI").gameObject;
            addCharDialoguesPanel = addCharDialoguesUI.transform.FindChild("AddCharDialoguesPanel").gameObject;
            addCharDialogueList = addCharDialoguesPanel.transform.FindChild("CharacterList").gameObject;
            activateAddCharBtn = addCharDialoguesUI.transform.FindChild("ActivateAddCharBtn").gameObject;

            //NODE COMPONENTS
            dialogueNodesListUI = GetPanel().transform.FindChild("DialogueNodesListUI").gameObject;
            dialogueNodesPanel = dialogueNodesListUI.transform.FindChild("DialogueNodesPanel").gameObject;
            dialogueNodesScrollView = dialogueNodesListUI.transform.GetComponentInChildren<ScrollRect>().gameObject;
            dialogueNodesList = dialogueNodesScrollView.transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
            //add
            nodeDetailsUI = dialogueNodesPanel.transform.FindChild("NodeDetailsUI").gameObject;
            nodeDetailsUIPanel = nodeDetailsUI.transform.FindChild("Panel").gameObject;
            activateNodeDetailsBtn = nodeDetailsUI.transform.FindChild("ActivateNodeDetailsBtn").gameObject;
            inputNodeText = nodeDetailsUIPanel.transform.GetComponentInChildren<InputField>();
            endDialogueOptionToggle = nodeDetailsUIPanel.transform.GetComponentInChildren<Toggle>();
            selectNodeSpeakersUI = nodeDetailsUIPanel.transform.FindChild("SelectNodeSpeakersUI").gameObject;
            selectNodeSpeakersUIpanel = selectNodeSpeakersUI.transform.FindChild("Panel").gameObject;
            nodeSpeakerBtn = nodeDetailsUIPanel.transform.FindChild("NodeSpeakerBtn").gameObject;
            charOverrideList = selectNodeSpeakersUIpanel.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

            //PLAYER CHOICE COMPONENTS
            playerChoicesListUI = GetPanel().transform.FindChild("PlayerChoicesListUI").gameObject;
            playerChoicesPanel = playerChoicesListUI.transform.FindChild("Panel").gameObject;
            playerChoicesList = playerChoicesPanel.transform.FindChild("PlayerChoicesList").gameObject;
            //add
            playerChoiceDetails = playerChoicesPanel.transform.FindChild("PlayerChoiceDetails").gameObject;
            newPlayerChoicePanel = playerChoiceDetails.transform.FindChild("NewPlayerChoicePanel").gameObject;
            displayPlayerChoiceDetailsBtn = playerChoiceDetails.transform.FindChild("DisplayPlayerChoiceDetailsBtn").gameObject;
            inputChoiceText = newPlayerChoicePanel.transform.GetComponentInChildren<InputField>();

            //PLAYER CHOICE RESULTS COMPONENTS
            playerChoiceResultsListUI = GetPanel().transform.FindChild("PlayerChoiceResultsListUI").gameObject;
            playerChoiceResultsPanel = playerChoiceResultsListUI.transform.FindChild("Panel").gameObject;
            playerChoicesResultsList = playerChoiceResultsPanel.transform.FindChild("PlayerChoiceResultsList").gameObject;
            //add
            newChoiceResultUI = playerChoiceResultsPanel.transform.FindChild("NewChoiceResultUI").gameObject;
            displayNewChoiceResultBtn = newChoiceResultUI.transform.FindChild("DisplayNewChoiceResultBtn").gameObject;
            newChoiceResultPanel = newChoiceResultUI.transform.FindChild("Panel").gameObject;
            choiceResultOptionsScrollView = newChoiceResultPanel.GetComponentInChildren<ScrollRect>();
            selectedResultTypeList = choiceResultOptionsScrollView.transform.FindChild("SelectedResultTypeList").gameObject;

            //display dialogue list
            print("print dialogueList: " +dialogueList);
            ui.FillDisplayFromDb(GetDialogueDisplayQry(), dialogueList.transform, BuildDialogue);

            selectedDialogue = "selectedDialogue";
            selectedNode = "selectedNode";
            selectedChoice = "selectedChoice";
            selectedChoiceResult = "selectedChoiceResult";
            CreateSelectionToggleGroup(selectedDialogue);
            CreateSelectionToggleGroup(selectedChoice);
            CreateSelectionToggleGroup(selectedChoiceResult);
            CreateSelectionToggleGroup(selectedNode);
        }

        public void ActivateNewDialogue() {
            newDialoguePanel.SetActive(true);
            activateNewDialogueBtn.GetComponent<Button>().interactable = false;
        }

        public void ActivateCharacterLink() {
            addCharDialoguesPanel.SetActive(true);
            activateAddCharBtn.GetComponent<Button>().interactable = false;
            ui.FillDisplayFromDb(GetCharLinkDisplayQry(), addCharDialogueList.transform, BuildAddCharDialogueBtn);
        }

        public void ActivateEditNewNode() {
            nodeDetailsUIPanel.SetActive(true);
            activateNodeDetailsBtn.GetComponent<Button>().interactable = false;
            if (editingNode) {
                string[] nodeDesc = DbCommands.GetTupleFromTable("DialogueNodes",
                    "NodeIDs = " + ((GetSelectedItemFromGroup(selectedNode)) as DialogueNode).MyID + ";");
                inputNodeText.text = nodeDesc[1];
                bool endDialogueOptionBool = false;
                if (nodeDesc[5] != "") {
                    endDialogueOptionBool = (int.Parse(nodeDesc[5]) == 1) ? true : false;
                }
                print(endDialogueOptionToggle);
                endDialogueOptionToggle.isOn = endDialogueOptionBool;
                SetOverrideBtnTxt(nodeDesc[3], nodeDesc[4]);
            }
            else {
                ClearEditNodeDetails();
            }

        }

        private void ClearEditNodeDetails() {
            inputNodeText.text = "";
            endDialogueOptionToggle.isOn = false;
            SetOverrideBtnTxt("<i>None</i>", "");
        }

        public void ActivateNewCharacterOverride() {
            selectNodeSpeakersUI.SetActive(true);
            nodeSpeakerBtn.GetComponent<Button>().interactable = false;
            string dialogueID = ((GetSelectedItemFromGroup(selectedNode)) as DialogueNode).MyID;
            /*We only want to pick characters from scenes which are the same as those of the characters related to the dialogue
             * as a whole */
            string qry = "SELECT * FROM Characters "
                + "WHERE (Characters.Scenes IN (SELECT Scenes FROM CharacterDialogues "
                    + "WHERE DialogueIDs = " + dialogueID + ")) "
                + "OR (Characters.CharacterNames = '!Player') "
                + "ORDER BY CharacterNames ASC;";
            ui.FillDisplayFromDb(qry, charOverrideList.transform, BuildDialogueNodeSpeaker);
        }

        public void ActivateEditNewChoice() {
            newPlayerChoicePanel.SetActive(true);
            displayPlayerChoiceDetailsBtn.GetComponent<Button>().interactable = false; //indicate to user that button no longer functions.
            if (editingChoice) {
                string[] choiceDesc = DbCommands.GetTupleFromTable("PlayerChoices",
                    "ChoiceIDs = " + ((GetSelectedItemFromGroup(selectedChoice)) as PlayerChoice).GetComponent<PlayerChoice>().MyID + ";");
                inputChoiceText.text = choiceDesc[1];
            }
        }

        public void ActivateNewChoiceResult() {
            newChoiceResultPanel.SetActive(true);
            displayNewChoiceResultBtn.GetComponent<Button>().interactable = false; //indicate to user that button no longer functions.
        }

        public void DeactivateNewDialogue() {
            newDialoguePanel.SetActive(false);
            inputShortDescriptionText.GetComponent<InputField>().text = "";
            activateNewDialogueBtn.GetComponent<Button>().interactable = true;
        }

        public void DeactivateCharacterLink() {
            addCharDialoguesPanel.SetActive(false);
            activateAddCharBtn.GetComponent<Button>().interactable = true;
        }

        public void DeactivateNewNode() {
            nodeDetailsUIPanel.SetActive(false);
            activateNodeDetailsBtn.GetComponent<Button>().interactable = true;
            activateNodeDetailsBtn.GetComponent<Text>().text = "New node";
            editingNode = false;
        }

        public void DeactivateNewCharacterOverride() {
            selectNodeSpeakersUI.SetActive(false);
            nodeSpeakerBtn.GetComponent<Button>().interactable = true;
        }

        private void SetOverrideBtnTxt(string name, string scene) {
            string nameTxt = (name == "") ? "<i>None</i>" : name;
            string btnTxt = (scene != "") ? nameTxt + " <i>in " + scene + "</i>" : nameTxt;
            nodeSpeakerBtn.GetComponent<Text>().text = btnTxt;
        }

        public void DeactivateNewPlayerChoice() {
            newPlayerChoicePanel.SetActive(false);
            displayPlayerChoiceDetailsBtn.GetComponent<Button>().interactable = true;
            displayPlayerChoiceDetailsBtn.GetComponent<Text>().text = "New choice";
        }

        public void DeactivateNewChoiceResult() {
            newChoiceResultPanel.SetActive(false);
            displayNewChoiceResultBtn.GetComponent<Button>().interactable = true; //indicate to user that button no longer functions.
        }

        public void InsertNewDialogue() {
            if ((inputShortDescriptionText.text != null) && (inputShortDescriptionText.text != "")) {
                string dialogueActiveStr = dialogueActive.isOn ? "1" : "0";
                string dialogueID = DbCommands.GenerateUniqueID("Dialogues", "DialogueIDs", "DialogueID");
                DbCommands.InsertTupleToTable("Dialogues", dialogueID, inputShortDescriptionText.text, dialogueActiveStr);
                ui.FillDisplayFromDb(GetDialogueDisplayQry(), dialogueList.transform, BuildDialogue);
                HideCharsRelatedToDialogue();
                HideNodesRelatedToDialogue();
            }
        }

        public void InsertNewCharLink() {
            DbCommands.InsertTupleToTable("CharacterDialogues",
                                        selectedCharLink.GetComponent<AddCharDialogueBtn>().CharacterName,
                                        selectedCharLink.GetComponent<AddCharDialogueBtn>().SceneName,
                                        (GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID);
        }

        public void UpdateInsertNewNode() {
            string endDialogueStr = endDialogueOptionToggle.isOn ? "1" : "0";
            if (inputNodeText.text != null) {
                DialogueNodeSpeaker charOverride;
                if (selectedCharOverride != null) {
                    charOverride = selectedCharOverride.GetComponent<DialogueNodeSpeaker>();
                }
                else {
                    charOverride = null;
                }
                string overrideName = (charOverride != null) ? charOverride.CharacterName : "null";
                string overrideScene = (charOverride != null) ? charOverride.SceneName : "null";
                if (editingNode) {
                    string[,] fieldVals = new string[,] {
                                                { "NodeText", inputNodeText.text },
                                                { "EndDialogueOption", endDialogueStr },
                                                { "CharacterSpeaking", overrideName },
                                                { "Scenes", overrideScene }
                                            };
                    DbCommands.UpdateTableTuple("DialogueNodes", "NodeIDs = " + (GetSelectedItemFromGroup(selectedNode) as DialogueNode).MyID, fieldVals);
                    (GetSelectedItemFromGroup(selectedNode) as DialogueNode).UpdateNodeDisplay(inputNodeText.text);
                }
                else {
                    string nodeID = DbCommands.GenerateUniqueID("DialogueNodes", "NodeIDs", "NodeID");
                    DbCommands.InsertTupleToTable("DialogueNodes",
                                            nodeID,
                                            inputNodeText.text,
                                            (GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID,
                                            overrideName,
                                            overrideScene,
                                            endDialogueStr);
                    ui.FillDisplayFromDb(GetDialogueNodeDisplayQry(), dialogueNodesList.transform, BuildDialogueNode);
                    HidePlayerChoices();
                }
            }
        }

        public void SetDialogueCharacterOverride(GameObject charOverride) {
            selectedCharOverride = charOverride;
            string name = charOverride.GetComponent<DialogueNodeSpeaker>().CharacterName;
            string scene = charOverride.GetComponent<DialogueNodeSpeaker>().SceneName;
            SetOverrideBtnTxt(name, scene);
        }

        public void UpdateInsertNewPlayerChoice() {
            if (inputChoiceText.text != null) {
                if (editingChoice) {
                    string[,] fieldVals = new string[,] {
                                                { "ChoiceText", inputChoiceText.text },
                                            };
                    DbCommands.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID, fieldVals);
                    (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).UpdateChoiceDisplay(inputChoiceText.text);
                }
                else {
                    string choiceID = DbCommands.GenerateUniqueID("PlayerChoices", "ChoiceIDs", "ChoiceID");
                    DbCommands.InsertTupleToTable("PlayerChoices",
                                            choiceID,
                                            inputChoiceText.text,
                                            (GetSelectedItemFromGroup(selectedNode) as DialogueNode).MyID,
                                            null);
                    ui.FillDisplayFromDb(GetPlayerChoiceDisplayQry(), playerChoicesList.transform, BuildPlayerChoice);
                }
            }

        }

        public void InsertNewChoiceResult() {
            switch (choiceResultOptions) {
                case ChoiceResultOptions.DialogueNodes:
                    print("InsertNewChoiceResult");
                    NewPlayerChoiceResultBtn newPlayerChoiceResultBtn = selectedChoiceResultOption.GetComponent<NewPlayerChoiceResultBtn>();
                    (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyNextNode = newPlayerChoiceResultBtn.MyID;
                    string[,] fieldVals = new string[,] { { "NextNodes", newPlayerChoiceResultBtn.MyID } };
                    DbCommands.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID, fieldVals);
                    break;
            }
            DisplayResultsRelatedToChoices();
            DeactivateNewChoiceResult();
        }

        private Transform BuildDialogue(string[] strArray) {
            string idStr = (strArray[0]);
            string descStr = (strArray[1]);
            bool activeBool = (int.Parse(strArray[2]) == 1) ? true : false;
            GameObject dialogue = Instantiate(dialoguePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            dialogue.transform.FindChild("DescriptionInput").GetComponent<InputField>().text = descStr;
            dialogue.transform.Find("DialogueID").GetComponent<Text>().text = idStr;
            dialogue.transform.GetComponentInChildren<Toggle>().isOn = activeBool;
            dialogue.GetComponent<Dialogue>().MyID = idStr;
            dialogue.GetComponent<Dialogue>().MyDescription = descStr;
            dialogue.GetComponent<Dialogue>().Active = activeBool;
            return dialogue.transform;
        }

        private Transform BuildAddCharDialogueBtn(string[] strArray) {
            string charName = (strArray[0]);
            string sceneName = (strArray[1]);
            GameObject charLink = Instantiate(characterLinkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            string sceneStr = " <i>in " + sceneName + "</i>";
            if (sceneName == "") {
                sceneStr = " <i>scene not found</i>";
                charLink.GetComponent<AddCharDialogueBtn>().DisplayDeletionOption();
            }
            charLink.transform.FindChild("CharacterName").GetComponent<Text>().text = charName + sceneStr;
            charLink.GetComponent<AddCharDialogueBtn>().CharacterName = charName;
            charLink.GetComponent<AddCharDialogueBtn>().SceneName = sceneName;

            return charLink.transform;
        }

        private Transform BuildCharacterDialogue(string[] strArray) {
            string charName = (strArray[0]);
            string sceneName = (strArray[1]);
            string dialogueID = (strArray[2]);
            string sceneStr = " <i>scene not found</i>";
            if (sceneName != "") {
                sceneStr = " <i>in " + sceneName + "</i>";
            }
            GameObject charDialogue = Instantiate(charDialoguePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            charDialogue.transform.FindChild("CharacterInput").GetComponent<InputField>().text = charName + sceneStr;
            charDialogue.GetComponent<CharacterDialogue>().CharacterName = charName;
            charDialogue.GetComponent<CharacterDialogue>().DialogueID = dialogueID;
            charDialogue.GetComponent<CharacterDialogue>().SceneName = sceneName;
            return charDialogue.transform;
        }

        private Transform BuildDialogueNode(string[] strArray) {
            string idStr = (strArray[0]);
            string nodeText = (strArray[1]);
            GameObject dialogNode = Instantiate(dialogNodePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            dialogNode.transform.GetComponentInChildren<InputField>().text = nodeText;
            dialogNode.transform.Find("NodeID").GetComponent<Text>().text = idStr;
            dialogNode.GetComponent<DialogueNode>().MyID = idStr;
            dialogNode.GetComponent<DialogueNode>().MyText = nodeText;
            return dialogNode.transform;
        }

        private Transform BuildDialogueNodeSpeaker(string[] strArray) {
            string charName = strArray[0];
            string sceneName = strArray[1];
            GameObject charOverride = Instantiate(charOverridePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            string sceneStr = " <i>in " + sceneName + "</i>";
            if (sceneName == "") {
                sceneStr = " <i>scene not found</i>";
            }
            charOverride.GetComponent<Text>().text = charName + sceneStr;
            charOverride.GetComponent<DialogueNodeSpeaker>().CharacterName = charName;
            charOverride.GetComponent<DialogueNodeSpeaker>().SceneName = sceneName;

            return charOverride.transform;
        }

        private Transform BuildPlayerChoice(string[] strArray) {
            string idStr = strArray[0];
            string choiceText = strArray[1];
            string nextNode = strArray[3];
            GameObject playerChoice = Instantiate(playerChoicePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            playerChoice.transform.GetComponentInChildren<InputField>().text = choiceText;
            playerChoice.transform.Find("ChoiceID").GetComponent<Text>().text = idStr;
            playerChoice.GetComponent<PlayerChoice>().MyID = idStr;
            playerChoice.GetComponent<PlayerChoice>().MyText = choiceText;
            playerChoice.GetComponent<PlayerChoice>().MyNextNode = nextNode;
            return playerChoice.transform;
        }

        private Transform BuildNewChoiceResultNode(string[] strArray) {
            string idStr = strArray[0];
            string nodeText = strArray[1];
            GameObject optionChoiceResult = Instantiate(optionChoiceResultPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            optionChoiceResult.transform.GetComponentInChildren<InputField>().text = nodeText;
            optionChoiceResult.transform.Find("NodeID").GetComponent<Text>().text = idStr;
            optionChoiceResult.GetComponent<NewPlayerChoiceResultBtn>().MyID = idStr;
            optionChoiceResult.GetComponent<NewPlayerChoiceResultBtn>().MyText = nodeText;
            return optionChoiceResult.transform;
        }

        private Transform BuildChoiceResultNode(string[] strArray) {
            string idStr = strArray[0];
            string nodeText = strArray[1];
            GameObject choiceResult = Instantiate(pChoiceResultPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            choiceResult.transform.GetComponentInChildren<InputField>().text = string.Format("Node ID <b>{0}</b>: \"{1}\"", idStr, nodeText);
            choiceResult.GetComponent<PlayerChoiceResult>().MyID = idStr;
            choiceResult.GetComponent<PlayerChoiceResult>().MyText = nodeText;
            return choiceResult.transform;
        }

        //public void SetSelectedListItem(GameObject go) {
        //    if (selectedDialogue != null) {
        //        if (selectedDialogue != go) {
        //            selectedDialogue.GetComponent<ISelectableUI>().DeselectSelf();
        //        }
        //    }
        //    if (selectedDialogue != go) {
        //        selectedDialogue = go;
        //        selectedDialogue.GetComponent<ISelectableUI>().SelectSelf();
        //    }
        //}

        //public void SetSelectedDialogueNode(GameObject nodeInput) {
        //    if (selectedNode != null) {
        //        if (selectedNode != nodeInput) {
        //            selectedNode.GetComponent<DialogueNode>().DeselectSelf();
        //        }
        //    }
        //    if (selectedNode != nodeInput) {
        //        selectedNode = nodeInput;
        //        selectedNode.GetComponent<DialogueNode>().SelectSelf();
        //    }
        //}

        public void SetSelectedCharLink(GameObject charlink) {
            selectedCharLink = charlink;
        }

        //public void SetSelectedPlayerChoice(GameObject choice) {
        //    if (selectedChoice != null) {
        //        if (selectedChoice != choice) {
        //            selectedChoice.GetComponent<PlayerChoice>().DeselectSelf();
        //        }
        //    }
        //    if (selectedChoice != choice) {
        //        selectedChoice = choice;
        //        selectedChoice.GetComponent<PlayerChoice>().SelectSelf();
        //    }
        //}

        public void SetSelectedChoiceResultOption(GameObject option) {
            selectedChoiceResultOption = option;
        }

        //public void SetSelectedPchoiceResult(GameObject result) {
        //    selectedChoiceResult = result;
        //}

        //public GameObject GetSelectedPlayerChoice() {
        //    return selectedChoice;
        //}

        public void DisplayCharsRelatedToDialogue() {
            charDialoguesListUI.SetActive(true);
            ui.FillDisplayFromDb(GetCharDialogueDisplayQry(), characterList.transform, BuildCharacterDialogue);
        }

        public void DisplayNodesRelatedToDialogue() {
            dialogueNodesListUI.SetActive(true);
            ui.FillDisplayFromDb(GetDialogueNodeDisplayQry(), dialogueNodesList.transform, BuildDialogueNode);
        }

        public void DisplayChoicesRelatedToNode() {
            playerChoicesListUI.SetActive(true);
            ui.FillDisplayFromDb(GetPlayerChoiceDisplayQry(), playerChoicesList.transform, BuildPlayerChoice);
        }

        public void DisplayNewChoiceResultsNodes() {
            ui.FillDisplayFromDb(GetNewNodeChoiceResultQry(), selectedResultTypeList.transform, BuildNewChoiceResultNode);
            choiceResultOptions = ChoiceResultOptions.DialogueNodes;
        }

        public void DisplayResultsRelatedToChoices() {

            playerChoiceResultsListUI.SetActive(true);
            ui.EmptyDisplay(playerChoicesResultsList.transform);
            if (DbCommands.GetFieldValueFromTable("PlayerChoices", "NextNodes", " ChoiceIDs = " + (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID) != "") {
                print("DisplayChoiceResults");
                GameObject pChoiceResultsTitle = Instantiate(pChoiceResultTitlePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                AppendDisplayWithTitle(playerChoicesResultsList.transform, pChoiceResultsTitle.transform, "Goes to dialogue node... ");
                ui.AppendDisplayFromDb(GetNextNodeResultQry(), playerChoicesResultsList.transform, BuildChoiceResultNode);
            }
        }

        public void HideCharsRelatedToDialogue() {
            charDialoguesListUI.SetActive(false);
        }

        public void HideNodesRelatedToDialogue() {
            dialogueNodesListUI.SetActive(false);
        }

        public void HidePlayerChoices() {
            playerChoicesListUI.SetActive(false);
        }

        public void HidePlayerChoiceResults() {
            playerChoiceResultsListUI.SetActive(false);
        }

        public void SetActiveNodeEdit() {
            activateNodeDetailsBtn.GetComponent<Text>().text = "Edit node";
            editingNode = true;
            ActivateEditNewNode();
        }

        public void SetActivePlayerChoiceEdit() {
            displayPlayerChoiceDetailsBtn.GetComponent<Text>().text = "Edit choice";
            editingChoice = true;
            ActivateEditNewChoice();
        }

        private string GetDialogueDisplayQry() {
            return "SELECT * FROM Dialogues ORDER BY DialogueIDs ASC;";
        }

        private string GetCharLinkDisplayQry() {
            return "SELECT * FROM Characters WHERE CharacterNames != '!Player' ORDER BY CharacterNames ASC;";
        }

        private string GetCharDialogueDisplayQry() {
            return "SELECT * FROM CharacterDialogues WHERE DialogueIDs = "
                + (GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID + ";";
        }

        private string GetDialogueNodeDisplayQry() {
            return "SELECT * FROM DialogueNodes WHERE DialogueIDs = "
               + (GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID + ";";
        }

        private string GetPlayerChoiceDisplayQry() {
            return "SELECT * FROM PlayerChoices WHERE NodeIDs = "
               + (GetSelectedItemFromGroup(selectedNode) as DialogueNode).MyID + ";";
        }

        private string GetNewNodeChoiceResultQry() {
            return "SELECT * FROM DialogueNodes WHERE DialogueIDs = " + (GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID + ";";
        }

        private string GetNextNodeResultQry() {
            return "SELECT * FROM DialogueNodes WHERE NodeIDs = " + (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyNextNode + ";";
        }

        /// <summary>
        /// The characters from the game are updated so that you know which ones still exist in the scene and are given the option
        /// to delete them (from the Data UI -> Dialogues UI -> Character lists(s?)) when the list is built. New characters in the scene are also added.
        /// </summary>
        private void UpdateCharactersTableFromGame() {
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            string currentScene = sceneLoader.GetCurrentSceneName();
            InsertCharsNotInDbFromScene(currentScene);
            List<string[]> characterNamesList = new List<string[]>();
            DbCommands.GetDataStringsFromQry(DBqueries.GetCharacterNamesWithScene(currentScene), out characterNamesList, currentScene);
            UpdateCharsInDbNoLongerInScene(characterNamesList);
        }

        /// <summary>
        /// Inserts any character in the scene that is not yet in the characters table.
        /// </summary>
        private void InsertCharsNotInDbFromScene(string currentScene) {
            NPCs npcs = FindObjectOfType<NPCs>();
            foreach (string npcName in npcs.GetCharNamesList()) {
                DbCommands.InsertTupleToTable("Characters", npcName, currentScene);
            }
        }

        /// <summary>
        /// Check a list of npc names from the database against the npc names in the current scene so
        /// if the names are not there (meaning they have been removed from the scene) then the table 
        /// of characters is updated to remove the scene from the character table scene field and in 
        /// turn informs the user using the data dialogue UI.
        /// </summary>
        /// <param name="namesList">A list of character names</param>
        private void UpdateCharsInDbNoLongerInScene(List<string[]> namesList) {
            NPCs npcs = FindObjectOfType<NPCs>();
            foreach (string[] nameBox in namesList) {
                string npcName = nameBox[0];
                if (!npcs.IsNameInCharDict(npcName)) {
                    DbCommands.UpdateTableField(
                        "Characters",
                        "Scenes",
                        "null",
                        "CharacterNames = " + DbCommands.GetParameterNameFromValue(npcName),
                        DbCommands.GetParameterNameFromValue(npcName),
                        npcName
                        );
                }
            }
        }

        private void AppendDisplayWithTitle(Transform display, Transform titleTransform, string titleText) {
            titleTransform.GetComponentInChildren<Text>().text = titleText;
            titleTransform.SetParent(display, false);
        }
    }
}