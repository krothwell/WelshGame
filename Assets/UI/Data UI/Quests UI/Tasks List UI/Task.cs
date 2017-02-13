using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;

namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// The user can select a task to use options to edit or delete the
        /// task. When a task is selected it will call the QuestsUI class to
        /// display a list of associated task parts and menus related to the 
        /// management of those parts. 
        /// </summary>
        public class Task : UIInputListItem, ISelectableUI, IDeletableUI, IEditableUI {
            QuestsUI questsUI;
            GameObject options;
            bool editing;

            private string myID;
            public string MyID {
                get { return myID; }
                set { myID = value; }
            }
            private string myDescription;
            public string MyDescription {
                get { return myDescription; }
                set { myDescription = value; }
            }
            Button saveBtn;

            void Awake() {
                GetInputField().readOnly = true;
            }
            // Use this for initialization
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
                options = transform.FindChild("TaskOptions").gameObject;
                saveBtn = transform.FindChild("SaveBtn").GetComponent<Button>();
            }

            void OnMouseUp() {
                if (!questsUI.IsEditingDetails()) {
                    questsUI.ToggleSelectionTo(gameObject.GetComponent<Task>(), questsUI.selectedTask);
                }
            }

            public void SelectSelf() {
                DisplayOptions();
                SetInputColour(Colours.colorDataUIInputSelected);
                questsUI.DisplayPartsRelatedToTask(myID);
                //dialogueUI.DisplayNodesRelatedToDialogue();
                //dialogueUI.HidePlayerChoices();
                //dialogueUI.HidePlayerChoiceResults();
            }

            public void DeselectSelf() {
                HideOptions();
                SetInputColour(Color.white);
                GetInputField().text = myDescription;
                editing = false;
            }

            private void DisplayOptions() {
                options.SetActive(true);
            }

            private void HideOptions() {
                options.SetActive(false);
            }

            public void SaveSelf() {
                questsUI.UpdateTaskInDb(myID, GetInputField().text);
                myDescription = GetInputField().text;
            }

            public void DeleteSelf() {
                questsUI.DeleteTaskFromDb(myID);
                Destroy(gameObject);

                //TODO: ? possibly needs to utilise a function from UI
                // controller to clear selection 

                
                //dialogueUI.HideNodesRelatedToDialogue();
            }

            public void EditSelf() {
                GetInputField().readOnly = false;
                editing = true;
                HideOptions();
                saveBtn.gameObject.SetActive(true);
                GetInputField().Select();
            }
            public void StopEditing() {
                GetInputField().readOnly = true;
                saveBtn.gameObject.SetActive(false);
                editing = false;
                DisplayOptions();
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