using UnityEngine;
using UnityEngine.UI;
using DbUtilities;


namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// 
        /// </summary>
        public class ActivateDialogueNodePartOptionBtn : MonoBehaviour {
            protected TaskPartsListUI taskPartsListUI;


            private string myDialogueDescription;
            public string MyDialougeDescription {
                get { return myDialogueDescription; }
                set { myDialogueDescription = value; }
            }

            private string myNodeDescription;
            public string MyNodeDescription {
                get { return myNodeDescription; }
                set { myNodeDescription = value; }
            }

            private string myNodeID;
            public string MyNodeID {
                get { return myNodeID; }
                set { myNodeID = value; }
            }

            // Use this for initialization
            void Start() {
                taskPartsListUI = FindObjectOfType<TaskPartsListUI>();
            }

            public void InitialiseMe(string myDialogueDescription_, string myNodeDescription_, string myNodeID_) {
                myDialogueDescription = myDialogueDescription_;
                myNodeDescription = myNodeDescription_;
                myNodeID = myNodeID_;
                SetText(myDialogueDescription, myNodeDescription, myNodeID);
            }

            public void InsertMe() {
                string currentTaskID = FindObjectOfType<QuestsUI>().GetCurrentTaskID();
                string partID = DbCommands.GenerateUniqueID("QuestTaskParts", "PartIDs", "PartID");
                taskPartsListUI.InsertPart(partID);
                DbCommands.InsertTupleToTable("QuestTaskPartsActivateDialogueNode",
                                                myNodeID,
                                                partID
                                             );
                taskPartsListUI.DisplayPartsRelatedToTask(currentTaskID);
                taskPartsListUI.HideNewPartPanel();
            }

            public void SetText(string myDialogueDescription_, string myNodeDescription_, string myNodeID_) {
                transform.Find("DialogueDescription").GetComponent<Text>().text = myDialogueDescription_;
                transform.Find("NodeDescription").GetComponent<Text>().text = myNodeDescription_;
                transform.Find("NodeID").GetComponent<Text>().text = myNodeID_;
            }
        }
    }
}
