using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;

namespace DataUI {
    namespace ListItems {
        public class ExistingTaskResult : UITextPanelListItem, ISelectableUI, IDeletableUI {
            private string resultID;
            public string ResultID {
                get { return resultID; }
                set { resultID = value; }
            }
            protected QuestsUI questsUI;
            protected GameObject options;

            // Use this for initialization
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
                options = transform.Find("Options").gameObject;
            }

            public virtual void InitialiseMe(string resID) {
                resultID = resID;
            }

            public void SelectSelf() {
                DisplayOptions();
                SetColour(Colours.colorDataUIInputSelected);
            }

            public void DeselectSelf() {
                HideOptions();
                SetColour(Colours.colorDataUIPanelInactive);
            }

            public void DeleteSelf() {
                questsUI.DeleteTaskResultFromDB(ResultID);
                Destroy(gameObject);
                Destroy(this);
            }
            private void DisplayOptions() {
                options.SetActive(true);
            }

            private void HideOptions() {
                options.SetActive(false);
            }

            void OnMouseUpAsButton() {
                questsUI.ToggleSelectionTo(this, questsUI.selectedTaskResult);
            }
        }
    }
}