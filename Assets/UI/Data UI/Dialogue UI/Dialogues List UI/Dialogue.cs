using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;
using System.Collections;
using DbUtilities;
using UnityUtilities;
namespace DataUI {
    namespace ListItems {
        public class Dialogue : UIInputListItem, ISelectableUI, IEditableUI {
            PlayerChoicesListUI playerChoicesListUI;
            DialogueNodesListUI dialogueNodesListUI;
            DialogueUI dialogueUI;
            
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
            void Start() {
                playerChoicesListUI = FindObjectOfType<PlayerChoicesListUI>();
                dialogueUI = FindObjectOfType<DialogueUI>();
                options = transform.Find("DialogueOptions").gameObject;
                saveDialogue = transform.Find("Save").gameObject;
                activeToggle = transform.GetComponentInChildren<Toggle>();
                dialogueNodesListUI = FindObjectOfType<DialogueNodesListUI>();
            }

            public void Select() {
                dialogueUI.ToggleSelectionTo(gameObject.GetComponent<Dialogue>(), dialogueUI.selectedDialogue);
            }

            public void SelectSelf() {
                DisplayOptions();
                SetColour(Colours.colorDataUIInputSelected);
                dialogueUI.DisplayCharsRelatedToDialogue();
                dialogueNodesListUI.DisplayNodesRelatedToDialogue();
                playerChoicesListUI.HidePlayerChoices();
                dialogueUI.HidePlayerChoiceResults();
            }

            public void DeselectSelf() {
                HideOptions();
                SetColour(Color.white);
                GetInputField().text = myDescription;
                StopEditing();
                activeToggle.isOn = active;
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
                DbCommands.DeleteTupleInTable("Dialogues",
                                             fields);
                Destroy(gameObject);
                dialogueUI.HideCharsRelatedToDialogue();
                dialogueUI.HideNodesRelatedToDialogue();
            }

            public void EditSelf() {
                editable = true;
                GetInputField().readOnly = false;
                activeToggle.interactable = true;
                HideOptions();
                saveDialogue.SetActive(true);
                GetInputField().Select();
            }

            public void StopEditing() {
                editable = false;
                GetInputField().readOnly = true;
                activeToggle.interactable = false;
                saveDialogue.SetActive(false);
                active = activeToggle.isOn;
            }

            public void SaveEdits() {
                string[,] fields = new string[,]{
                                            { "DialogueDescriptions", GetInputField().text },
                                         };
                DbCommands.UpdateTableTuple("Dialogues",
                                         "DialogueIDs = " + myID,
                                         fields);
                print("updated Dialogues tuple");
                if (activeToggle.isOn) {
                    DbCommands.InsertTupleToTable("ActivatedDialogues", myID, "-1", "0"); //Puts the dialgoue in activated dialogues under the "New game" save ref.
                } else {
                    string[,] activeDialoguefields = { { "DialogueIDs", myID }, { "SaveIDs", "-1" } };
                    DbCommands.DeleteTupleInTable("ActivatedDialogues", activeDialoguefields); //Removes the dialgoue in activated dialogues if it is marked as inactive.
                }
                MyDescription = GetInputField().text;
                StopEditing();
                dialogueUI.ToggleSelectionTo(GetComponent<Dialogue>(), dialogueUI.selectedDialogue);
            }


        }
    }
}