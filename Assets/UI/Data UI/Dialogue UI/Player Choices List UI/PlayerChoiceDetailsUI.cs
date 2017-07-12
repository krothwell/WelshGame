using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using DataUI.ListItems;

namespace DataUI {
    public class PlayerChoiceDetailsUI : UIController {
        DialogueNodesListUI dialogueNodesListUI;
        PlayerChoicesListUI playerChoicesListUI;
        GameObject displayPlayerChoiceDetailsBtn;
        InputField inputChoiceText;
        Dropdown choiceDropdown;
        public GameObject[] OptionComponentGroupArray;
        bool editingChoice = false;
        public string NodeType;
        // Use this for initialization
        void Start() {
            dialogueNodesListUI = FindObjectOfType<DialogueNodesListUI>();
            playerChoicesListUI = FindObjectOfType<PlayerChoicesListUI>();
            print(playerChoicesListUI);
            choiceDropdown = GetPanel().GetComponentInChildren<Dropdown>();
            displayPlayerChoiceDetailsBtn = transform.Find("DisplayPlayerChoiceDetailsBtn").gameObject;
            inputChoiceText = GetPanel().transform.GetComponentInChildren<InputField>();
            NodeType = "NodeType";
            CreateSelectionToggleGroup(NodeType);

        }

        public void ToggleChoiceComponents() {
            ToggleSelectionTo(OptionComponentGroupArray[choiceDropdown.value].GetComponent<ISelectableUI>(), NodeType);
            //OptionComponentGroupArray[choiceDropdown.value].GetComponent<ISelectableUI>().SelectSelf();
        }

        public void DeactivateChoiceDetails() {
            GetPanel().SetActive(false);
            displayPlayerChoiceDetailsBtn.GetComponent<Button>().interactable = true;
            displayPlayerChoiceDetailsBtn.GetComponent<Text>().text = "New choice";
            editingChoice = false;
            choiceDropdown.interactable = true;
        }

        public void ActivateEditNewChoice() {
            GetPanel().SetActive(true);
            displayPlayerChoiceDetailsBtn.GetComponent<Button>().interactable = false; //indicate to user that button no longer functions.
            if (editingChoice) {
                string[] choiceDesc = DbCommands.GetTupleFromTable("PlayerChoices",
                    "ChoiceIDs = " + (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).GetComponent<PlayerChoice>().MyID + ";");
                inputChoiceText.text = choiceDesc[1];
            }
        }

        public void UpdateInsertNewTextOnlyPlayerChoice() {
            if (inputChoiceText.text != null) {
                if (editingChoice) {
                    string[,] fieldVals = new string[,] {
                                                { "ChoiceText", inputChoiceText.text },
                                            };
                    DbCommands.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).MyID, fieldVals);
                    PlayerChoiceTextOnly selectedPlayerChoice = (playerChoicesListUI.GetSelectedItemFromGroup(playerChoicesListUI.SelectedChoice) as PlayerChoice).GetComponent<PlayerChoiceTextOnly>();
                    selectedPlayerChoice.UpdateChoiceDisplay(inputChoiceText.text);
                }
                else {
                    string choiceID = DbCommands.GenerateUniqueID("PlayerChoices", "ChoiceIDs", "ChoiceID");
                    InsertPlayerChoice(inputChoiceText.text, choiceID);
                    playerChoicesListUI.DisplayChoicesRelatedToNode();
                }
            }

        }

        public void InsertPlayerChoice(string choiceText, string choiceID) {
            DbCommands.InsertTupleToTable("PlayerChoices",
                                    choiceID,
                                    choiceText,
                                    (dialogueNodesListUI.GetSelectedItemFromGroup(dialogueNodesListUI.SelectedNode) as DialogueNode).MyID,
                                    null,
                                    "0");
        }

        public void SetActivePlayerChoiceEdit() {
            displayPlayerChoiceDetailsBtn.GetComponent<Text>().text = "Edit choice";
            editingChoice = true;
            choiceDropdown.value = 0;
            choiceDropdown.interactable = false;
            ActivateEditNewChoice();
        }

    }
}

