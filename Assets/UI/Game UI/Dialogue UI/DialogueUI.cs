using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Text.RegularExpressions;
using GameUI.ListItems;
using UIUtilities;
using DbUtilities;
using UnityEditor;
using GameUI;

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
    public class DialogueUI : MonoBehaviour {
        bool submissionScored = false;
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
    	Text answerTxt, percentageTxt;
        private GameObject panel;
        GameObject answerField;
        InputField answerInput;
        GameObject submitBtn;
        CombatUI combatUI;
        Text btnTxt;
        Animator animator;
        DialogueUI dialogueUI;
        Image objPortrait;
        protected RectTransform contentPanel;
        public GameObject dialogueHolderPrefab, characterSpeakingPrefab, inGameDialogueNode, inGamePlayerChoice, endDialogueBtn, spacer, emptyBlockPrefab;
        private ScrollRect dialogueScroller;
        private GameObject currentDialogueHolder, currentDialogueNode, currentCharSpeaking;
        private string currentCharID, currentDialogueID;
        private UIController ui;
        private PlayerController player;
        public Character currentChar;
        private Sprite currentPortrait;
        private NPCs npcs;

        void Awake() {
            dialogueScroller = transform.GetComponentInChildren<ScrollRect>();
        }
        // Use this for initialization
        void Start() {
            npcs = FindObjectOfType<NPCs>();
            ui = FindObjectOfType<UIController>();
            player = FindObjectOfType<PlayerController>();
            panel = transform.FindChild("Panel").gameObject;
            answerField = panel.transform.FindChild("InputField").gameObject;
            answerInput = answerField.GetComponent<InputField>();
            answerTxt = answerInput.transform.FindChild("Text").GetComponent<Text>();
            submitBtn = panel.transform.FindChild("SubmitBtn").gameObject;
            percentageTxt = panel.transform.FindChild("PercentageTxt").GetComponent<Text>();
            combatUI = FindObjectOfType<CombatUI>();
            btnTxt = submitBtn.transform.FindChild("Text").gameObject.GetComponent<Text>();
            player = FindObjectOfType<PlayerController>();
            objPortrait = panel.transform.FindChild("CharacterPortrait").GetComponent<Image>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void StartNewDialogue(Character character) {
            ResetLowerUIText();
            currentChar = character;
            currentCharID = character.nameID;
            currentDialogueHolder = Instantiate(dialogueHolderPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            currentDialogueHolder.transform.SetParent(dialogueScroller.gameObject.transform, false);
            dialogueScroller.GetComponent<ScrollRect>().content = currentDialogueHolder.GetComponent<RectTransform>();
            SetDialogueID();
            DisplayFirstDialogueNode();
        }

        public void ResetLowerUIText() {
            foreach (Transform dialogue in dialogueScroller.gameObject.transform) {
                Destroy(dialogue.gameObject);
            }
            currentDialogueHolder = null;
        }

        private void DisplayFirstDialogueNode() {
            if (currentDialogueID != "") {
                string[] nodeArray = DbCommands.GetTupleFromTable("DialogueNodes", "DialogueIDs = " + currentDialogueID, "NodeIDs ASC");
                DisplayDialogueNode(nodeArray);
            }
        }

        private string GetSpeakersName(string nodeID) {
            string overrideName = DbCommands.GetFieldValueFromTable("DialogueNodes", "CharacterSpeaking", "NodeIDs = " + nodeID);
            if (overrideName == "") {
                return currentChar.nameID;
            }
            else if (overrideName == "!Player") {
                return player.GetMyName();
            }
            else {
                return overrideName;
            }
        }

        public void DisplayDialogueNode(string[] nodeArray) {
            //check if node character override is there and use the override name if so.
            string charName = GetSpeakersName(nodeArray[0]);
            InsertCharName(charName);
            GameObject speakerScrollObj = currentCharSpeaking.gameObject;
            SetCurrentPortraitFromName(charName);
            DialogueNode dialogueNode = (Instantiate(
                inGameDialogueNode,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity) as GameObject).GetComponent<DialogueNode>();
            dialogueNode.MyID = nodeArray[0];
            dialogueNode.MyText = nodeArray[1];
            dialogueNode.transform.SetParent(currentDialogueHolder.transform, false);
            dialogueNode.GetComponent<DialogueNode>().SetDisplay();
            DisplayNodeChoices(nodeArray[0]);
            ScrollToDialogueElement(speakerScrollObj);
        }

        public void SetCurrentPortraitFromName(string charName) {
            if (charName != player.GetMyName()) {
                Character charOfName = npcs.GetCharacterFromName(charName);
                currentPortrait = charOfName.GetMyPortrait();
                SetObjectPortrait(currentPortrait);
            }
            else {
                SetObjectPortrait(player.GetPlayerPortrait());
                currentPortrait = player.GetPlayerPortrait();
            }

        }

        public void SetCurrentPortrait() {
            SetObjectPortrait(currentPortrait);
        }

        private void DisplayNodeChoices(string nodeID) {
            InsertSpacer();
            //check if player is node character override, if not then the name can be inserted
            if (GetSpeakersName(nodeID) != player.GetMyName()) {
                InsertCharName(player.GetMyName());
            }
            string nodeChoiceQry = "SELECT * FROM PlayerChoices WHERE NodeIDs = " + nodeID + ";";
            ui.AppendDisplayFromDb(nodeChoiceQry, currentDialogueHolder.transform, BuildNodeChoice);
            string displayEndDialogueIndicator = (DbCommands.GetFieldValueFromTable("DialogueNodes", "EndDialogueOption", "DialogueIDs = " + nodeID));
            int choicesInt = DbCommands.GetCountFromTable("PlayerChoices", "NodeIDs = " + nodeID);
            if (displayEndDialogueIndicator == "1" || choicesInt == 0) {
                InsertEndDialogue();
            }
        }

        private Transform BuildNodeChoice(string[] strArray) {
            string idStr = strArray[0];
            string choiceText = strArray[1];
            string nextNode = strArray[3];
            GameObject choice = Instantiate(inGamePlayerChoice, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            choice.transform.GetComponentInChildren<Text>().text = "\t" + choiceText;
            choice.GetComponent<PlayerChoice>().MyID = idStr;
            choice.GetComponent<PlayerChoice>().MyText = choiceText;
            choice.GetComponent<PlayerChoice>().MyNextNode = nextNode;
            choice.GetComponent<PlayerChoice>().SetDialogueUI(GetComponent<DialogueUI>());
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
            GameObject newSpacer = Instantiate(spacer, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            newSpacer.transform.SetParent(currentDialogueHolder.transform, false);
        }

        public void InsertCharName(string name) {
            GameObject charSpeaking = Instantiate(characterSpeakingPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            charSpeaking.GetComponent<Text>().text = "<b>" + name + "</b>:";
            charSpeaking.transform.SetParent(currentDialogueHolder.transform, false);
            currentCharSpeaking = charSpeaking;
        }

        public void InsertEndDialogue() {
            GameObject endDialogue = Instantiate(endDialogueBtn, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            endDialogue.transform.SetParent(currentDialogueHolder.transform, false);
            endDialogue.GetComponent<Text>().text = "\t<i>End Dialogue</i>";
        }


        private void SetDialogueID() {
            string qry = "SELECT * FROM CharacterDialogues "
                        + "LEFT JOIN Dialogues ON CharacterDialogues.DialogueIDs = Dialogues.DialogueIDs "
                        + "WHERE CharacterDialogues.CharacterNames = '" + currentCharID + "' AND Dialogues.Active = 1";
            currentDialogueID = DbCommands.GetFieldValueFromQry(qry, "DialogueIDs");
            //print(currentDialogueID);
        }

        public void ScrollToDialogueElement(GameObject element) {
            UICommands.SnapScrollToContentChild(element, dialogueScroller);
        }

        public GameObject GetCurrentSpeaker() {
            return currentCharSpeaking;
        }
        
    	public void SetPercentageCorrect() {
    		int welshLength = testWelsh.Length;
    		int percentage = 0;
    		int countCorrect = 0;
    		string answer = answerTxt.text;
    		for(int i = 0; i < welshLength; i++) {
    			if (i < answer.Length) {
    				if (answer[i] == testWelsh[i]) {
    					countCorrect++;
    				} 
    			} else { break; }
    		}
    
    		percentage = (int)Mathf.Round((100f/welshLength) * countCorrect);
    		Debug.Log(countCorrect);
    		percentageTxt.text = percentage.ToString() + "%";
    
    	}
    	
    	public void ActivateAnswerField() {
            //answerField.SetActive(true);
            //answerInput.Select();
        }
        
        public void SetInUse() {
            animator = GetComponent<Animator>();
            animator.SetBool("InUse", true);
        }


        public void SetNotInUse() {
            animator.SetBool("InUse", false);
            player.DestroySelectionCircleOfInteractiveObject();
        }
        
        public void SetBtnText(string newText) {
            btnTxt.text = newText;
        }
        
        public void ManageLowerUISubmission() {
            if (submissionScored) {
                if (combatUI.currentAbility == (CombatUI.CombatAbilities.strike)) { 
                    player.StrikeSelectedEnemy();
                    combatUI.ToggleCombatMode();
                    submissionScored = false;
                    SetBtnText("Submit answer");
                    SetNotInUse();
                }
            } else {
                SetPercentageCorrect();
                submissionScored = true;
                SetBtnText("Finish move");
            }
        }
    
        public void SetObjectPortrait(Sprite portrait) {
            objPortrait.sprite = portrait;
        }


    }
}