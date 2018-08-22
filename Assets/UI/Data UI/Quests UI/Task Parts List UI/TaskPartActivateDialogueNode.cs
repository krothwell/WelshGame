using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class TaskPartActivateDialogueNode : TaskPart {
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

            public void InitialiseMe(string myDialogueDescription_, string myNodeDescription_, string myNodeID_, string partID_) {
                SetDescriptionText(myDialogueDescription_, myNodeDescription_, myNodeID_);
                MyID = partID_;
                myDialogueDescription = myDialogueDescription_;
                myNodeDescription = myNodeDescription_;
                myNodeID = myNodeID_;
            }

            public void SetDescriptionText(string myDialogueDescription_, string myNodeDescription_, string myNodeID) {
                transform.Find("DialogueDescription").GetComponent<Text>().text = myDialogueDescription_;
                transform.Find("NodeDescription").GetComponent<Text>().text = myNodeDescription_;
                transform.Find("NodeID").GetComponent<Text>().text = myNodeID;
            }
        }
    }
}