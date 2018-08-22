using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameUI.Utilities;
namespace GameUI {
    namespace ListItems {
        public class Quest : MonoBehaviour, ISelectableUI {
            QuestsUI questsUI;
            LayoutElement layoutElement;
            Text myText;
            BoxCollider2D myBoxCollider;

            // Use this for initialization

            private string myName;
            public string MyName {
                get { return myName; }
                set { myName = value; }
            }

            private bool isCompleted;
            public bool IsCompleted {
                get { return isCompleted; }
                set { isCompleted = value; }
            }
            void Awake() {
                SetHeightToMatchText();
                questsUI = FindObjectOfType<QuestsUI>();
            }
            
            void SetHeightToMatchText() {
                layoutElement = GetComponent<LayoutElement>();
                myText = GetComponentInChildren<Text>();
                myBoxCollider = GetComponent<BoxCollider2D>();
                Canvas.ForceUpdateCanvases();
                layoutElement.minHeight = myText.preferredHeight;
                myBoxCollider.size = new Vector2(layoutElement.minWidth, layoutElement.minHeight);

            }

            public void SelectSelf() {
                GetComponent<Image>().color = Colours.colorSelectedQuestPanel;
                questsUI.SelectQuest(myName);
            }

            public void SetCompleted() {
                GetComponent<Image>().color = Colours.colorCompletedQuestPanel;
                isCompleted = true;
            }

            public void DeselectSelf() {
                if (isCompleted) {
                    GetComponent<Image>().color = Colours.colorCompletedQuestPanel;
                }
                else {
                    GetComponent<Image>().color = Colours.colorQuestPanel;
                }
            }

            void OnMouseUpAsButton () {
                questsUI.ToggleSelectionTo(this, questsUI.selectedQuest);
            }
        }
    }
}