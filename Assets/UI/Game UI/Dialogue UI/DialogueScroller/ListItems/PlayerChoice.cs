using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using DbUtilities;

namespace GameUI {
    namespace ListItems {
        /// <summary>
        /// Displays a player choice clickable text in the game’s dialogue UI.
        /// The button is instantiated with data from the database when a
        /// related node is activated either at the start of a Dialogue or 
        /// because of another player choice being selected. Selecting the 
        /// choice can result in various outcomes steered by querying related
        /// tables in the database from the DialogueUI class. 
        /// </summary>
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
            public DialogueUI dialogueUI;
            NotificationQueue notificationQueue;

            void DisableMe() {
                GetComponent<Button>().interactable = false;
                GetComponent<EventTrigger>().enabled = false;
                GetComponent<Text>().text = myText;
            }

            public void SetDialogueUI(DialogueUI dm) {
                dialogueUI = dm;
            }

            public void ProcessChoiceResults() {
                DisableMe();
                dialogueUI.DestroyInteractiveChoices();
                dialogueUI.InsertSpacer();
                if (myNextNode != "") {
                    dialogueUI.DisplayDialogueNode(GetDialogueNodeData(myNextNode));
                    DisplayCurrentNodeCharacterPortrait();
                } else {
                    dialogueUI.SetNotInUse();
                }
                
                if (dialogueUI.GetChoiceResultsCount(myID) > 0) {
                    dialogueUI.ActivateQuests(myID);
                    dialogueUI.ActivateQuestTasks(myID);
                    dialogueUI.MarkDialogueComplete(myID);

                    notificationQueue = FindObjectOfType<NotificationQueue>();
                    notificationQueue.DisplayQueuedNotifications();
                }
            }

            public void DisplayPlayerPortrait() {

            }

            public void DisplayCurrentNodeCharacterPortrait() {
                dialogueUI.SetCurrentPortrait();
            }

            private string[] GetDialogueNodeData(string nodeID) {
                string[] nodeData = DbCommands.GetTupleFromTable("DialogueNodes", "NodeIDs = " + nodeID);
                return nodeData;
            }

        }
    }
}
