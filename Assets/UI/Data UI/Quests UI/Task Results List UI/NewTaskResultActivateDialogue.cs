using UnityEngine;
using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class NewTaskResultActivateDialogue : MonoBehaviour {

            private string myID;
            public string MyID {
                get { return myID; }
                set { myID = value; }
            }
            Text idTxt;
            Text speakerTxt;
            Text descTxt;
            QuestsUI questsUI;
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
            }
            public void InitialiseMe(string id, string desc, string speaker) {
                idTxt = transform.Find("DialogueIDLbl").GetComponent<Text>();
                descTxt = transform.Find("DialogueDescriptionLbl").GetComponent<Text>();
                speakerTxt = transform.Find("DialogueSpeakerLbl").GetComponent<Text>();
                idTxt.text = id;
                speakerTxt.text = speaker;
                descTxt.text = desc;
            }

            void OnMouseUpAsButton() {
                questsUI.InsertTaskResultActivateDialogue(myID);
            }
        }
    }
}