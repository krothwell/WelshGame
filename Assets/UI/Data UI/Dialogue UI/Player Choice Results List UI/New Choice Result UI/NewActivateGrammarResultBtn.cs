using UnityEngine.UI;
using DbUtilities;

namespace DataUI.ListItems {
    public class NewActivateGrammarResultBtn : NewChoiceResultBtn {
        private string ruleID;

        void Start () {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        public void InitialiseMe(string grammarID, string grammarSummary, string grammarDesc, string playerChoiceIDStr) {
            transform.FindChild("GrammarID").GetComponent<Text>().text = ruleID;
            transform.FindChild("GrammarSummary").GetComponent<Text>().text = grammarSummary;
            transform.FindChild("GrammarDescription").GetComponent<Text>().text = grammarDesc;
            ruleID = grammarID;
            PlayerChoiceID = playerChoiceIDStr;
        }

        protected override void InsertResult() {
            InsertNewPlayerChoiceResultID();
            DbCommands.InsertTupleToTable("GrammarActivatedByDialogueChoices", playerChoiceResultID, PlayerChoiceID, ruleID);
            dialogueUI.DisplayResultsRelatedToChoices();
            dialogueUI.DeactivateNewChoiceResult();
        }
    }


}