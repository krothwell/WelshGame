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
            transform.Find("English").GetComponent<Text>().text = englishTxt;
            transform.Find("Welsh").GetComponent<Text>().text = welshTxt;
            englishText = englishTxt;
            welshText = welshTxt;
            PlayerChoiceID = playerChoiceIDStr;
        }

        protected override void InsertResult() {
            InsertNewPlayerChoiceResultID();
            DbCommands.InsertTupleToTable("WelshVocabActivatedByDialogueChoices", playerChoiceResultID, PlayerChoiceID, englishText, welshText);
            dialogueUI.DisplayResultsRelatedToChoices();
            dialogueUI.DeactivateNewChoiceResult();
        }
    }
}