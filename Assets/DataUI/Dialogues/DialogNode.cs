using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogNode : MonoBehaviour {
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

    // Use this for initialization
    void Start() {
        dataUI = FindObjectOfType<DataUI>();
        dialogueUI = FindObjectOfType<DialogueUI>();
        inputBG = transform.GetComponentInChildren<Image>();
        input = transform.GetComponentInChildren<InputField>();
        options = transform.FindChild("NodeOptions").gameObject;
    }

    void Update() {
        DeselectIfClickingAnotherNode();
    }

    void DeselectIfClickingAnotherNode() {
        /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();
            if (SelectController.IsClickedGameObjectName("DialogNode") && SelectController.ClickedDifferentGameObjectTo(gameObject)) {
                DeselectNode();
            }
        }
    }

    void OnMouseUp() {
        SelectNode();
    }

    public void SelectNode() {
        DisplayOptions();
        SetMyColour(dataUI.colorDataUIInputSelected);
        dialogueUI.SetSelectedNode(gameObject);
        dialogueUI.DisplayChoicesRelatedToNode();
        dialogueUI.HidePlayerChoiceResults();
    }

    private void DeselectNode() {
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

    public void DeleteNode() {
        string[,] fields = { { "NodeIDs", myID } };
        DbSetup.DeleteTupleInTable("DialogueNodes",
                                     fields);
        dialogueUI.HidePlayerChoices();
        Destroy(gameObject);
    }

    public void EditNode() {
        dialogueUI.SetActiveNodeEdit();
    }

    void SetMyColour(Color newColor) {
        inputBG.color = newColor;
    }

    public void UpdateNodeDisplay(string newText) {
        input.GetComponent<InputField>().text = newText;
    }
}
