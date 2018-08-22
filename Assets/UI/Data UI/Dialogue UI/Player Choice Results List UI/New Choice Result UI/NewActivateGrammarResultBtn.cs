using UnityEngine.UI;
using DbUtilities;

namespace DataUI.ListItems {
    public class NewActivateGrammarResultBtn : NewChoiceResultBtn {
        private string ruleID;

        void Start () {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        public void InitialiseMe(string grammarID, string grammarSummary, string grammarDesc, string playerChoiceIDStr) {
            transform.Find("GrammarID").GetComponent<Text>().text = ruleID;
            transform.Find("GrammarSummary").GetComponent<Text>().text = grammarSummary;
            transform.Find("GrammarDescription").GetComponent<Text>().text = grammarDesc;
            ruleID = grammarID;
            PlayerChoiceID = playerChoiceIDStr;
        }

        public override void InsertResult() {
            InsertNewPlayerChoiceResultID();
            DbCommands.InsertTupleToTable("GrammarActivatedByDialogueChoices", playerChoiceResultID, PlayerChoiceID, ruleID);
            dialogueUI.DisplayResultsRelatedToChoices();
            dialogueUI.DeactivateNewChoiceResult();
        }
    }


}