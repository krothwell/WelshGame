﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Text.RegularExpressions;

public class InGameDialogueManager : MonoBehaviour {
    protected RectTransform contentPanel;
    public GameObject dialogueHolderPrefab, characterSpeakingPrefab, inGameDialogueNode, inGamePlayerChoice, endDialogueBtn, spacer, emptyBlockPrefab;
    private ScrollRect dialogueScroller;
    private GameObject currentDialogueHolder, currentDialogueNode;
    private string currentCharID, currentDialogueID;
    private UI ui;
    private MainCharacter mainCharacter;
    public Character currentChar;
    private Sprite currentPortrait;
    private NPCs npcs;
    private LowerUI lowerUI;

    void Awake() {
        dialogueScroller = transform.GetComponentInChildren<ScrollRect>();
    }
    // Use this for initialization
    void Start () {
        npcs = FindObjectOfType<NPCs>();
        lowerUI = FindObjectOfType<LowerUI>();
        ui = FindObjectOfType<UI>();
        mainCharacter = FindObjectOfType<MainCharacter>();
    }
	
	// Update is called once per frame
	void Update () {
	
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
        foreach(Transform dialogue in dialogueScroller.gameObject.transform) {
            Destroy(dialogue.gameObject);
        }
        currentDialogueHolder = null;
    }

    private void DisplayFirstDialogueNode() {
        string[] nodeArray = DbSetup.GetTupleFromTable("DialogueNodes", "DialogueIDs = " + currentDialogueID, "NodeIDs ASC");
        DisplayDialogueNode(nodeArray);
    }

    private string GetSpeakersName(string nodeID) {
        string overrideName = DbSetup.GetFieldValueFromTable("DialogueNodes", "CharacterSpeaking", "NodeIDs = " + nodeID);
        if (overrideName == "") {
            return currentChar.nameID;
        }
        else if (overrideName == "!Player") {
            return mainCharacter.GetMyName();
        } else {
            return overrideName;
        }
    }

    public void DisplayDialogueNode(string[] nodeArray) {
        //check if node character override is there and use the override name if so.
        string charName = GetSpeakersName(nodeArray[0]);
        InsertCharName(charName);
        SetCurrentPortraitFromName(charName);
        InGameDialogueNode dialogueNode = (Instantiate (
            inGameDialogueNode,
            new Vector3(0f, 0f, 0f), 
            Quaternion.identity) as GameObject).GetComponent<InGameDialogueNode>();
        dialogueNode.MyID = nodeArray[0];
        dialogueNode.MyText = nodeArray[1];
        dialogueNode.transform.SetParent(currentDialogueHolder.transform, false);
        dialogueNode.GetComponent<InGameDialogueNode>().SetDisplay();
        DisplayNodeChoices(nodeArray[0]);
        SnapScrollTo(dialogueNode.GetComponent<RectTransform>());
    }

    public void SetCurrentPortraitFromName(string charName) {
        print(charName);
        if (charName != mainCharacter.GetMyName()) {
            Character charOfName = npcs.GetCharacterFromName(charName);
            currentPortrait = charOfName.charPortrait;
            lowerUI.SetObjectPortrait(currentPortrait);
        } else {
            lowerUI.SetObjectPortrait(mainCharacter.GetPortrait());
        }

    }

    public void SetCurrentPortrait() {
        lowerUI.SetObjectPortrait(currentPortrait);
    }

    private void DisplayNodeChoices(string nodeID) {
        InsertSpacer();
        //check if player is node character override, if not then the name can be inserted
        if (GetSpeakersName(nodeID) != mainCharacter.GetMyName()) {
            InsertCharName(mainCharacter.GetMyName());
        }
        string nodeChoiceQry = "SELECT * FROM PlayerChoices WHERE NodeIDs = " + nodeID + ";";
        ui.AppendDisplayFromDb(nodeChoiceQry, currentDialogueHolder.transform, BuildNodeChoice);
        string displayEndDialogueIndicator = (DbSetup.GetFieldValueFromTable("DialogueNodes", "EndDialogueOption", "DialogueIDs = " + nodeID));
        int choicesInt = DbSetup.GetCountFromTable("PlayerChoices", "NodeIDs = " + nodeID);
        if (displayEndDialogueIndicator == "1" || choicesInt == 0){
            GameObject endDialogue = Instantiate(endDialogueBtn, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            endDialogue.transform.SetParent(currentDialogueHolder.transform, false);
            endDialogue.GetComponent<Text>().text = "\t<i>End Dialogue</i>";
        }
    }

    private Transform BuildNodeChoice(IDataReader _dbr) {
        string choiceText = (_dbr["ChoiceText"].ToString());
        string idStr = (_dbr["ChoiceIDs"].ToString());
        string nextNode = (_dbr["NextNodes"].ToString());
        GameObject choice = Instantiate(inGamePlayerChoice, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        choice.transform.GetComponentInChildren<Text>().text = "\t" + choiceText;
        choice.GetComponent<InGamePlayerChoice>().MyID = idStr;
        choice.GetComponent<InGamePlayerChoice>().MyText = choiceText;
        choice.GetComponent<InGamePlayerChoice>().MyNextNode = nextNode;
        choice.GetComponent<InGamePlayerChoice>().SetDialogueManager(GetComponent<InGameDialogueManager>());
        IncreaseNodeComponentsHeight(choice);
        return choice.transform;
    }

    public void DestroyInteractiveChoices() {
        foreach(Transform component in currentDialogueHolder.transform) {
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
        IncreaseNodeComponentsHeight(newSpacer);
    }

    public void InsertCharName(string name) {
        GameObject charSpeaking = Instantiate(characterSpeakingPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        charSpeaking.GetComponent<Text>().text = "<b>" + name + "</b>:";
        charSpeaking.transform.SetParent(currentDialogueHolder.transform, false);
    }


    private void SetDialogueID() {
        string qry = "SELECT * FROM CharacterDialogues "
                    + "LEFT JOIN Dialogues ON CharacterDialogues.DialogueIDs = Dialogues.DialogueIDs "
                    + "WHERE CharacterDialogues.CharacterNames = '" + currentCharID + "' AND Dialogues.Active = 1";
        currentDialogueID = DbSetup.GetFieldValueFromQry(qry, "DialogueIDs");
    }

    public void IncreaseNodeComponentsHeight(GameObject component) {
        //cancelled
    }

    public void SnapScrollTo(RectTransform target) {
        Canvas.ForceUpdateCanvases();
        currentDialogueHolder.GetComponent<RectTransform>().anchoredPosition =
            (Vector2)dialogueScroller.transform.InverseTransformPoint(currentDialogueHolder.GetComponent<RectTransform>().position)
            - (Vector2)dialogueScroller.transform.InverseTransformPoint(currentDialogueHolder.GetComponent<RectTransform>().position);
    }



}
