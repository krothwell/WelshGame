using DbUtilities;

namespace DataUI.ListItems {
    public abstract class NewChoiceResultBtn : UITextPanelListItem {

        protected DialogueUI dialogueUI;

        private string playerChoiceID;
        public string PlayerChoiceID {
            get { return playerChoiceID; }
            set { playerChoiceID = value; }
        }

        protected string playerChoiceResultID;

        protected void InsertNewPlayerChoiceResultID() {
            playerChoiceResultID = DbCommands.GenerateUniqueID("PlayerChoiceResults", "ResultIDs", "ResultID");
            DbCommands.InsertTupleToTable("PlayerChoiceResults", playerChoiceResultID, PlayerChoiceID);
        }

        public abstract void InsertResult();
    }
}
