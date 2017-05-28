using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;
using System.Collections;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// Responsible for allowing the user to select, edit and delete 
        /// dialogue nodes using the dialogue nodes list in the UI.
        /// </summary>
        public class DialogueNode : UIInputListItem, ISelectableUI {
            DialogueUI dialogueUI;
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
                dialogueUI = FindObjectOfType<DialogueUI>();
                options = transform.Find("NodeOptions").gameObject;
            }

            void OnMouseUpAsButton() {
                dialogueUI.ToggleSelectionTo(GetComponent<DialogueNode>(), dialogueUI.selectedNode);
                
            }

            public void SelectSelf() {
                DisplayOptions();
                SetInputColour(Colours.colorDataUIInputSelected);
                dialogueUI.DisplayChoicesRelatedToNode();
                dialogueUI.HidePlayerChoiceResults();
            }

            public void DeselectSelf() {
                HideOptions();
                SetInputColour(Color.white);
                GetInputField().readOnly = true;
                GetInputField().text = myText;
            }

            private void DisplayOptions() {
                options.SetActive(true);
            }

            private void HideOptions() {
                options.SetActive(false);
            }

            public void DeleteNode() {
                string[,] fields = { { "NodeIDs", myID } };
                DbCommands.DeleteTupleInTable("DialogueNodes",
                                             fields);
                dialogueUI.HidePlayerChoices();
                Destroy(gameObject);
            }

            public void EditNode() {
                dialogueUI.SetActiveNodeEdit();
            }

            public void UpdateNodeDisplay(string newText) {
                GetInputField().text = newText;
                myText = newText;
            }
        }
    }
}