using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PchoiceResult : MonoBehaviour {
    UI ui;
    DataUI dataUI;
    DialogueUI dialogueUI;
    private string myID;
    public string MyID {
        get { return myID; }
        set { myID = value; }
    }

    private string myText;
    public string MyText {
        get { return myText; }
        set { myText = value; }
    }
    Image inputBG;
    InputField input;
    GameObject options;

    // Use this for initialization
    void Start () {
        options = transform.FindChild("Options").gameObject;
        inputBG = transform.GetComponentInChildren<Image>();
        ui = FindObjectOfType<UI>();
        dataUI = FindObjectOfType<DataUI>();
        dialogueUI = FindObjectOfType<DialogueUI>();
        input = transform.GetComponentInChildren<InputField>();
    }

    void Update() {
        ui.DeselectIfClickingAnotherListItem("ChoiceResult", gameObject, DeselectMe);
    }

    //void DeselectIfClickingAnotherNode() {
    //    /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
    //    if (Input.GetMouseButtonUp(0)) {
    //        SelectController.ClickSelect();
    //        if (SelectController.IsClickedGameObjectName("DialogNode") && SelectController.ClickedDifferentGameObjectTo(gameObject)) {
    //            DeselectNode();
    //        }
    //    }
    //}

    void SelectMe() {
        DisplayOptions();
        SetMyColour(dataUI.colorDataUIInputSelected);
        dialogueUI.SetSelectedPchoiceResult(gameObject);
    }

    void DeselectMe() {
        HideOptions();
        SetMyColour(Color.white);
        input.readOnly = true;
    }

    void DisplayOptions () {
        options.SetActive(true);
    }

    private void HideOptions() {
        options.SetActive(false);
    }

    void SetMyColour(Color newColor) {
        inputBG.color = newColor;
    }

    public void DeleteMe() {
        DbSetup.UpdateTableField("PlayerChoices", "NextNodes", "null", "ChoiceIDs = " + dialogueUI.GetSelectedPlayerChoice().GetComponent<PlayerChoice>().MyID);
        Destroy(gameObject);
    }

    void OnMouseUp() {
        SelectMe();
    }
}

