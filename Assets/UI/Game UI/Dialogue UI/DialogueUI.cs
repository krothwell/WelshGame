using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GameUI.ListItems;
using UIUtilities;
using DbUtilities;
using UnityUtilities;
using System;

/// <summary>
/// When certain in-game interactions take place, this class is responsible for 
/// communicating with game objects and the database to determine the display of
/// pre-stored dialogue, quest input (e.g. a quest may ask you to introduce 
/// yourself to 5 different npcs – when you start a dialogue with a new npc, the
/// quest action will be included in the dialogue options) and combat input 
/// (when a combat ability is used it may test the player asking him to 
/// translate something in Welsh. It also provides side effects including 
/// character portrait changing depending on speaker, and break down of player 
/// input results (% of answer that the player input that is correct).
/// </summary>
namespace GameUI {
    public class DialogueUI : UIController {
        private Button percentageBtn;
        private PlayerInputField currentInputField;
        private DialogueTestDataController currentDialogueTestData;
    	private string testEnglish;
    	private string testWelsh;
    	public string TestEnglish {
    		get {return testEnglish;}
    		set {testEnglish = value;}
    	}
    	public string TestWelsh {
    		get {return testWelsh;}
    		set {testWelsh = value;}
    	}
        private PlayerChoice currentPlayerChoice;
        public PlayerChoice CurrentPlayerChoice {
            get { return currentPlayerChoice; }
            set { currentPlayerChoice = value; }
        }

        private bool isInUse;
        public bool IsInUse {
            get { return animator.GetBool("InUse"); }
        }

        List<String> dialogueQueue;
        //CharAbility queuedAbility;
    	Text percentageTxt, btnTxt;
        GameObject submitBtn, dialogueReason;
        CombatUI combatUI;
        Animator animator;
        QuestsUI questsUI;
        NewWelshLearnedUI newWelshLearnedUI;
        Image objPortrait, reasonImg;
        protected RectTransform contentPanel;

        public GameObject 
            DialogueHolderPrefab,
            CharacterSpeakingPrefab, 
            InGameDialogueNodePrefab, 
            InGamePlayerChoicePrefab, 
            EndDialogueBtnPrefab, 
            SpacerPrefab, 
            EmptyBlockPrefab,
            VocabTestNodePrefab,
            PlayerInputFieldPrefab,
            GrammarShortDescPrefab;
        private ScrollRect dialogueScroller;
        private GameObject currentDialogueHolder, currentCharSpeaking;
        private string currentCharID, currentDialogueID, currentNodeID;
        private PlayerCharacter playerCharacter;
        public Character currentChar;
        private Sprite currentPortrait;
        public Sprite DialogueIcon;
        private NPCs npcs;
        SkillsMenuUI skillsMenuUI;
        void Awake() {
            animator = GetComponent<Animator>();
            dialogueScroller = transform.GetComponentInChildren<ScrollRect>();
        }
        // Use this for initialization
        void Start() {
            skillsMenuUI = FindObjectOfType<SkillsMenuUI>();
            dialogueQueue = new List<String>();
            questsUI = FindObjectOfType<QuestsUI>();
            newWelshLearnedUI = FindObjectOfType<NewWelshLearnedUI>();
            npcs = FindObjectOfType<NPCs>();
            playerCharacter = FindObjectOfType<PlayerCharacter>();
            panel = transform.Find("Panel").gameObject;
            submitBtn = panel.transform.Find("SubmitBtn").gameObject;
            dialogueReason = panel.transform.Find("DialogueReason").gameObject;
            percentageTxt = dialogueReason.transform.Find("TestResult").GetComponent<Text>();
            percentageBtn = percentageTxt.GetComponent<Button>();
            reasonImg = dialogueReason.transform.Find("Symbol").GetComponent<Image>();
            combatUI = FindObjectOfType<CombatUI>();
            btnTxt = submitBtn.transform.Find("Text").gameObject.GetComponent<Text>();
            playerCharacter = FindObjectOfType<PlayerCharacter>();
            objPortrait = panel.transform.Find("CharacterPortrait").GetComponent<Image>();
            DisableSubmitBtn();
        }

