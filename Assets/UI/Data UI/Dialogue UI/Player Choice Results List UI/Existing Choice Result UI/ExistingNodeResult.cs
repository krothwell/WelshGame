using UnityEngine.UI;

namespace DataUI.ListItems {
    public class ExistingNodeResult : ExistingResult, IDeletableUI {
        private string myNodeID;
        public string MyNodeID {
            get { return myNodeID; }
            set { myNodeID = value; }
        }

        private string myText;
        public string MyText {
            get { return myText; }
            set { myText = value; }
        }

        public override void DeleteSelf() {
            dialogueUI.DeleteNodePlayerChoice();
            Destroy(gameObject);
            Destroy(this);
        }

        public void InitialiseMe(string nodeID, string nodeText) {
            transform.Find("NodeIDLbl").GetComponent<Text>().text = nodeID;
            MyID = nodeID;
            transform.Find("NodeTextLbl").GetComponent<Text>().text = nodeText;
            myText = nodeText;
        }
    }
}
