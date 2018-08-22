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

            string[] vocabArray;
            DialogueUI dialogueUI;
            NotificationQueue notificationQueue;
            PlayerCharacter playerCharacter;
            bool isVocabTest;
            DialogueTestDataController testData;


            public void InitialiseMe(string id_, string text_, string nextNode_) {
                dialogueUI = FindObjectOfType<DialogueUI>();
                playerCharacter = FindObjectOfType<PlayerCharacter>();
                myID = id_;
                myText = text_;
                myNextNode = nextNode_;
                isVocabTest = (DbCommands.GetCountFromTable("PlayerChoicesVocabTests", "ChoiceIDs = " + myID) != 0);
                if (isVocabTest) {
                    string[] playerChoiceTestArray = DbCommands.GetTupleFromTable("PlayerChoicesVocabTests", "ChoiceIDs = " + myID);
                    vocabArray = new string[2];
                    vocabArray[0] = playerChoiceTestArray[1];
                    vocabArray[1] = playerChoiceTestArray[2];
                    print(vocabArray[0]);
                    print(vocabArray[1]);
                    TestTrigger testTrigger = new TestTrigger("Translating to Welsh", dialogueUI.DialogueIcon, TestTrigger.TriggerType.DialogueChoice);
                    testData = new DialogueTestDataController(testTrigger, vocabArray, DialogueTestDataController.TestType.write, playerCharacter.CharacterName);

                }
                transform.GetComponentInChildren<Text>().text = (isVocabTest) ? "\t" + testData.GetPlayerVocab()[0] : "\t" + myText;
            }

            void DisableMe() {
                GetComponent<Button>().interactable = false;
                GetComponent<EventTrigger>().enabled = false;
                transform.GetComponentInChildren<Text>().text = (isVocabTest) ? "\t" + testData.GetPlayerVocab()[0] : "\t" + myText;
            }

            public void SetDialogueUI(DialogueUI dm) {
                dialogueUI = dm;
            }

            public void ProcessChoiceResults() {
                DisplayCurrentNodeCharacterPortrait();
                DisableMe();
                dialogueUI.DestroyInteractiveChoices();
                dialogueUI.InsertSpacer();

                if (dialogueUI.GetChoiceResultsCount(myID) > 0) {
                    dialogueUI.ActivateQuests(myID);
                    dialogueUI.ActivateQuestTasks(myID);
                    //dialogueUI.ActivateNewGrammar(myID);
                    dialogueUI.ActivateNewWelsh(myID);
                    dialogueUI.MarkDialogueComplete(myID);
                    dialogueUI.ActivateNewDialogue(myID);

                    notificationQueue = FindObjectOfType<NotificationQueue>();
                    notificationQueue.DisplayQueuedNotifications();
                }

                if (isVocabTest) {
                    dialogueUI.CurrentPlayerChoice = this;
                    dialogueUI.ProcessPlayerChoiceTest(vocabArray, testData);
                } else if (myNextNode != "") {
                    dialogueUI.DisplayDialogueNode(GetDialogueNodeData(myNextNode));
                } else {
                    dialogueUI.SetNotInUse();
                }
            }

            public void DisplayPlayerPortrait() {
                dialogueUI.SetObjectPortrait(playerCharacter.GetPlayerPortrait());
            }

            public void DisplayCurrentNodeCharacterPortrait() {
                dialogueUI.SetCurrentPortrait();
            }

            public string[] GetDialogueNodeData(string nodeID) {
                string[] nodeData = DbCommands.GetTupleFromTable("DialogueNodes", "NodeIDs = " + nodeID);
                return nodeData;
            }

        }
    }
}
