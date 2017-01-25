using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialogue : MonoBehaviour {
    DataUI dataUI;
    DialogueUI dialogueUI;
    InputField input;
    Toggle activeToggle;
    Image inputBG;
    GameObject options, saveDialogue;
    bool editable;
    private string myDescription;
    public string MyDescription {
        get { return myDescription; }
        set { myDescription = value; }
    }
    private string myID;
    public string MyID {
        get { return myID; }
        set { myID = value; }
    }
    private bool active;
    public bool Active {
        get { return active; }
        set { active = value; }
    }
    // Use this for initialization
    void Start () {
        dataUI = FindObjectOfType<DataUI>();
        dialogueUI = FindObjectOfType<DialogueUI>();
        inputBG = transform.GetComponentInChildren<Image>();
        input = transform.GetComponentInChildren<InputField>();
        options = transform.FindChild("DialogueOptions").gameObject;
        saveDialogue = transform.FindChild("Save").gameObject;
        activeToggle = transform.GetComponentInChildren<Toggle>();
    }

    void Update() {
        DeselectIfClickingAnotherDialogue();
    }

    void DeselectIfClickingAnotherDialogue () {
        /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();
            if (SelectController.IsClickedGameObjectName("Dialogue") && SelectController.ClickedDifferentGameObjectTo(gameObject)) {
                DeselectDialogue();
            }
        }
    }

    void OnMouseUp() {
        SelectDialogue();
    }

    public void SelectDialogue() {
        DisplayOptions();
        SetMyColour(dataUI.colorDataUIInputSelected);
        dialogueUI.SetSelectedDialogue(gameObject);
        dialogueUI.DisplayCharsRelatedToDialogue();
        dialogueUI.DisplayNodesRelatedToDialogue();
        dialogueUI.HidePlayerChoices();
        dialogueUI.HidePlayerChoiceResults();
    }

    private void DeselectDialogue() {
        HideOptions();
        SetMyColour(Color.white);
        input.text = myDescription;
        activeToggle.isOn = active;
        input.readOnly = true;
        activeToggle.interactable = false;
        saveDialogue.SetActive(false);
        editable = false;
    }

    private void DisplayOptions() {
        if (!editable) {
            options.SetActive(true);
        }
    }

    private void HideOptions() {
        options.SetActive(false);
    }

    public void DeleteDialogue() {
        string[,] fields = { { "DialogueIDs", myID } };
        DbSetup.DeleteTupleInTable("Dialogues",
                                     fields);
        Destroy(gameObject);
        dialogueUI.HideCharsRelatedToDialogue();
        dialogueUI.HideNodesRelatedToDialogue();
    }

    public void SetEditable() {
        editable = true;
        input.readOnly = false;
        activeToggle.interactable = true;
        HideOptions();
        saveDialogue.SetActive(true);
        input.Select();
    }


    public void SaveDialogueChanges() {
        string isActive = activeToggle.isOn ? "1" : "0";
        print(isActive);
        string[,] fields = new string [,]{ 
                                            { "DialogueDescriptions", input.text },
                                            { "Active", isActive }
                                         };
        DbSetup.UpdateTableTuple("Dialogues",
                                 "DialogueIDs = " + myID,
                                 fields);
        MyDescription = input.text;
        active = activeToggle.isOn;
        editable = false;
        input.readOnly = true;
        activeToggle.interactable = false;
        saveDialogue.SetActive(false);
        SelectDialogue();
    }

    void SetMyColour(Color newColor) {
        inputBG.color = newColor;
    }
}
