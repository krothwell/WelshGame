using UnityEngine;
using System.Collections;

namespace DataUI {
    namespace ListItems {

        public class NewNodeChoiceResultBtn : MonoBehaviour {

            DialogueUI dialogueUI;
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

            void Start() {
                dialogueUI = FindObjectOfType<DialogueUI>();
            }

            void OnMouseUpAsButton() {
                dialogueUI.InsertDialogueNodeResult(gameObject);
            }
        }
    }
}