using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;
using DataUI.Utilities;

namespace DataUI.ListItems {

    public class PlayerChoice : MonoBehaviour, ISelectableUI {
        DialogueUI dialogueUI;
        PlayerChoicesListUI playerChoicesListUI;
        UIDisplayController displayController;

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

        void Start() {
            playerChoicesListUI = FindObjectOfType<PlayerChoicesListUI>();
            displayController = GetComponent<UIDisplayController>();
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        public void DeleteChoice() {
            string[,] fields = { { "ChoiceIDs", myID } };
            DbCommands.DeleteTupleInTable("PlayerChoices",
                                         fields);
            Destroy(gameObject);
        }

        public void InitialiseMe(string myText_, string myID_, string myNextNode_) {
            myText = myText_;
            myID = myID_;
            myNextNode = myNextNode_;
        }

        public void SelectSelf() {
            displayController.SetColour(Colours.colorDataUIInputSelected);
            print("working1");
            dialogueUI.DisplayResultsRelatedToChoices();
            print("working2");
            GetComponent<UIGameObjectToggle>().ActivateGameObject();
        }

        public void DeselectSelf() {
            displayController.SetColour(Colours.colorDataUIPanelInactive);
            GetComponent<UIGameObjectToggle>().DeactivateGameObject();
        }

        void OnMouseUpAsButton() {
            playerChoicesListUI.ToggleSelectionTo(this, playerChoicesListUI.SelectedChoice);
        }
    }
}