using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DataUI.Utilities;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class PlayerChoice : UIInputListItem, ISelectableUI {
            DialogueUI dialogueUI;
            InputField input;
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
                dialogueUI = FindObjectOfType<DialogueUI>();
                input = transform.GetComponentInChildren<InputField>();
                options = transform.FindChild("PlayerChoiceOptions").gameObject;
                print(options);
            }

            void OnMouseUp() {
                dialogueUI.ToggleSelectionTo(GetComponent<PlayerChoice>(), dialogueUI.selectedChoice);
            }

            public void SelectSelf() {
                DisplayOptions();
                SetInputColour(Colours.colorDataUIInputSelected);
                //dialogueUI.SetSelectedPlayerChoice(gameObject);
                dialogueUI.DisplayResultsRelatedToChoices();
            }

            public void DeselectSelf() {
                HideOptions();
                SetInputColour(Color.white);
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
                DbCommands.DeleteTupleInTable("PlayerChoices",
                                             fields);
                Destroy(gameObject);
            }

            public void EditChoice() {
                dialogueUI.SetActivePlayerChoiceEdit();
            }

            public void UpdateChoiceDisplay(string newText) {
                input.GetComponent<InputField>().text = newText;
            }
        }
    }
}