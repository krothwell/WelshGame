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
        public class DialogueNode : MonoBehaviour, ISelectableUI {
            PlayerChoicesListUI playerChoicesListUI;
            DialogueUI dialogueUI;
            UIDisplayController displayController;
            DialogueNodesListUI dialogueNodesListUI;

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
                dialogueNodesListUI = FindObjectOfType<DialogueNodesListUI>();
                playerChoicesListUI = FindObjectOfType<PlayerChoicesListUI>();
                dialogueUI = FindObjectOfType<DialogueUI>();
                displayController = GetComponent<UIDisplayController>();
            }

            void OnMouseUpAsButton() {
                dialogueNodesListUI.ToggleSelectionTo(GetComponent<DialogueNode>(), dialogueNodesListUI.SelectedNode);
                
            }

            public void SelectSelf() {
                displayController.SetColour(Colours.colorDataUIInputSelected);
                playerChoicesListUI.DisplayChoicesRelatedToNode();
                dialogueUI.HidePlayerChoiceResults();
                GetComponent<UIGameObjectToggle>().ActivateGameObject();
            }

            public void DeselectSelf() {
                displayController.SetColour(Color.white);
                GetComponent<UIGameObjectToggle>().DeactivateGameObject();
            }

            public void DeleteNode() {
                string[,] fields = { { "NodeIDs", myID } };
                DbCommands.DeleteTupleInTable("DialogueNodes",
                                             fields);
                playerChoicesListUI.HidePlayerChoices();
                Destroy(gameObject);
            }

            public void InitialiseMe(string myText_, string myID_) {
                myText = myText_;
                myID = myID_;
            }
        }
    }
}