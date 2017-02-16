using DbUtilities;

namespace DataUI {
    namespace ListItems {
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
            public void DeleteSelf() {
                dialogueUI.DeleteNodePlayerChoice();
                Destroy(gameObject);
                Destroy(this);
            }
        }
    }
}