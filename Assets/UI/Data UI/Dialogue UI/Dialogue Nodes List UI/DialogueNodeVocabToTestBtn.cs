using UnityEngine.UI;
using DbUtilities;

namespace DataUI.ListItems {
    public class DialogueNodeVocabToTestBtn : UITextPanelListItem {
        DialogueNodeDetailsUI dialogueNodeDetailsUI;
        DialogueNodesListUI dialogueNodesListUI;
        protected DialogueUI dialogueUI;

        private string english;
        public string English {
            get { return english; }
            set { english = value; }
        }

        private string welsh;
        public string Welsh {
            get { return welsh; }
            set { welsh = value; }
        }

        void Start() {
            dialogueNodeDetailsUI = FindObjectOfType<DialogueNodeDetailsUI>();
            dialogueNodesListUI = FindObjectOfType<DialogueNodesListUI>();
        }

        public void InitialiseMe(string en, string cy) {
            english = en;
            welsh = cy;
            transform.Find("English").GetComponent<Text>().text = en;
            transform.Find("Welsh").GetComponent<Text>().text = cy;
        }

        //protected void OnMouseUpAsButton() {
        //    InsertChoice();
        //}

        protected void InsertNewDialogueNodeVocabTest(string nodeID) {
            DbCommands.InsertTupleToTable("DialogueNodesVocabTests",
                                    nodeID,
                                    english,
                                    welsh);
            dialogueNodeDetailsUI.DeactivateNodeDetails();
        }

        public void InsertNode() {
            string nodeID = DbCommands.GenerateUniqueID("DialogueNodes", "NodeIDs", "NodeID");
            string endDialogueStr = dialogueNodeDetailsUI.EndDialogueOptionToggle.isOn ? "1" : "0";
            dialogueNodeDetailsUI.SetCharOverrideDetails();
            dialogueNodeDetailsUI.InsertDialogueNode(english, nodeID, endDialogueStr);
            InsertNewDialogueNodeVocabTest(nodeID);
            dialogueNodesListUI.DisplayNodesRelatedToDialogue();
        }
    }
}