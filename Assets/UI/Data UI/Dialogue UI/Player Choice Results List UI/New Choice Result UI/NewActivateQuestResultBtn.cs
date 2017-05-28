using UnityEngine.UI;

namespace DataUI.ListItems {

    public class NewActivateQuestResultBtn : NewChoiceResultBtn {

        private string myName;
        public string MyName{
            get { return myName; }
            set { myName = value; }
        }

        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        public void InitialiseMe(string questName, string questDesc) {
            transform.Find("QuestDescription").GetComponent<Text>().text = questDesc;
            transform.Find("QuestName").GetComponent<Text>().text = questName;
            GetComponent<NewActivateQuestResultBtn>().MyName = questName;
        }

        protected override void InsertResult() {
            dialogueUI.InsertActivateQuestResult(myName);
        }
    }
}