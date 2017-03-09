using UnityEngine.UI;
using DbUtilities;

namespace DataUI.ListItems {


    public class NewActivateWelshVocabResultBtn : NewChoiceResultBtn {
        private string englishText;
        public string EnglishText {
            get { return englishText; }
            set { englishText = value; }
        }
        private string welshText;
        public string WelshText {
            get { return welshText; }
            set { welshText = value; }
        }

        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        public void InitialiseMe(string englishTxt, string welshTxt, string playerChoiceIDStr) {
            transform.FindChild("English").GetComponent<Text>().text = englishTxt;
            transform.FindChild("Welsh").GetComponent<Text>().text = welshTxt;
            englishText = englishTxt;
            welshText = welshTxt;
            PlayerChoiceID = playerChoiceIDStr;
        }

        protected override void InsertResult() {
            string newResultID = DbCommands.GenerateUniqueID("PlayerChoiceResults", "ResultIDs", "ResultID");
            DbCommands.InsertTupleToTable("PlayerChoiceResults", newResultID, PlayerChoiceID);
            DbCommands.InsertTupleToTable("WelshVocabActivatedByDialogueChoices", newResultID, PlayerChoiceID, welshText, englishText);
            dialogueUI.DisplayResultsRelatedToChoices();
            dialogueUI.DeactivateNewChoiceResult();
        }
    }
}