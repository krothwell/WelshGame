using UnityEngine;
using UnityEngine.UI;
namespace GameUI {
    namespace ListItems {
        public class DialogueNode : MonoBehaviour {
            private string myText;
            public string MyText {
                get { return myText; }
                set { myText = value; }
            }
            private string myID;
            public string MyID {
                get { return myID; }
                set { myID = value; }
            }

            Text displayText;

            DialogueUI dialogueManager;
            // Use this for initialization
            void Start() {

            }

            public void SetDisplay() {
                displayText = GetComponent<Text>();
                displayText.text = myText;
            }
        }
    }
}