using UnityEngine.UI;
using DbUtilities;

namespace DataUI.ListItems {
    public class NewActivateDialogueResultBtn : NewChoiceResultBtn {
        private string dialogueID;

        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        public void InitialiseMe(string dialogueID_, string dialogueDesc, string playerChoiceID_) {
            transform.Find("DialogueID").GetComponent<Text>().text = dialogueID_;
            transform.Find("DialogueDescription").GetComponent<Text>().text = dialogueDesc;
            dialogueID = dialogueID_;
            PlayerChoiceID = playerChoiceID_;
        }

        protected override void InsertResult() {
            InsertNewPlayerChoiceResultID();
            DbCommands.InsertTupleToTable("DialoguesActivatedByDialogueChoices", playerChoiceResultID, PlayerChoiceID, dialogueID);
            dialogueUI.DisplayResultsRelatedToChoices();
            dialogueUI.DeactivateNewChoiceResult();
        }
    }


}