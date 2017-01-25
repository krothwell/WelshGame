using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerChoice : MonoBehaviour {
    DataUI dataUI;
    DialogueUI dialogueUI;
    InputField input;
    Image inputBG;
    GameObject options;

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
    // Use this for initialization
    void Start() {
        dataUI = FindObjectOfType<DataUI>();
        dialogueUI = FindObjectOfType<DialogueUI>();
        inputBG = transform.GetComponentInChildren<Image>();
        input = transform.GetComponentInChildren<InputField>();
        options = transform.FindChild("PlayerChoiceOptions").gameObject;
    }

    void Update() {
        DeselectIfClickingAnotherChoice();
    }

    void DeselectIfClickingAnotherChoice() {
        /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();
            if (SelectController.IsClickedGameObjectName("PlayerChoice") && SelectController.ClickedDifferentGameObjectTo(gameObject)) {
                DeselectChoice();
            }
        }
    }

    void OnMouseUp() {
        SelectChoice();
    }

    public void SelectChoice() {
        DisplayOptions();
        SetMyColour(dataUI.colorDataUIInputSelected);
        dialogueUI.SetSelectedPlayerChoice(gameObject);
        dialogueUI.DisplayResultsRelatedToChoices();
    }

    private void DeselectChoice() {
        HideOptions();
        SetMyColour(Color.white);
        input.readOnly = true;
    }

    private void DisplayOptions() {
        options.SetActive(true);
    }

    private void HideOptions() {
        options.SetActive(false);
    }

    public void DeleteChoice() {
        string[,] fields = { { "ChoiceIDs", myID } };
        DbSetup.DeleteTupleInTable("PlayerChoices",
                                     fields);
        Destroy(gameObject);
    }

    public void EditChoice() {
        dialogueUI.SetActivePlayerChoiceEdit();
    }

    void SetMyColour(Color newColor) {
        inputBG.color = newColor;
    }

    public void UpdateChoiceDisplay(string newText) {
        input.GetComponent<InputField>().text = newText;
    }
}