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
        PlayerChoicesListUI playerChoicesListUI;
        DialogueNodesListUI dialogueNodesListUI;

        public string selectedDialogue,
                       selectedNode,
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
        GameObject dialogueNodesPanel, dialogueNodesScrollView, dialogueNodesList,
                   nodeDetailsUI, nodeDetailsUIPanel, activateNodeDetailsBtn,
                   selectNodeSpeakersUIpanel, charOverrideList;
        InputField inputNodeText;
        public GameObject dialogNodePrefab;

        //PLAYER CHOICE RESULTS
        GameObject playerChoiceResultsListUI, playerChoiceResultsPanel, playerChoicesResultsList,
                   newChoiceResultUI, displayNewChoiceResultBtn, newChoiceResultPanel, selectedResultTypeList;
        //ChoiceResultOptions choiceResultOptions;
        ListSearcher newChoiceResultListSearcher;
        ListDisplayInfo newActivateWelshVocabListInfo,
                        newActivateGrammarListInfo,
                        newActivateDialogueListInfo;
        ScrollRect choiceResultOptionsScrollView;
        public GameObject existingResultTitlePrefab,
                          existingNodeResultPrefab,
                          existingActivateQuestResultPrefab,
                          existingCompleteDialogueResultPrefab,
                          existingActivateTaskResultPrefab,
                          existingActivateVocabResultPrefab,
                          existingActivateGrammarResultPrefab,
                          existingActivateDialougueResultPrefab,
                          newNodeResultBtnPrefab,
                          newActivateQuestResultBtnPrefab,
                          newActivateTaskResultBtnPrefab,
                          newActivateWelshVocabResultsBtnPrefab,
                          newActivateGrammarResultBtnPrefab,
                          newActivateDialogueResultBtnPrefab;

        void Start() {
            playerChoicesListUI = FindObjectOfType<PlayerChoicesListUI>();
            dialogueNodesListUI = FindObjectOfType<DialogueNodesListUI>();
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
            newActivateDialogueListInfo = new ListDisplayInfo(
                DbQueries.GetNewActivateDialoguePlayerChoiceResultQry,
                BuildNewChoiceResultActivateDialogueBtn);

            //display dialogue list
            FillDisplayFromDb(DbQueries.GetDialogueDisplayQry(), dialogueList.transform, BuildDialogue);

            selectedDialogue = "selectedDialogue";
            selectedNode = "selectedNode";
            selectedChoiceResult = "selectedChoiceResult";
            CreateSelectionToggleGroup(selectedDialogue);
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



        public void InsertDialogueNodeResult(GameObject selectedBtn) {
            NewNodeChoiceResultBtn dialogueNodeChoiceResultBtn = selectedBtn.GetComponent<NewNodeChoiceResultBtn>();
            //(GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MyNextNode = dialogueNodeChoiceResultBtn.MyID;
            string[,] fieldVals = new string[,] { { "NextNodes", dialogueNodeChoiceResultBtn.MyID } };
            PlayerChoice selectedChoiceObj = playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice;
            selectedChoiceObj.MyNextNode = dialogueNodeChoiceResultBtn.MyID;
            DbCommands.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + selectedChoiceObj.MyID, fieldVals);
            DisplayResultsRelatedToChoices();
            DeactivateNewChoiceResult();
        }

        public void InsertCompleteDialogueResult() {
            //(GetSelectedItemFromGroup(selectedChoice) as PlayerChoice).MarkDialogueComplete = "1";
            string[,] fieldVals = new string[,] { { "MarkDialogueCompleted", "1" } };
            DbCommands.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID, fieldVals);
            DisplayResultsRelatedToChoices();
            DeactivateNewChoiceResult();
        }

        public void InsertActivateQuestResult(string questName) {
            string playerChoiceID = (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID;
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
            activateTaskChoiceResult.InitialiseMe(taskID, taskDescription, (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID, questName);
            return activateTaskChoiceResult.transform;
        }

        private Transform BuildNewChoiceResultActivateVocabBtn(string[] strArray) {
            string english = strArray[0];
            string welsh = strArray[1];
            NewActivateWelshVocabResultBtn activateVocabChoiceResult = (Instantiate(newActivateWelshVocabResultsBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewActivateWelshVocabResultBtn>();
            activateVocabChoiceResult.InitialiseMe(english, welsh, (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID);
            return activateVocabChoiceResult.transform;
        }

        private Transform BuildNewChoiceResultActivateGrammarBtn(string[] strArray) {
            string grammarID = strArray[0];
            string grammarSummary = strArray[1];
            string grammarDescription = strArray[2];
            NewActivateGrammarResultBtn activateGrammarResult = (Instantiate(newActivateGrammarResultBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewActivateGrammarResultBtn>();
            activateGrammarResult.InitialiseMe(grammarID, grammarSummary, grammarDescription, (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID);
            return activateGrammarResult.transform;
        }

        private Transform BuildNewChoiceResultActivateDialogueBtn(string[] strArray) {
            string dialogueID = strArray[0];
            string dialogueDescription = strArray[1];
            NewActivateDialogueResultBtn activateDialogueResult = (Instantiate(newActivateDialogueResultBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject).GetComponent<NewActivateDialogueResultBtn>();
            activateDialogueResult.InitialiseMe(dialogueID, dialogueDescription, (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID);
            return activateDialogueResult.transform;
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

        private Transform BuildExistingResultActivateDialogue(string[] strArray) {
            string resultID = strArray[0];
            string dialogueID = strArray[1];
            string dialogueDesc = strArray[2];
            ExistingActivateDialogueResult choiceResult = (Instantiate(existingActivateDialougueResultPrefab, new Vector3(0f, 0f), Quaternion.identity)).GetComponent<ExistingActivateDialogueResult>();
            choiceResult.InitialiseMe(resultID, dialogueID, dialogueDesc);
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

        public void DisplayNewChoiceResultsActivateDialogue() {
            FillDisplayFromDb(newActivateDialogueListInfo.GetMyDefaultQuery(), selectedResultTypeList.transform, newActivateDialogueListInfo.GetMyBuildMethod());
            newChoiceResultListSearcher.SetSearchInfo(newActivateDialogueListInfo);
        }

        public void DisplayResultsRelatedToChoices() {

            playerChoiceResultsListUI.SetActive(true);
            EmptyDisplay(playerChoicesResultsList.transform);
            string selectedChoiceID = (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID;
            if (DbCommands.GetFieldValueFromTable("PlayerChoices", "MarkDialogueCompleted", " ChoiceIDs = " + selectedChoiceID) != "0") {
                AppendDisplayFromDb((DbQueries.GetChoiceCompleteDialogueQry(selectedChoiceID)), playerChoicesResultsList.transform, BuildExistingResultCompleteDialogue);
            }
            if (DbCommands.GetFieldValueFromTable("PlayerChoices", "NextNodes", " ChoiceIDs = " + selectedChoiceID) != "") {
                GameObject pChoiceResultsTitle = Instantiate(existingResultTitlePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                AppendDisplayWithTitle(playerChoicesResultsList.transform, pChoiceResultsTitle.transform, "Goes to dialogue node... ");
                PlayerChoice currentPlayerChoice = playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice;
                AppendDisplayFromDb(DbQueries.GetNextNodeResultQry(currentPlayerChoice.MyNextNode), playerChoicesResultsList.transform, BuildExistingResultNode);
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

                int dialogueActivateCount = DbCommands.GetCountFromQry(DbQueries.GetDialogueActivateCountFromChoiceIDqry(selectedChoiceID));
                if (dialogueActivateCount > 0) {
                    GameObject existingResultsTitle = Instantiate(existingResultTitlePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                    AppendDisplayWithTitle(playerChoicesResultsList.transform, existingResultsTitle.transform, "Activates new dialogue(s)... ");
                    AppendDisplayFromDb(DbQueries.GetCurrentActivateDialoguePlayerChoiceResultQry(selectedChoiceID),
                            playerChoicesResultsList.transform,
                            BuildExistingResultActivateDialogue);
                }
            }
        }
            

        public void HideCharsRelatedToDialogue() {
            charDialoguesListUI.SetActive(false);
        }

        public void HideNodesRelatedToDialogue() {
            dialogueNodesListUI.HideComponents();
        }



        public void HidePlayerChoiceResults() {
            playerChoiceResultsListUI.SetActive(false);
            
        }



        public void DeleteNodePlayerChoice() {
            DbCommands.UpdateTableField(
                    "PlayerChoices",
                    "NextNodes",
                    "null",
                    "ChoiceIDs = " + (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID);
        }

        public void DeletePlayerChoiceResultGeneric(string resultID) { 
            string[,] fields = { { "ResultIDs", resultID},
                                 { "ChoiceIDs", (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID} };
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