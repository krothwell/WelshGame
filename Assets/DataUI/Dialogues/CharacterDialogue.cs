using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterDialogue : MonoBehaviour {
    DataUI dataUI;
    private string characterName;
    public string CharacterName {
        get { return characterName; }
        set { characterName = value; }
    }
    private string dialogueID;
    public string DialogueID {
        get { return dialogueID; }
        set { dialogueID = value; }
    }
    private string sceneName;
    public string SceneName {
        get { return sceneName; }
        set { sceneName = value; }
    }

    GameObject removeLinkBtn;
    Image inputBG;
    // Use this for initialization
    void Start () {
        dataUI = FindObjectOfType<DataUI>();
        removeLinkBtn = transform.FindChild("RemoveLink").gameObject;
        inputBG = transform.GetComponentInChildren<Image>();
    }
    void Update() {
        DeselectIfClickingAnotherChar();
    }

    void DeselectIfClickingAnotherChar() {
        /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();
            if (SelectController.IsClickedGameObjectName("CharacterDialog") && SelectController.ClickedDifferentGameObjectTo(gameObject)) {
                DeselectCharDialogue();
            }
        }
    }

    void OnMouseUp() {
        SelectCharDialogue();
    }

    public void SelectCharDialogue() {
        DisplayRemoveLinkBtn();
        SetMyColour(dataUI.colorDataUIInputSelected);
    }

    private void DeselectCharDialogue() {
        HideRemoveLinkBtn();
        SetMyColour(Color.white);
    }

    private void DisplayRemoveLinkBtn() {
        removeLinkBtn.SetActive(true);
    }

    private void HideRemoveLinkBtn() {
        removeLinkBtn.SetActive(false);
    }

    public void DeleteCharacterDialogue() {
        string[,] fields = { { "CharacterNames", characterName }, { "DialogueIDs", dialogueID } };
        DbSetup.DeleteTupleInTable("CharacterDialogues",
                                     fields);
        Destroy(gameObject);
    }

    void SetMyColour(Color newColor) {
        inputBG.color = newColor;
    }
}
