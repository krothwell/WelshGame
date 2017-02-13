using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DataUI.Utilities;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        public class PlayerChoiceResult : UIInputListItem, ISelectableUI {
            protected DialogueUI dialogueUI;
            private string myID;
            public string MyID {
                get { return myID; }
                set { myID = value; }
            }

            private string myText;
            public string MyText {
                get { return myText; }
                set { myText = value; }
            }
            Image inputBG;
            InputField input;
            GameObject options;

            public void SelectSelf() {
                DisplayOptions();
                SetMyColour(Colours.colorDataUIInputSelected);
            }

            public void DeselectSelf() {
                input = transform.GetComponentInChildren<InputField>();
                HideOptions();
                SetMyColour(Color.white);
                input.readOnly = true;
            }

            void DisplayOptions() {
                options = transform.FindChild("Options").gameObject;
                options.SetActive(true);
            }

            private void HideOptions() {
                options = transform.FindChild("Options").gameObject;
                options.SetActive(false);
            }

            void SetMyColour(Color newColor) {
                inputBG = transform.GetComponentInChildren<Image>();
                inputBG.color = newColor;
            }

            void OnMouseUp() {
                dialogueUI = FindObjectOfType<DialogueUI>();
                dialogueUI.ToggleSelectionTo(GetComponent<PlayerChoiceResult>(),dialogueUI.selectedChoiceResult);
            }
        }
    }
}