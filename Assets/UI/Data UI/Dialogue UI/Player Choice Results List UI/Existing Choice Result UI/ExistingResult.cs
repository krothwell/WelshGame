using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DataUI.Utilities;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        public class ExistingResult : UITextPanelListItem, ISelectableUI {
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
                SetPanelColour(Colours.colorDataUIInputSelected);
            }

            public void DeselectSelf() {
                HideOptions();
                SetPanelColour(Color.white);
            }

            void DisplayOptions() {
                options = transform.FindChild("Options").gameObject;
                options.SetActive(true);
            }

            private void HideOptions() {
                options = transform.FindChild("Options").gameObject;
                options.SetActive(false);
            }

            void OnMouseUp() {
                dialogueUI = FindObjectOfType<DialogueUI>();
                dialogueUI.ToggleSelectionTo(GetComponent<ExistingResult>(),dialogueUI.selectedChoiceResult);
            }
        }
    }
}