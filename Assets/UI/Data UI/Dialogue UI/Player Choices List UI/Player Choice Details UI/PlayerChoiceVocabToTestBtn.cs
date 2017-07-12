using UnityEngine.UI;
using DbUtilities;

namespace DataUI.ListItems {
    public class PlayerChoiceVocabToTestBtn : UITextPanelListItem {
        PlayerChoiceDetailsUI playerChoiceDetailsUI;
        PlayerChoicesListUI playerChoiceListUI;
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
            playerChoiceDetailsUI = FindObjectOfType<PlayerChoiceDetailsUI>();
            playerChoiceListUI = FindObjectOfType<PlayerChoicesListUI>();
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

        protected void InsertNewPlayerChoiceVocabTest(string choiceID) {
            DbCommands.InsertTupleToTable("PlayerChoicesVocabTests",
                                    choiceID,
                                    english,
                                    welsh);
            playerChoiceDetailsUI.DeactivateChoiceDetails();
        }

        public void InsertChoice() {
            string choiceID = DbCommands.GenerateUniqueID("PlayerChoices", "ChoiceIDs", "ChoiceID");
            playerChoiceDetailsUI.InsertPlayerChoice(english, choiceID);
            InsertNewPlayerChoiceVocabTest(choiceID);
            playerChoiceListUI.DisplayChoicesRelatedToNode();
        }
    }
}