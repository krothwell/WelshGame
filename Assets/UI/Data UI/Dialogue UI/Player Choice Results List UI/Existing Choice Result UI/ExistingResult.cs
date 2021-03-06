﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DataUI.Utilities;
using DbUtilities;

namespace DataUI.ListItems {
    public class ExistingResult : UITextPanelListItem, ISelectableUI, IDeletableUI {
        protected DialogueUI dialogueUI;
        private string myID;
        public string MyID {
            get { return myID; }
            set { myID = value; }
        }

        Image inputBG;
        InputField input;
        GameObject options;

        public void SelectSelf() {
            DisplayOptions();
            SetColour(Colours.colorDataUIInputSelected);
        }

        public void DeselectSelf() {
            HideOptions();
            SetColour(Color.white);
        }

        void DisplayOptions() {
            options = transform.Find("Options").gameObject;
            options.SetActive(true);
        }

        private void HideOptions() {
            options = transform.Find("Options").gameObject;
            options.SetActive(false);
        }

        void OnMouseUpAsButton() {
            dialogueUI = FindObjectOfType<DialogueUI>();
            dialogueUI.ToggleSelectionTo(GetComponent<ExistingResult>(),dialogueUI.selectedChoiceResult);
        }

        public void SetMyID(string idStr) {
            myID = idStr;
        }

        public virtual void DeleteSelf() {
            dialogueUI.DeletePlayerChoiceResultGeneric(myID);
            Destroy(gameObject);
            Destroy(this);
        }

    }
}