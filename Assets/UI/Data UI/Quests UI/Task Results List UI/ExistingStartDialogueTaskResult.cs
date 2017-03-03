using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;

namespace DataUI {
    namespace ListItems {
        public class ExistingStartDialogueTaskResult : UITextPanelListItem, ISelectableUI, IDeletableUI {
            private string resultID;
            public string ResultID {
                get { return resultID; }
                set { resultID = value; }
            }
            protected QuestsUI questsUI;
            protected Text dialogueIDTxt;
            protected Text dialogueDescTxt;
            protected GameObject options;

            // Use this for initialization
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
                options = transform.FindChild("Options").gameObject;
            }

            public void SetMyText(string dialogueID, string dialogueDesc) {
                dialogueIDTxt = transform.FindChild("DialogueIDLbl").GetComponent<Text>();
                dialogueDescTxt = transform.FindChild("DialogueDescriptionLbl").GetComponent<Text>();
                dialogueIDTxt.text = dialogueID;
                dialogueDescTxt.text = dialogueDesc;
            }

            public void SelectSelf() {
                DisplayOptions();
                SetPanelColour(Colours.colorDataUIInputSelected);
            }

            public void DeselectSelf() {
                HideOptions();
                SetPanelColour(Colours.colorDataUIPanelInactive);
            }

            public void DeleteSelf() {
                questsUI.DeleteStartDialogueTaskResultFromDB(ResultID);
                Destroy(gameObject);
                Destroy(this);
            }
            private void DisplayOptions() {
                options.SetActive(true);
                print("options activated");
            }

            private void HideOptions() {
                options.SetActive(false);
            }

            void OnMouseUp() {
                questsUI.ToggleSelectionTo(this, questsUI.selectedTaskResult);
            }
        }
    }
}