using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using DbUtilities;

namespace GameUI {
    namespace ListItems {
        public class PlayerChoice : MonoBehaviour {
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

            private string myNextNode;
            public string MyNextNode {
                get { return myNextNode; }
                set { myNextNode = value; }
            }

            PlayerController mainChar;
            LowerUI lowerUI;
            public DialogueUI dialogueUI;
            // Use this for initialization
            void Start() {
                mainChar = FindObjectOfType<PlayerController>();
                lowerUI = FindObjectOfType<LowerUI>();
            }

            void DisableMe() {
                GetComponent<Button>().interactable = false;
                GetComponent<EventTrigger>().enabled = false;
                GetComponent<Text>().text = myText;
            }

            public void SetDialogueUI(DialogueUI dm) {
                dialogueUI = dm;
            }

            public void DisplayChoiceResults() {
                DisableMe();
                dialogueUI.DestroyInteractiveChoices();
                print("displaying results");
                dialogueUI.InsertSpacer();
                dialogueUI.DisplayDialogueNode(GetDialogueNodeData(myNextNode));
                print("displaycurrentnodecharportrait");
                DisplayCurrentNodeCharacterPortrait();
            }

            public void DisplayPlayerPortrait() {
                lowerUI.SetObjectPortrait(mainChar.GetMyPortrait());
            }

            public void DisplayCurrentNodeCharacterPortrait() {
                print(dialogueUI);
                dialogueUI.SetCurrentPortrait();
            }

            private string[] GetDialogueNodeData(string nodeID) {
                string[] nodeData = DbCommands.GetTupleFromTable("DialogueNodes", "NodeIDs = " + nodeID);
                return nodeData;
            }

        }
    }
}