        public bool IsReadyForNewDialogue() {
            bool ready;
            //print(animator);
            if (animator.GetBool("InUse")) {
                ready = false;
            } else {
                ready = true;
            }
            return ready;
        }

        void QueueNewDialogue(string dialogueID_) {
            if (!dialogueQueue.Contains(dialogueID_)) {
                dialogueQueue.Add(dialogueID_);
            }
        }


        public void StartNewDialogue(Character character) {
            print("START DIALOGUE");
            if (IsReadyForNewDialogue()) {
                currentChar = character;
                print("starting dialogue with: " + character);
                currentCharID = character.CharacterName;
                SetDialogueID(character);
                print("working1");
                DisplayFirstDialogueNode();
                print("working2");
            } else {
                string queueID = GetDialogueID(character);
                QueueNewDialogue(queueID);
            }
        }

        public void StartNewDialogue(string dialogueID) {
            //print("ready for new dialogue = " + IsReadyForNewDialogue());
            if (IsReadyForNewDialogue()) {
                currentCharID = DbCommands.GetFieldValueFromTable("CharacterDialogues", "CharacterNames", "DialogueIDs = " + dialogueID);
                //print("DIALOGUE ID = " + dialogueID);
                currentChar = npcs.GetCharacterFromName(currentCharID);
                currentDialogueID = dialogueID;
                DisplayFirstDialogueNode();
            }
            else {
                QueueNewDialogue(dialogueID);
            }
        }

        private void StartNewDialogueFromQueue() {
            //print("Dialogue queue count: " + dialogueQueue.Count);
            if (dialogueQueue.Count > 0) {
                string newCharForDialogue = dialogueQueue[0];
                dialogueQueue.RemoveAt(0);
                StartNewDialogue(newCharForDialogue);
                print("Dialogue queue count: " + dialogueQueue.Count);
                
            }
        }



        public void ResetLowerDialogueContainer() {
            foreach (Transform dialogue in dialogueScroller.gameObject.transform) {
                Destroy(dialogue.gameObject);
            }
            currentDialogueHolder = null;
        }

        private void DisplayFirstDialogueNode() {
            if (!IsDialogueComplete()) {
                SetNewDialogueHolder();
                SetInUse();
                dialogueScroller.GetComponent<ScrollRect>().content = currentDialogueHolder.GetComponent<RectTransform>();
                string[] nodeArray = DbCommands.GetTupleFromTable("DialogueNodes", "DialogueIDs = " + currentDialogueID, "NodeIDs ASC");
                DisplayDialogueNode(nodeArray);
            }
            else {
                SetNotInUse();
            }
        }

