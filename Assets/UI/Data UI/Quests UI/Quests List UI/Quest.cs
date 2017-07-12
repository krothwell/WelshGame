using UnityEngine;
using DataUI.Utilities;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// The user can select a quest to use options to edit or delete the
        /// quest. When a quest is selected it will call the QuestsUI class to
        /// display a list of associated tasks and menus related to the 
        /// management of those tasks. 
        /// </summary>
        public class Quest : UIInputListItem, ISelectableUI, IDeletableUI, IEditableUI {
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
                options = transform.Find("QuestOptions").gameObject;
            }

            void OnMouseUpAsButton() {
                if (!questsUI.IsEditingDetails()) {
                    questsUI.ToggleSelectionTo(gameObject.GetComponent<Quest>(), questsUI.selectedQuest);
                }
            }

            public void SelectSelf() {
                DisplayOptions();
                SetColour(Colours.colorDataUIInputSelected);
                questsUI.DisplayTasksRelatedToDialogue(myName);
                //dialogueUI.DisplayNodesRelatedToDialogue();
                //dialogueUI.HidePlayerChoices();
                //dialogueUI.HidePlayerChoiceResults();
            }

            public void DeselectSelf() {
                HideOptions();
                SetColour(Color.white);
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

                //TODO: ? possibly needs to utilise a function from UI
                // controller to clear selection 

                questsUI.HideTasksListUI();
                //dialogueUI.HideNodesRelatedToDialogue();
            }

            public void EditSelf() {
                editing = true;
                questsUI.EditSelectedQuest();
            }

            public void UpdateSelf (string newName, string newDesc) {
                myName = newName;
                myDescription = newDesc;
                SetInputText(myDescription);
            }

            public void StopEditing() {
                editing = false;
            }
            
            /// <summary>
            /// Used to tell parent UI controller if it should open the details
            /// panel to edit or add a new quest.
            /// </summary>
            public bool IsInEditMode() {
                return editing;
            }
        }
    }
}