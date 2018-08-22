using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// The user can select a task part to use options to edit or delete the
        /// task part. When a quest is selected it will call the QuestsUI class to
        /// display a list of associated tasks and menus related to the 
        /// management of those tasks. 
        /// </summary>
        public class TaskPart : UITextPanelListItem, ISelectableUI, IDeletableUI {
            protected QuestsUI questsUI;
            protected TaskPartsListUI taskPartsListUI;
            protected GameObject options;


            private string myID;
            public string MyID {
                get { return myID; }
                set { myID = value; }
            }

            // Use this for initialization
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
                taskPartsListUI = FindObjectOfType<TaskPartsListUI>();
                options = transform.Find("PartOptions").gameObject;
            }

            void OnMouseUpAsButton() {
                if (!questsUI.IsEditingDetails()) {
                    taskPartsListUI.ToggleSelectionTo(gameObject.GetComponent<TaskPart>(), taskPartsListUI.selectedTaskPart);
                }
            }

            public void SelectSelf() {
                DisplayOptions();
                SetColour(Colours.colorDataUIInputSelected);
            }

            public void DeselectSelf() {
                HideOptions();
                SetColour(Colours.colorDataUIPanelInactive);
            }

            private void DisplayOptions() {
                options.SetActive(true);
            }

            private void HideOptions() {
                options.SetActive(false);
            }

            public void DeleteSelf() {
                questsUI.DeleteTaskPartFromDb(MyID);
                Destroy(gameObject);
                Destroy(this);
            }
        }
    }
}