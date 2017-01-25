using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InGamePlayerChoice : MonoBehaviour {
    private string myText;
    public string MyText {
        get { return myText; }
        set { myText = value; }
    }
    private string myID;
    public string MyID {
        get { return myID; }
        set { myID = value; }
    }

    private string myNextNode;
    public string MyNextNode {
        get { return myNextNode; }
        set { myNextNode = value; }
    }

    MainCharacter mainChar;
    LowerUI lowerUI;
    InGameDialogueManager dialogueManager;
    // Use this for initialization
    void Start () {
        mainChar = FindObjectOfType<MainCharacter>();
        lowerUI = FindObjectOfType<LowerUI>();
	}

    void DisableMe() {
        GetComponent<Button>().interactable = false;
        GetComponent<EventTrigger>().enabled = false;
        GetComponent<Text>().text = myText;
    }

    public void SetDialogueManager(InGameDialogueManager dm) {
        dialogueManager = dm;
    }

    public void DisplayChoiceResults() {
        DisableMe();
        dialogueManager.DestroyInteractiveChoices();
        print("displaying results");
        dialogueManager.InsertSpacer();
        dialogueManager.DisplayDialogueNode(GetDialogueNodeData(myNextNode));
        DisplayCurrentNodeCharacterPortrait();
    }

    public void DisplayPlayerPortrait() {
        lowerUI.SetObjectPortrait(mainChar.GetPortrait());
    }

    public void DisplayCurrentNodeCharacterPortrait() {
        dialogueManager.SetCurrentPortrait();
    }

    private string[] GetDialogueNodeData(string nodeID) {
        string[] nodeData = DbSetup.GetTupleFromTable("DialogueNodes", "NodeIDs = " + nodeID);
        return nodeData;
    }

}
