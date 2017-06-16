using UnityEngine;
using UnityEngine.UI;
using DataUI.ListItems;
using DbUtilities;
using System.Collections.Generic;
using UnityUtilities;

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
        //ChoiceResultOptions choiceResultOptions;
        ListSearcher newChoiceResultListSearcher;
        ListDisplayInfo newActivateWelshVocabListInfo,
                        newActivateGrammarListInfo;
        ScrollRect choiceResultOptionsScrollView;
        public GameObject existingResultTitlePrefab,
                          existingNodeResultPrefab,
                          existingActivateQuestResultPrefab,
                          existingCompleteDialogueResultPrefab,
                          existingActivateTaskResultPrefab,
                          existingActivateVocabResultPrefab,
                          existingActivateGrammarResultPrefab,
                          newNodeResultBtnPrefab,
                          newActivateQuestResultBtnPrefab,
                          newActivateTaskResultBtnPrefab,
                          newActivateWelshVocabResultsBtnPrefab,
                          newActivateGrammarResultBtnPrefab;

        void Start() {
            
            UpdateCharactersTableFromGame();
            //choiceResultOptions = new ChoiceResultOptions();

            //DIALOGUE COMPONENTS
            dialoguesListUI = GetPanel().transform.Find("DialoguesListUI").gameObject;
            dialoguesPanel = dialoguesListUI.transform.Find("Panel").gameObject;
            dialogueList = dialoguesPanel.transform.Find("DialoguesList").gameObject; 
             //add
            submitNewDialogue = dialoguesPanel.transform.Find("SubmitNewDialog").gameObject;
            newDialoguePanel = submitNewDialogue.transform.Find("NewDialogPanel").gameObject;
            activateNewDialogueBtn = submitNewDialogue.transform.Find("ActivateNewDialogBtn").gameObject;
            inputShortDescriptionText = newDialoguePanel.transform.Find("InputShortDescriptionText").GetComponent<InputField>();
            dialogueActive = newDialoguePanel.transform.GetComponentInChildren<Toggle>();

            //CHARACTER DIALOGUES COMPONENTS
            charDialoguesListUI = GetPanel().transform.Find("CharDialoguesListUI").gameObject;
            charDialoguesPanel = charDialoguesListUI.transform.Find("CharDialoguesPanel").gameObject;
            characterList = charDialoguesPanel.transform.Find("CharDialoguesList").gameObject;
            //add
            addCharDialoguesUI = charDialoguesPanel.transform.Find("AddCharDialoguesUI").gameObject;
            addCharDialoguesPanel = addCharDialoguesUI.transform.Find("AddCharDialoguesPanel").gameObject;
            addCharDialogueList = addCharDialoguesPanel.transform.Find("CharacterList").gameObject;
            activateAddCharBtn = addCharDialoguesUI.transform.Find("ActivateAddCharBtn").gameObject;

            //NODE COMPONENTS
            dialogueNodesListUI = GetPanel().transform.Find("DialogueNodesListUI").gameObject;
            dialogueNodesPanel = dialogueNodesListUI.transform.Find("DialogueNodesPanel").gameObject;
            dialogueNodesScrollView = dialogueNodesListUI.transform.GetComponentInChildren<ScrollRect>().gameObject;
            dialogueNodesList = dialogueNodesScrollView.transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
            //add
            nodeDetailsUI = dialogueNodesPanel.transform.Find("NodeDetailsUI").gameObject;
            nodeDetailsUIPanel = nodeDetailsUI.transform.Find("Panel").gameObject;
            activateNodeDetailsBtn = nodeDetailsUI.transform.Find("ActivateNodeDetailsBtn").gameObject;
            inputNodeText = nodeDetailsUIPanel.transform.GetComponentInChildren<InputField>();
            endDialogueOptionToggle = nodeDetailsUIPanel.transform.GetComponentInChildren<Toggle>();
            selectNodeSpeakersUI = nodeDetailsUIPanel.transform.Find("SelectNodeSpeakersUI").gameObject;
            selectNodeSpeakersUIpanel = selectNodeSpeakersUI.transform.Find("Panel").gameObject;
            nodeSpeakerBtn = nodeDetailsUIPanel.transform.Find("NodeSpeakerBtn").gameObject;
            charOverrideList = selectNodeSpeakersUIpanel.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

            //PLAYER CHOICE COMPONENTS
            playerChoicesListUI = GetPanel().transform.Find("PlayerChoicesListUI").gameObject;
            playerChoicesPanel = playerChoicesListUI.transform.Find("Panel").gameObject;
            playerChoicesList = playerChoicesPanel.transform.Find("PlayerChoicesList").gameObject;
            //add
            playerChoiceDetails = playerChoicesPanel.transform.Find("PlayerChoiceDetails").gameObject;
            newPlayerChoicePanel = playerChoiceDetails.transform.Find("NewPlayerChoicePanel").gameObject;
            displayPlayerChoiceDetailsBtn = playerChoiceDetails.transform.Find("DisplayPlayerChoiceDetailsBtn").gameObject;
            inputChoiceText = newPlayerChoicePanel.transform.GetComponentInChildren<InputField>();

            //PLAYER CHOICE RESULTS COMPONENTS
            playerChoiceResultsListUI = GetPanel().transform.Find("PlayerChoiceResultsListUI").gameObject;
            playerChoiceResultsPanel = playerChoiceResultsListUI.transform.Find("Panel").gameObject;
            playerChoicesResultsList = playerChoiceResultsPanel.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
            //add
            newChoiceResultUI = playerChoiceResultsPanel.transform.Find("NewChoiceResultUI").gameObject;
            displayNewChoiceResultBtn = newChoiceResultUI.transform.Find("DisplayNewChoiceResultBtn").gameObject;
            newChoiceResultPanel = newChoiceResultUI.transform.Find("Panel").gameObject;
            choiceResultOptionsScrollView = newChoiceResultPanel.GetComponentInChildren<ScrollRect>();
            selectedResultTypeList = choiceResultOptionsScrollView.transform.Find("SelectedResultTypeList").gameObject;
            newChoiceResultListSearcher = newChoiceResultPanel.GetComponentInChildren<ListSearcher>();

            newActivateWelshVocabListInfo = new ListDisplayInfo(
                DbQueries.GetNewActivateVocabPlayerChoiceResultQry,
                BuildNewChoiceResultActivateVocabBtn);
            newActivateGrammarListInfo = new ListDisplayInfo(
                DbQueries.GetNewActivateGrammarPlayerChoiceResultQry,
                BuildNewChoiceResultActivateGrammarBtn);


            //display dialogue list
            FillDisplayFromDb(DbQueries.GetDialogueDisplayQry(), dialogueList.transform, BuildDialogue);

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
            FillDisplayFromDb(DbQueries.GetCharLinkDisplayQry(), addCharDialogueList.transform, BuildAddCharDialogueBtn);
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
            selectNodeSpeakersUI.transform.Find("Panel").gameObject.SetActive(true);
            nodeSpeakerBtn.GetComponent<Button>().interactable = false;
            string dialogueID = ((GetSelectedItemFromGroup(selectedDialogue)) as Dialogue).MyID;
            /*We only want to pick characters from scenes which are the same as those of the characters related to the dialogue
             * as a whole */
            string qry = "SELECT * FROM Characters "
                + "WHERE (Characters.Scenes IN (SELECT Scenes FROM CharacterDialogues "
                    + "WHERE DialogueIDs = " + dialogueID + ")) "
                + "OR (Characters.CharacterNames = '!Player') "
                + "ORDER BY CharacterNames ASC;";
            Debugging.PrintDbTable("CharacterDialogues");
            print(dialogueID);
            FillDisplayFromDb(qry, charOverrideList.transform, BuildDialogueNodeSpeaker);
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
            selectNodeSpeakersUI.transform.Find("Panel").gameObject.SetActive(false);
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
                string dialogueID = DbCommands.GenerateUniqueID("Dialogues", "DialogueIDs", "DialogueID");
                DbCommands.InsertTupleToTable("Dialogues", dialogueID, inputShortDescriptionText.text);
                if (dialogueActive.isOn) {
                    DbCommands.InsertTupleToTable("ActivatedDialogues", dialogueID, "0", "0"); //Puts the dialgoue in activated dialogues under the "New game" save ref as uncompleted.
                }
                FillDisplayFromDb(DbQueries.GetDialogueDisplayQry(), dialogueList.transform, BuildDialogue);
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
                    FillDisplayFromDb(DbQueries.GetDialogueNodeDisplayQry((GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID), 
                        dialogueNodesList.transform, 
                        BuildDialogueNode);
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
                                            null,
                                            "0");
                    FillDisplayFromDb(DbQueries.GetPlayerChoiceDisplayQry((GetSelectedItemFromGroup(selectedNode) as DialogueNode).MyID), 
                        playerChoicesList.transform, BuildPlayerChoice);
                }
            }

        }

        public void InsertDialogueNodeResult(GameObject selectedBtn) {
            NewNodeChoiceResultBtn dialogueNodeChoiceResultBtn = selectedBtn.GetComponent<NewNodeChoiceResultBtn>();
            //(GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyNextNode = dialogueNodeChoiceResultBtn.MyID;
            string[,] fieldVals = new string[,] { { "NextNodes", dialogueNodeChoiceResultBtn.MyID } };
            PlayerChoice selectedChoiceObj = GetSelectedItemFromGroup(selectedChoice) as PlayerChoice;
            selectedChoiceObj.MyNextNode = dialogueNodeChoiceResultBtn.MyID;
            DbCommands.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + selectedChoiceObj.MyID, fieldVals);
            DisplayResultsRelatedToChoices();
            DeactivateNewChoiceResult();
        }

        public void InsertCompleteDialogueResult() {
            //(GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MarkDialogueComplete = "1";
            string[,] fieldVals = new string[,] { { "MarkDialogueCompleted", "1" } };
            DbCommands.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID, fieldVals);
            DisplayResultsRelatedToChoices();
            DeactivateNewChoiceResult();
        }

        public void InsertActivateQuestResult(string questName) {
            string playerChoiceID = (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID;
            string newResultID = DbCommands.GenerateUniqueID("PlayerChoiceResults", "ResultIDs", "ResultID");
            DbCommands.InsertTupleToTable("PlayerChoiceResults", newResultID, playerChoiceID);
            DbCommands.InsertTupleToTable("QuestActivateResults", newResultID, playerChoiceID, questName);
            DisplayResultsRelatedToChoices();
            DeactivateNewChoiceResult();
        }

        private Transform BuildDialogue(string[] strArray) {
            string idStr = (strArray[0]);
            string descStr = (strArray[1]);
            print(strArray[2]);
            bool activeAtStartOfGameBool = false;
            if (strArray[2] != "") {
                activeAtStartOfGameBool = (int.Parse(strArray[2]) == -1) ? true : false;
            }
            GameObject dialogue = Instantiate(dialoguePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            dialogue.transform.Find("DescriptionInput").GetComponent<InputField>().text = descStr;
            dialogue.transform.Find("DialogueID").GetComponent<Text>().text = idStr;
            dialogue.transform.GetComponentInChildren<Toggle>().isOn = activeAtStartOfGameBool;
            dialogue.GetComponent<Dialogue>().MyID = idStr;
            dialogue.GetComponent<Dialogue>().MyDescription = descStr;
            dialogue.GetComponent<Dialogue>().Active = activeAtStartOfGameBool;
            return dialogue.transform;
        }

        private Transform BuildAddCharDialogueBtn(string[] strArray) {
            string charName = (strArray[0]);
            string sceneName = (strArray[1]);
            GameObject charLink = Instantiate(characterLinkPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            string sceneStr = " <i>in " + sceneName + "</i>";
            if (sceneName == "") {
                sceneStr = " <i>scene not found</i>";
                charLink.GetComponent<AddCharDialogueBtn>().DisplayDeletionOption();
            }
            charLink.transform.Find("CharacterName").GetComponent<Text>().text = charName + sceneStr;
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
            GameObject charDialogue = Instantiate(charDialoguePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            charDialogue.transform.Find("CharacterInput").GetComponent<InputField>().text = charName + sceneStr;
            charDialogue.GetComponent<CharacterDialogue>().CharacterName = charName;
            charDialogue.GetComponent<CharacterDialogue>().DialogueID = dialogueID;
            charDialogue.GetComponent<CharacterDialogue>().SceneName = sceneName;
            return charDialogue.transform;
        }

        private Transform BuildDialogueNode(string[] strArray) {
            string idStr = (strArray[0]);
            string nodeText = (strArray[1]);
            GameObject dialogNode = Instantiate(dialogNodePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            dialogNode.transform.GetComponentInChildren<InputField>().text = nodeText;
            dialogNode.transform.Find("NodeID").GetComponent<Text>().text = idStr;
            dialogNode.GetComponent<DialogueNode>().MyID = idStr;
            dialogNode.GetComponent<DialogueNode>().MyText = nodeText;
            return dialogNode.transform;
        }

        private Transform BuildDialogueNodeSpeaker(string[] strArray) {
            string charName = strArray[0];
            string sceneName = strArray[1];
            GameObject charOverride = Instantiate(charOverridePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
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
            GameObject playerChoice = Instantiate(playerChoicePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            playerChoice.transform.GetComponentInChildren<InputField>().text = choiceText;
            playerChoice.transform.Find("ChoiceID").GetComponent<Text>().text = idStr;
            playerChoice.GetComponent<PlayerChoice>().MyID = idStr;
            playerChoice.GetComponent<PlayerChoice>().MyText = choiceText;
            playerChoice.GetComponent<PlayerChoice>().MyNextNode = nextNode;
            return playerChoice.transform;
        }

        //for adding a new choice result, not to be confused with existing results
        private Transform BuildNewChoiceResultNodeBtn(string[] strArray) {
            string idStr = strArray[0];
            string nodeText = strArray[1];
            GameObject optionChoiceResult = Instantiate(newNodeResultBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            optionChoiceResult.transform.GetComponentInChildren<InputField>().text = nodeText;
            optionChoiceResult.transform.Find("NodeID").GetComponent<Text>().text = idStr;
            optionChoiceResult.GetComponent<NewNodeChoiceResultBtn>().MyID = idStr;
            optionChoiceResult.GetComponent<NewNodeChoiceResultBtn>().MyText = nodeText;
            return optionChoiceResult.transform;
        }

        //for adding a new  result, not to be confused with existing results
        private Transform BuildNewChoiceResultActivateQuestBtn(string[] strArray) {
            string questName = strArray[0];
            string questDescription = strArray[1];
            NewActivateQuestResultBtn activateQuestChoiceResult = (Instantiate(newActivateQuestResultBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewActivateQuestResultBtn>();
            activateQuestChoiceResult.InitialiseMe(questName, questDescription);
            return activateQuestChoiceResult.transform;
        }

        //for adding a new  result, not to be confused with existing results
        private Transform BuildNewChoiceResultActivateTaskBtn(string[] strArray) {
            string taskID = strArray[0];
            string taskDescription = strArray[1];
            string questName = strArray[2];
            NewActivateTaskResultBtn activateTaskChoiceResult = (Instantiate(newActivateTaskResultBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewActivateTaskResultBtn>();
            activateTaskChoiceResult.InitialiseMe(taskID, taskDescription, (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID, questName);
            return activateTaskChoiceResult.transform;
        }

        private Transform BuildNewChoiceResultActivateVocabBtn(string[] strArray) {
            string english = strArray[0];
            string welsh = strArray[1];
            NewActivateWelshVocabResultBtn activateVocabChoiceResult = (Instantiate(newActivateWelshVocabResultsBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewActivateWelshVocabResultBtn>();
            activateVocabChoiceResult.InitialiseMe(english, welsh, (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID);
            return activateVocabChoiceResult.transform;
        }

        private Transform BuildNewChoiceResultActivateGrammarBtn(string[] strArray) {
            string grammarID = strArray[0];
            string grammarSummary = strArray[1];
            string grammarDescription = strArray[2];
            NewActivateGrammarResultBtn activateGrammarResult = (Instantiate(newActivateGrammarResultBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewActivateGrammarResultBtn>();
            activateGrammarResult.InitialiseMe(grammarID, grammarSummary, grammarDescription, (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID);
            return activateGrammarResult.transform;
        }

        //for adding an existing choice result, not to be confused with a new result
        private Transform BuildExistingResultNode(string[] strArray) {
            string nodeIDStr = strArray[0];
            string nodeText = strArray[1];
            ExistingNodeResult choiceResult = (Instantiate(existingNodeResultPrefab, new Vector3(0f, 0f), Quaternion.identity) as GameObject).GetComponent<ExistingNodeResult>();
            choiceResult.SetMyID("-1");//since the next node of a choice is not logged in the results table it is given -1 in case this needs to be checked somewhere.
            choiceResult.InitialiseMe(nodeIDStr,nodeText);
            return choiceResult.transform;
        }

        //for adding an existing choice result, not to be confused with a new result
        private Transform BuildExistingResultCompleteDialogue(string[] strArray) {
            string choiceIDstr = strArray[0];
            print(choiceIDstr);
            GameObject choiceResult = Instantiate(existingCompleteDialogueResultPrefab, new Vector3(0f, 0f), Quaternion.identity) as GameObject;
            choiceResult.GetComponent<ExistingResult>().MyID = "-1";//since the next node of a choice is not logged in the results table it is given -1 in case this needs to be checked somewhere.
            choiceResult.GetComponent<ExistingCompleteDialogueResult>().MyChoiceID = choiceIDstr;
            return choiceResult.transform;
        }

        private Transform BuildExistingResultActivateQuest(string[] strArray) {
            string resultID = strArray[0];
            string questName = strArray[1];
            string questDescription = strArray[2];
            GameObject choiceResult = Instantiate(existingActivateQuestResultPrefab, new Vector3(0f, 0f), Quaternion.identity) as GameObject;
            choiceResult.transform.Find("QuestNameLbl").GetComponent<Text>().text = questName;
            choiceResult.transform.Find("QuestDescriptionLbl").GetComponent<Text>().text = questDescription;
            choiceResult.GetComponent<ExistingResult>().MyID = resultID;
            choiceResult.GetComponent<ExistingActivateQuestResult>().MyName = questName;
            choiceResult.GetComponent<ExistingActivateQuestResult>().MyDescription = questDescription;
            return choiceResult.transform;
        }

        private Transform BuildExistingResultActivateTask(string[] strArray) {
            string resultID = strArray[0];
            string taskID = strArray[1];
            string taskDesc = strArray[2];
            string questName = strArray[3];
            ExistingActivateTaskResult choiceResult = (Instantiate(existingActivateTaskResultPrefab, new Vector3(0f, 0f), Quaternion.identity)).GetComponent<ExistingActivateTaskResult>();
            choiceResult.InitialiseMe(taskID, taskDesc, questName);
            choiceResult.SetMyID(resultID);
            return choiceResult.transform;
        }

        private Transform BuildExistingResultActivateVocab(string[] strArray) {
            string resultID = strArray[0];
            string englishTxt = strArray[2];
            string welshTxt = strArray[3];
            ExistingActivateWelshVocab choiceResult = (Instantiate(existingActivateVocabResultPrefab, new Vector3(0f, 0f), Quaternion.identity)).GetComponent<ExistingActivateWelshVocab>();
            choiceResult.InitialiseMe(resultID, englishTxt, welshTxt);
            return choiceResult.transform;
        }

        private Transform BuildExistingResultActivateGrammar(string[] strArray) {
            string resultID = strArray[0];
            string grammarID = strArray[1];
            string grammarSummary = strArray[2];
            string grammarDesc = strArray[3];
            ExistingActivateGrammarResult choiceResult = (Instantiate(existingActivateGrammarResultPrefab, new Vector3(0f, 0f), Quaternion.identity)).GetComponent<ExistingActivateGrammarResult>();
            choiceResult.InitialiseMe(resultID, grammarID, grammarSummary, grammarDesc);
            return choiceResult.transform;
        }

        public void SetSelectedCharLink(GameObject charlink) {
            selectedCharLink = charlink;
        }

        public void DisplayCharsRelatedToDialogue() {
            charDialoguesListUI.SetActive(true);
            FillDisplayFromDb(DbQueries.GetCharDialogueDisplayQry((GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID),
                characterList.transform, BuildCharacterDialogue);
        }

        public void DisplayNodesRelatedToDialogue() {
            dialogueNodesListUI.SetActive(true);
            FillDisplayFromDb(DbQueries.GetDialogueNodeDisplayQry((GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID), dialogueNodesList.transform, BuildDialogueNode);
        }

        public void DisplayChoicesRelatedToNode() {
            playerChoicesListUI.SetActive(true);
            FillDisplayFromDb(DbQueries.GetPlayerChoiceDisplayQry((GetSelectedItemFromGroup(selectedNode) as DialogueNode).MyID), playerChoicesList.transform, BuildPlayerChoice);
            editingChoice = false;
            DeactivateNewPlayerChoice();
        }

        public void DisplayNewChoiceResultsNodes() {
            FillDisplayFromDb(DbQueries.GetNewNodeChoiceResultQry((GetSelectedItemFromGroup(selectedDialogue) as Dialogue).MyID), selectedResultTypeList.transform, BuildNewChoiceResultNodeBtn);
            newChoiceResultListSearcher.DeactivateSelf();
        }

        public void DisplayNewChoiceResultsQuests() {
            FillDisplayFromDb(DbQueries.GetNewQuestChoiceResultQry(), selectedResultTypeList.transform, BuildNewChoiceResultActivateQuestBtn);
            newChoiceResultListSearcher.DeactivateSelf();
        }

        public void DisplayNewChoiceResultsActivateTask() {
            FillDisplayFromDb(DbQueries.GetNewActivateTaskPlayerChoiceResultQry(), selectedResultTypeList.transform, BuildNewChoiceResultActivateTaskBtn);
            newChoiceResultListSearcher.DeactivateSelf();
        }

        public void DisplayNewChoiceResultsActivateVocab() {
            FillDisplayFromDb(DbQueries.GetNewActivateVocabPlayerChoiceResultQry(), selectedResultTypeList.transform, BuildNewChoiceResultActivateVocabBtn);
            newChoiceResultListSearcher.SetSearchInfo(newActivateWelshVocabListInfo);
        }

        public void DisplayNewChoiceResultsActivateGrammar() {
            FillDisplayFromDb(newActivateGrammarListInfo.GetMyDefaultQuery(), selectedResultTypeList.transform, newActivateGrammarListInfo.GetMyBuildMethod());
            newChoiceResultListSearcher.SetSearchInfo(newActivateGrammarListInfo);

        }

        public void DisplayResultsRelatedToChoices() {

            playerChoiceResultsListUI.SetActive(true);
            EmptyDisplay(playerChoicesResultsList.transform);
            string selectedChoiceID = (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID;
            print("working1");
            if (DbCommands.GetFieldValueFromTable("PlayerChoices", "MarkDialogueCompleted", " ChoiceIDs = " + selectedChoiceID) != "0") {
                AppendDisplayFromDb((DbQueries.GetChoiceCompleteDialogueQry(selectedChoiceID)), playerChoicesResultsList.transform, BuildExistingResultCompleteDialogue);
            }
            print("working2");
            if (DbCommands.GetFieldValueFromTable("PlayerChoices", "NextNodes", " ChoiceIDs = " + selectedChoiceID) != "") {
                print("working3");
                GameObject pChoiceResultsTitle = Instantiate(existingResultTitlePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                AppendDisplayWithTitle(playerChoicesResultsList.transform, pChoiceResultsTitle.transform, "Goes to dialogue node... ");
                print("working4");
                AppendDisplayFromDb(DbQueries.GetNextNodeResultQry((GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyNextNode), playerChoicesResultsList.transform, BuildExistingResultNode);
                print("working5");
            }
            int resultsCount = DbCommands.GetCountFromTable("PlayerChoiceResults", "ChoiceIDs = " + selectedChoiceID);
            if (resultsCount > 0) {
                int questActivateCount = DbCommands.GetCountFromQry(DbQueries.GetQuestActivateCountFromChoiceIDqry(selectedChoiceID));
                if (questActivateCount > 0) {
                    GameObject existingResultsTitle = Instantiate(existingResultTitlePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                    AppendDisplayWithTitle(playerChoicesResultsList.transform, existingResultsTitle.transform, "Activates quest... ");
                    AppendDisplayFromDb(DbQueries.GetCurrentActivateQuestsPlayerChoiceResultQry(selectedChoiceID),
                            playerChoicesResultsList.transform,
                            BuildExistingResultActivateQuest);
                }

                int taskActivateCount = DbCommands.GetCountFromQry(DbQueries.GetTaskActivateCountFromChoiceIDqry(selectedChoiceID));
                if (taskActivateCount > 0) {
                    GameObject existingResultsTitle = Instantiate(existingResultTitlePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                    AppendDisplayWithTitle(playerChoicesResultsList.transform, existingResultsTitle.transform, "Activates tasks... ");
                    AppendDisplayFromDb(DbQueries.GetCurrentActivateTasksPlayerChoiceResultQry(selectedChoiceID),
                            playerChoicesResultsList.transform,
                            BuildExistingResultActivateTask);

                }

                int grammarActivateCount = DbCommands.GetCountFromQry(DbQueries.GetGrammarActivateCountFromChoiceIDqry(selectedChoiceID));
                if (grammarActivateCount > 0) {
                    GameObject existingResultsTitle = Instantiate(existingResultTitlePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                    AppendDisplayWithTitle(playerChoicesResultsList.transform, existingResultsTitle.transform, "Activates new grammar... ");
                    AppendDisplayFromDb(DbQueries.GetCurrentActivateGrammarPlayerChoiceResultQry(selectedChoiceID),
                            playerChoicesResultsList.transform,
                            BuildExistingResultActivateGrammar);
                }

                int vocabActivateCount = DbCommands.GetCountFromQry(DbQueries.GetVocabActivateCountFromChoiceIDqry(selectedChoiceID));
                if (vocabActivateCount > 0) {
                    GameObject existingResultsTitle = Instantiate(existingResultTitlePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                    AppendDisplayWithTitle(playerChoicesResultsList.transform, existingResultsTitle.transform, "Activates new vocab... ");
                    AppendDisplayFromDb(DbQueries.GetCurrentActivateVocabPlayerChoiceResultQry(selectedChoiceID),
                            playerChoicesResultsList.transform,
                            BuildExistingResultActivateVocab);
                }
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

        public void DeleteNodePlayerChoice() {
            DbCommands.UpdateTableField(
                    "PlayerChoices",
                    "NextNodes",
                    "null",
                    "ChoiceIDs = " + (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID);
        }

        public void DeletePlayerChoiceResultGeneric(string resultID) { 
            string[,] fields = { { "ResultIDs", resultID},
                                 { "ChoiceIDs", (GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyID} };
            DbCommands.DeleteTupleInTable(
                    "PlayerChoiceResults",
                    fields);
        }

        public void DeleteCompleteDialogueResult (string choiceID) {
            DbCommands.UpdateTableField(
                    "PlayerChoices",
                    "NextNodes",
                    "0",
                    "ChoiceIDs = " + choiceID);
        }

        /// <summary>
        /// The characters from the game are updated so that you know which ones still exist in the scene and are given the option
        /// to delete them (from the Data UI -> Dialogues UI -> Character lists(s?)) when the list is built. New characters in the scene are also added.
        /// </summary>
        private void UpdateCharactersTableFromGame() {
            SceneLoader sceneLoader = new SceneLoader();
            string currentScene = sceneLoader.GetCurrentSceneName();
            InsertCharsNotInDbFromScene(currentScene);
            List<string[]> characterNamesList = new List<string[]>();
            DbCommands.GetDataStringsFromQry(DbQueries.GetCharacterNamesWithScene(currentScene), out characterNamesList, currentScene);
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
                string npcParam;
                if (npcName == "") {
                    npcParam = "''";
                } else {
                    npcParam = DbCommands.GetParameterNameFromValue(npcName);
                }
                if (!npcs.IsNameInCharDict(npcName)) {
                    DbCommands.UpdateTableField(
                        "Characters",
                        "Scenes",
                        "null",
                        "CharacterNames = " + npcParam,
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