        private void SetNewDialogueHolder() {
            ResetLowerDialogueContainer();
            currentDialogueHolder = Instantiate(DialogueHolderPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            currentDialogueHolder.transform.SetParent(dialogueScroller.gameObject.transform, false);
        }

        private bool IsDialogueComplete() {
            string dialogueComplete = DbCommands.GetFieldValueFromTable("ActivatedDialogues", "Completed", "DialogueIDs = " + currentDialogueID + " AND SaveIDs = 0");
            //no character dialogue so return true
            if(dialogueComplete == "") {
                return true;
            } else {
                return Convert.ToBoolean(int.Parse(dialogueComplete));
            }
        }

        private string GetSpeakersName(string nodeID) {
            string overrideName = DbCommands.GetFieldValueFromTable("DialogueNodes", "CharacterSpeaking", "NodeIDs = " + nodeID);
            if (overrideName == "") {
                return currentChar.CharacterName;
            }
            else if (overrideName == "!Player") {
                return playerCharacter.GetMyName();
            }
            else {
                return overrideName;
            }
        }

        public void DisplayDialogueNode(string[] nodeArray_) {
            currentNodeID = nodeArray_[0];
            string charName = GetSpeakersName(currentNodeID);
            InsertCharName(charName);
            //check if node character override is there and use the override name if so.
            GameObject speakerScrollObj = currentCharSpeaking.gameObject;
            SetCurrentPortraitFromName(charName);
            bool isDialogueNodeVocabTest = (DbCommands.GetCountFromTable("DialogueNodesVocabTests", "NodeIDs = " + currentNodeID) != 0);
            if (isDialogueNodeVocabTest) {
                string[] nodeVocabTestArray = DbCommands.GetTupleFromTable("DialogueNodesVocabTests", "NodeIDs = " + currentNodeID);
                string[] vocabArray = new string[2];
                vocabArray[0] = nodeVocabTestArray[1];
                vocabArray[1] = nodeVocabTestArray[2];
                ProcessDialogueNodeTest(vocabArray);
            }
            else {
                InsertDialogueNode(nodeArray_);
                DisplayNodeChoices(currentNodeID);
            }
            ScrollToDialogueElement(speakerScrollObj);
        }

        public void SetCurrentPortraitFromName(string charName) {
            if (charName != playerCharacter.GetMyName()) {
                Character charOfName = npcs.GetCharacterFromName(charName);
                currentPortrait = charOfName.GetMyPortrait();
                SetObjectPortrait(currentPortrait);
            }
            else {
                SetObjectPortrait(playerCharacter.GetPlayerPortrait());
                currentPortrait = playerCharacter.GetPlayerPortrait();
            }

        }

        public void SetCurrentPortrait() {
            SetObjectPortrait(currentPortrait);
        }

        public void SetObjectPortrait(Sprite portrait) {
            objPortrait.sprite = portrait;
        }

        private void DisplayNodeChoices(string nodeID) {
            InsertSpacer();
            //check if player is node character override, if not then the name can be inserted
            if (GetSpeakersName(nodeID) != playerCharacter.GetMyName()) {
                InsertCharName(playerCharacter.GetMyName());
            }
            string nodeChoiceQry = "SELECT * FROM PlayerChoices WHERE NodeIDs = " + nodeID + ";";
            AppendDisplayFromDb(nodeChoiceQry, currentDialogueHolder.transform, BuildPlayerChoice);
            string displayEndDialogueIndicator = (DbCommands.GetFieldValueFromTable("DialogueNodes", "EndDialogueOption", "DialogueIDs = " + nodeID));
            int choicesInt = DbCommands.GetCountFromTable("PlayerChoices", "NodeIDs = " + nodeID);
            if (displayEndDialogueIndicator == "1" || choicesInt == 0) {
                InsertEndDialogue();
            }
        }

        private Transform BuildPlayerChoice(string[] strArray) {
            string idStr = strArray[0];
            string choiceText = strArray[1];
            string nextNode = strArray[3];
            PlayerChoice choice = (Instantiate(InGamePlayerChoicePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<PlayerChoice>();
            choice.InitialiseMe(idStr, choiceText, nextNode);
            return choice.transform;
        }

        public void DestroyInteractiveChoices() {
            foreach (Transform component in currentDialogueHolder.transform) {
                Button buttonCheck = component.GetComponent<Button>();
                if (buttonCheck != null) {
                    if (buttonCheck.interactable) {
                        Destroy(component.gameObject);
                    }
                }
            }
        }

        public void InsertSpacer() {
            GameObject newSpacer = Instantiate(SpacerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            newSpacer.transform.SetParent(currentDialogueHolder.transform, false);
        }

        public void InsertCharName(string characterName_) {
            GameObject charSpeaking = Instantiate(CharacterSpeakingPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            charSpeaking.GetComponent<Text>().text = "<b>" + characterName_ + "</b>:";
            charSpeaking.transform.SetParent(currentDialogueHolder.transform, false);
            currentCharSpeaking = charSpeaking;
        }

        public void InsertEndDialogue() {
            GameObject endDialogue = Instantiate(EndDialogueBtnPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            endDialogue.transform.SetParent(currentDialogueHolder.transform, false);
            endDialogue.GetComponent<Text>().text = "\t<i>End Dialogue</i>";
        }

        public void InsertDialogueNode(string[] nodeArray) {
            DialogueNode dialogueNode = (Instantiate(
                InGameDialogueNodePrefab,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity
            ) as GameObject).GetComponent<DialogueNode>();
            dialogueNode.InitialiseMe(nodeArray[0], nodeArray[1]);
            dialogueNode.transform.SetParent(currentDialogueHolder.transform, false);
        }

        public void InsertVocabTestNode(string[] detailsArray) {
            VocabTestNode node = (Instantiate(
                VocabTestNodePrefab,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity) as GameObject).GetComponent<VocabTestNode>();
            node.InitialiseMe(detailsArray[0], detailsArray[1]);
            node.transform.SetParent(currentDialogueHolder.transform, false);
        }

        public void InsertPlayerInputField() {
            PlayerInputField input = (Instantiate(
                PlayerInputFieldPrefab,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity) as GameObject).GetComponent<PlayerInputField>();
            input.transform.SetParent(currentDialogueHolder.transform, false);
            currentInputField = input;
        }

        public void InsertRelatedGrammarTag() {
            string[] nodeArray = new string[2];
            nodeArray[0] = "-1";
            nodeArray[1] = "<i>Related grammar: </i>";
            InsertDialogueNode(nodeArray);
        }

        public void InsertGrammarShortDesc(string grammarstr) {
            GameObject grammarShortDesc = (Instantiate(
                GrammarShortDescPrefab,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity) as GameObject);
            grammarShortDesc.GetComponent<Text>().text = "\t- " + grammarstr;
            grammarShortDesc.transform.SetParent(currentDialogueHolder.transform, false);
        }


        private void SetDialogueID(Character character) {
            currentDialogueID = GetDialogueID(character);
            currentDialogueID = (currentDialogueID == "") ? "-1" : currentDialogueID;

            
        }

        private string GetDialogueID(Character character) {
            string qry = "SELECT CharacterDialogues.DialogueIDs FROM CharacterDialogues "
                        + "INNER JOIN ActivatedDialogues ON CharacterDialogues.DialogueIDs = ActivatedDialogues.DialogueIDs "
                        + "WHERE CharacterDialogues.CharacterNames = " + DbCommands.GetParameterNameFromValue(character.CharacterName)
                            + " AND ActivatedDialogues.Completed = 0"
                            + " AND ActivatedDialogues.SaveIDs = 0";
            string dialogueID = DbCommands.GetFieldValueFromQry(qry, "DialogueIDs", character.CharacterName);
            Debugging.PrintDbQryResults(qry, character.CharacterName);
            //Debugging.PrintDbTable("ActivatedDialogues");
            return dialogueID;
        }

        public void ScrollToDialogueElement(GameObject element) {
            UICommands.SnapScrollToContentChild(element, dialogueScroller);
        }

        public GameObject GetCurrentSpeaker() {
            return currentCharSpeaking;
        }

        public void SetScoreBreakdownDisplay() {
            percentageBtn.interactable = true;
            SetPercentageCorrect();
        }

        private void DisablePercentageCorrect() {
            percentageTxt.text = "";
            percentageBtn.interactable = false;
        }

        private void SetPercentageCorrect() {
            percentageTxt.text = currentDialogueTestData.GetAnswerPercentageCorrect().ToString() + "%";
        }

        public void SetInUse() {
            animator.SetBool("InUse", true);
            percentageTxt.text = "";
            combatUI.HideAbilities();
            combatUI.HideUnderAttack();
        }


        public void SetNotInUse() {
            DisableSubmitBtn();
            animator.SetBool("InUse", false);
            //playerCharacter.EndSelection();
            //playerCharacter.playerStatus = PlayerCharacter.PlayerStatus.passive;
            if (dialogueQueue.Count > 0) {
                Invoke("StartNewDialogueFromQueue", 1f);
            }
        }
        
        private void SetSubmitBtnText(string newText) {
            btnTxt.text = newText;
        }

        private void EnableSubmitBtn() {
            submitBtn.GetComponent<Button>().interactable = true;
        }

        private void DisableSubmitBtn() {
            SetSubmitBtnText("...");
            submitBtn.GetComponent<Button>().interactable = false;
        }

        public void SubmitAnswer() {
            if(currentInputField != null) {
                if (currentInputField.Submitted) {
                    DisablePercentageCorrect();
                    switch (currentDialogueTestData.MyTestTrigger.TrigType) {
                        case TestTrigger.TriggerType.Ability:
                            SetNotInUse();
                            combatUI.UseSelectedAbility(currentDialogueTestData);
                            combatUI.DisplayUnderAttack();
                            break;
                        case TestTrigger.TriggerType.DialogueNode:
                            DisableSubmitBtn();
                            DisplayNodeChoices(currentNodeID);
                            ScrollToDialogueElement(currentCharSpeaking);
                            break;
                        case TestTrigger.TriggerType.DialogueChoice:
                            DisableSubmitBtn();
                            if (currentPlayerChoice.MyNextNode != "") {
                                print(currentPlayerChoice.MyNextNode);
                                DisplayDialogueNode(currentPlayerChoice.GetDialogueNodeData(currentPlayerChoice.MyNextNode));
                            } else {
                                InsertEndDialogue();
                                ScrollToDialogueElement(currentCharSpeaking);
                            }
                            break;
                    }
                    
                }
                else {
                    currentDialogueTestData.SetResultsData(currentInputField.GetPlayerInputString());
                    SetScoreBreakdownDisplay();
                    SetSubmitBtnText("Continue...");
                    currentInputField.SetSubmitted();
                    skillsMenuUI.IncrementTotalSkillPoints(currentDialogueTestData.GetSkillPointsGainedTotal());
                }
            } 
        }
    


        public void MarkDialogueComplete(string choiceID) {
            bool isDialogueComplete = Convert.ToBoolean(DbCommands.GetCountFromTable("PlayerChoices", "ChoiceIDs = " + choiceID + " AND MarkDialogueCompleted = 1"));
            if (isDialogueComplete) {
                int activatedDialogueCount = DbCommands.GetCountFromTable("ActivatedDialogues", "SaveIDs = 0 AND Completed = 0 AND DialogueIDs = " + currentDialogueID);
                if (activatedDialogueCount > 0) {
                    DbCommands.UpdateTableField("ActivatedDialogues", "Completed", "1", "SaveIDs = 0 AND DialogueIDs = " + currentDialogueID);
                }
            }
        }

        public void ActivateQuests(string choiceID) {
            int countQuestActivateResults = DbCommands.GetCountFromQry(DbQueries.GetQuestActivateCountFromChoiceIDqry(choiceID));
            print(countQuestActivateResults);
            if (countQuestActivateResults > 0) {
                List<string[]> questsActivatedList;
                DbCommands.GetDataStringsFromQry(DbQueries.GetCurrentActivateQuestsPlayerChoiceResultQry(choiceID), out questsActivatedList);
                print(questsActivatedList.Count);
                foreach (string[] activatedQuest in questsActivatedList) {
                    questsUI.InsertActivatedQuest(activatedQuest[1]);
                }
            }
        }

        public void ActivateQuestTasks(string choiceID) {
            QuestsController questsController = FindObjectOfType<QuestsController>();
            int countTaskActivateResults = DbCommands.GetCountFromQry(DbQueries.GetTaskActivateCountFromChoiceIDqry(choiceID));
            if (countTaskActivateResults > 0) {
                print("Task ACTIVATING!!!!");
                List<string[]> tasksActivatedList;
                DbCommands.GetDataStringsFromQry(DbQueries.GetCurrentActivateTasksPlayerChoiceResultQry(choiceID), out tasksActivatedList);
                foreach (string[] activatedTask in tasksActivatedList) {
                    questsUI.InsertActivatedTask(activatedTask[1], activatedTask[3], activatedTask[2]);
                    //a list of task parts in task that are prefab types are iterated over (if any) and instantiated.
                    List<string[]> prefabParts = new List<string[]>();
                    DbCommands.GetDataStringsFromQry(DbQueries.GetPrefabTaskPartsFromTaskIDqry(activatedTask[1]), out prefabParts);
                    foreach(string[] prefabPart in prefabParts) {
                        string prefabPath = prefabPart[0];
                        string partID = prefabPart[1];
                        UnityEngine.Object prefabTaskPartObj = Resources.Load(prefabPath);
                        print(prefabPath);
                        GameObject prefabTaskPart = Instantiate(prefabTaskPartObj, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                        prefabTaskPart.GetComponent<QuestTaskPart>().InitialiseMe(partID, activatedTask[1], activatedTask[3]);
                        prefabTaskPart.transform.SetParent(questsController.transform, false);
                    }
                }
            }
        }

        public void ActivateNewWelsh(string choiceID) {
            if (newWelshLearnedUI.transform.childCount > 0) {
                newWelshLearnedUI.DisplayNewWelsh(choiceID);
            }
            ActivateNewGrammar(choiceID);
            ActivateNewVocab(choiceID);
        }

        public void ActivateNewGrammar(string choiceID) {
            int countGrammarActivateResults = DbCommands.GetCountFromQry(DbQueries.GetGrammarActivateCountFromChoiceIDqry(choiceID));
            if (countGrammarActivateResults > 0) {
                print("Grammar ACTIVATING!!!!");
                List<string[]> grammarActivatedList;
                DbCommands.GetDataStringsFromQry(DbQueries.GetCurrentActivateGrammarPlayerChoiceResultQry(choiceID), out grammarActivatedList);
                foreach (string[] activatedGrammar in grammarActivatedList) {
                    newWelshLearnedUI.InsertDiscoveredGrammar(activatedGrammar[1]);
                }
            }

        }

        public void ActivateNewVocab(string choiceID) {
            int countVocabActivateResults = DbCommands.GetCountFromQry(DbQueries.GetVocabActivateCountFromChoiceIDqry(choiceID));
            if (countVocabActivateResults > 0) {
                print("Vocab ACTIVATING!!!!");
                List<string[]> vocabActivatedList;
                DbCommands.GetDataStringsFromQry(DbQueries.GetCurrentActivateVocabPlayerChoiceResultQry(choiceID), out vocabActivatedList);
                foreach (string[] activatedVocab in vocabActivatedList) {
                    newWelshLearnedUI.InsertDiscoveredVocab(activatedVocab[2], activatedVocab[3]);
                }
            }
        }

        public void ActivateNewDialogue(string choiceID) {
            Debugging.PrintDbTable("ActivatedDialogues");
            int countDialogueActivateResults = DbCommands.GetCountFromQry(DbQueries.GetDialogueActivateCountFromChoiceIDqry(choiceID));
            if (countDialogueActivateResults > 0) {
                print("Dialogue ACTIVATING!!!!");
                List<string[]> dialogueActivatedList;

                DbCommands.GetDataStringsFromQry(DbQueries.GetCurrentActivateDialoguePlayerChoiceResultQry(choiceID), out dialogueActivatedList);

                foreach (string[] activatedDialogue in dialogueActivatedList) {
                    //get characters related to activated dialogue. Set active dialogues related to each character to completed
                    List<string[]> charactersRelatedToDialogue;

                    DbCommands.GetDataStringsFromQry(DbQueries.GetCharsRelatedToDialogue(activatedDialogue[1]), out charactersRelatedToDialogue);

                    foreach (string[] characterName in charactersRelatedToDialogue) {
                        List<string[]> activeDialoguesWithCharacter;
                        string charname = characterName[0];
                        DbCommands.GetDataStringsFromQry(DbQueries.GetActiveDialoguesWithCharacter(charname), out activeDialoguesWithCharacter, characterName[0]);

                        foreach (string[] dialogueWithChar in activeDialoguesWithCharacter) {
                            string dialogueID = dialogueWithChar[0];
                            print("printing active dialogues with " + charname);
                            Debugging.PrintDbQryResults(DbQueries.GetActiveDialoguesWithCharacter(charname), charname);
                            DbCommands.UpdateTableField("ActivatedDialogues", "Completed", "1", "DialogueIDs = " + dialogueID + " AND SaveIDs = 0");
                            Debugging.PrintDbTable("ActivatedDialogues");
                        }
                    }
                    DbCommands.InsertTupleToTable("ActivatedDialogues", activatedDialogue[1], "0", "0");
                    //activate dialogues in dialogue activated list
                }
            }
        }

        public void SetDialogueReasonSymbol(Sprite symbol) {
            reasonImg.sprite = symbol;
        }

        public int GetChoiceResultsCount(string choiceID) {
            int countChoiceResults = DbCommands.GetCountFromTable("PlayerChoiceResults", "ChoiceIDs = " + choiceID);
            return countChoiceResults;
        }

        public void ProcessPlayerChoiceTest(string[] vocabArray, DialogueTestDataController testController) {
            currentDialogueTestData = testController;
            EnableSubmitBtn();
            SetSubmitBtnText("Submit answer");
            InsertVocabTestNode(currentDialogueTestData.GetPlayerVocab());
            DisplayGrammar();
            InsertPlayerInputField();
            ScrollToDialogueElement(currentCharSpeaking);
        }

        public void ProcessDialogueNodeTest(string[] vocabArray) {
            EnableSubmitBtn();
            SetSubmitBtnText("Submit answer");
            TestTrigger testTrigger = new TestTrigger("Translating to English", DialogueIcon, TestTrigger.TriggerType.DialogueNode);
            currentDialogueTestData = new DialogueTestDataController(testTrigger, vocabArray, DialogueTestDataController.TestType.read, GetSpeakersName(currentNodeID));
            InsertVocabTestNode(currentDialogueTestData.GetPlayerVocab());
            DisplayGrammar();
            InsertPlayerInputField();
        }

        public void ProcessAbilityTest(CharAbility ability) {
            //queuedAbility = ability;
            EnableSubmitBtn();
            SetSubmitBtnText("Submit answer");
            SetObjectPortrait(playerCharacter.GetCombatController().GetCurrentEnemyTarget().CharacterPortrait);
            SetDialogueReasonSymbol(ability.myIcon);
            SetNewDialogueHolder();
            InsertSpacer();
            TestTrigger testTrigger = new TestTrigger(ability.GetMyName(), ability.GetMyIcon(), TestTrigger.TriggerType.Ability);
            currentDialogueTestData = new DialogueTestDataController(testTrigger, playerCharacter.CharacterName);
            InsertDialogueNode(currentDialogueTestData.GetVocabIntro()); //text instruction to player
            InsertVocabTestNode(currentDialogueTestData.GetPlayerVocab());
            DisplayGrammar();
            InsertPlayerInputField();
            SetInUse();
        }

        public void DisplayTestResult() {
            print("display test result???");
            TestResultsUI skillsResultsUI = FindObjectOfType<TestResultsUI>();
            if (currentDialogueTestData != null) {
                skillsResultsUI.DisplayResults(currentDialogueTestData);
            }
        }

        public void DisplayGrammar() {
            print("working1");
            List<string[]> testGrammar = currentDialogueTestData.GetGrammar();
            if (testGrammar != null) {
                print("working2");
                if (testGrammar.Count > 0) {
                    print("working3");
                    InsertRelatedGrammarTag();
                    foreach (string[] grammarRule in testGrammar) {
                        print("working4");
                        string strGrammarShortDesc = grammarRule[1];
                        InsertGrammarShortDesc(strGrammarShortDesc);
                    }
                }
            }
        }
    }
}