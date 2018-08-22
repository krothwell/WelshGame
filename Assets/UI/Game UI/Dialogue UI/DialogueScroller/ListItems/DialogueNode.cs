using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DbUtilities;
namespace GameUI {
    namespace ListItems {
        public class DialogueNode : MonoBehaviour {
            QuestsUI questsUI;
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
            private List<string[]> nodeCompletePartList;
            public List<string[]> NodeCompletePartList {
                get { return nodeCompletePartList; }
                set { nodeCompletePartList = value; }
            }

            Text displayText;

            public void SetDisplay(string myText_) {
                displayText = GetComponent<Text>();
                displayText.text = myText_;
            }

            public void InitialiseMe(string idStr_, string nodeText_) {
                SetDisplay(nodeText_);
                MyID = idStr_;
                MyText = nodeText_;
                questsUI = FindObjectOfType<QuestsUI>();
                CompleteApplicableTaskParts();
            }

            private void CompleteApplicableTaskParts() {
                DbCommands.GetDataStringsFromQry(
                    "SELECT QuestTaskPartsActivateDialogueNode.PartIDs, QuestTaskParts.TaskIDs, QuestsActivated.QuestNames " +
                    "FROM QuestTaskPartsActivateDialogueNode " +
                        "INNER JOIN QuestTaskParts ON QuestTaskParts.PartIDs = QuestTaskPartsActivateDialogueNode.PartIDs " +
                        "INNER JOIN QuestTasks ON QuestTasks.TaskIDs = QuestTaskParts.TaskIDs " +
                        "INNER JOIN QuestsActivated ON QuestTasks.QuestNames = QuestsActivated.QuestNames " +
                    "WHERE NodeIDs = " + myID + " " + 
                        "AND QuestsActivated.SaveIDs = 0 " +
                        "AND QuestTaskPartsActivateDialogueNode.PartIDs NOT IN (SELECT CompletedQuestTaskParts.PartIDs FROM CompletedQuestTaskParts WHERE CompletedQuestTaskParts.SaveIDs = 0);",
                    out nodeCompletePartList);
                foreach (string[] partTuple in nodeCompletePartList) {
                    questsUI.CompleteTaskPart(partTuple[0], partTuple[1], partTuple[2]);
                }
            }
        }
    }
}