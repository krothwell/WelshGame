using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI {
    namespace ListItems {
        public class Quest : MonoBehaviour {
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
        }
    }
}