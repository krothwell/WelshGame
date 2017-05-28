using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class NewStartDialogueTaskResultOptionBtn : MonoBehaviour {

            private string myID;
            public string MyID {
                get { return myID; }
                set { myID = value; }
            }
            Text idTxt;
            Text speakerTxt;
            Text descTxt;
            QuestsUI questsUI;
            void Start () {
                questsUI = FindObjectOfType<QuestsUI>();
            }
            public void SetMyText(string id, string desc, string speaker) {
                idTxt = transform.Find("DialogueIDLbl").GetComponent<Text>();
                descTxt = transform.Find("DialogueDescriptionLbl").GetComponent<Text>();
                speakerTxt = transform.Find("DialogueSpeakerLbl").GetComponent<Text>();
                idTxt.text = id;
                speakerTxt.text = speaker;
                descTxt.text = desc;
            }

            void OnMouseUpAsButton() {
                questsUI.InsertTaskResultStartDialogue(myID);
            }
        }
    }
}
