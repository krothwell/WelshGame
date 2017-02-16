using UnityEngine;
using System.Collections;

namespace DataUI {
    namespace ListItems {

        public class NewActivateQuestResultBtn : UITextPanelListItem {

            DialogueUI dialogueUI;
            private string myName;
            public string MyName{
                get { return myName; }
                set { myName = value; }
            }

            private string myDescription;
            public string MyDescription {
                get { return myDescription; }
                set { myDescription = value; }
            }

            void Start() {
                dialogueUI = FindObjectOfType<DialogueUI>();
            }

            void OnMouseUp() {
                dialogueUI.InsertActivateQuestResult(gameObject);
            }


        }
    }
}