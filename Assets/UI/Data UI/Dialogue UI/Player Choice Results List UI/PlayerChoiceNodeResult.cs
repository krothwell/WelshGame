using DbUtilities;

namespace DataUI {
    namespace ListItems {
        public class PlayerChoiceNodeResult : PlayerChoiceResult, IDeletableUI {

            public void DeleteSelf() {
                dialogueUI.DeleteNodePlayerChoice();
                Destroy(gameObject);
            }
        }
    }
}