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
            void Awake() {
                SetHeightToMatchText();
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
            }

            public void DeselectSelf() {
                GetComponent<Image>().color = Colours.colorQuestPanel;
            }

            void OnMouseUp () {
                questsUI.ToggleSelectionTo(this, questsUI.selectedQuest);
            }
        }
    }
}