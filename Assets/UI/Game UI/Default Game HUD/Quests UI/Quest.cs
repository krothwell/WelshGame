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
            // Use this for initialization
            void Awake() {
                SetHeightToMatchText();
            }
            
            void SetHeightToMatchText() {
                layoutElement = GetComponent<LayoutElement>();
                myText = GetComponentInChildren<Text>();
                Canvas.ForceUpdateCanvases();
                layoutElement.minHeight = myText.preferredHeight;
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