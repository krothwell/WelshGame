using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;
using System.Collections;
using DbUtilities;
using UnityUtilities;
namespace DataUI {
    namespace ListItems {
        public class Quest : UIInputListItem, ISelectableUI {
            QuestsUI questsUI;
            GameObject options;
            bool editing;

            private string myName;
            public string MyName {
                get { return myName; }
                set { myName = value; }
            }
            private string myDescription;
            public string MyDescription {
                get { return myDescription; }
                set { myDescription = value; }
            }
            void Awake() {
                GetInputField().readOnly = true;
            }
            // Use this for initialization
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
                options = transform.FindChild("QuestOptions").gameObject;
            }

            void OnMouseUp() {
                if (!questsUI.IsEditingDetails()) {
                    questsUI.ToggleSelectionTo(gameObject.GetComponent<Quest>(), questsUI.selectedQuest);
                }
            }

            public void SelectSelf() {
                DisplayOptions();
                SetInputColour(Colours.colorDataUIInputSelected);
                //dialogueUI.DisplayCharsRelatedToDialogue();
                //dialogueUI.DisplayNodesRelatedToDialogue();
                //dialogueUI.HidePlayerChoices();
                //dialogueUI.HidePlayerChoiceResults();
            }

            public void DeselectSelf() {
                HideOptions();
                SetInputColour(Color.white);
                GetInputField().text = myName;
                editing = false;
            }

            private void DisplayOptions() {
                options.SetActive(true);
            }

            private void HideOptions() {
                options.SetActive(false);
            }

            public void DeleteSelf() {
                string[,] fields = { { "QuestNames", myName } };
                DbCommands.DeleteTupleInTable("Quests",
                                             fields);
                Destroy(gameObject);
                //QuestsUI.HideCharsRelatedToDialogue();
                //dialogueUI.HideNodesRelatedToDialogue();
            }

            public void EditSelf() {
                editing = true;
                questsUI.EditSelectedQuest();
            }

            public bool IsInEditMode() {
                return editing;
            }
        }
    }
}