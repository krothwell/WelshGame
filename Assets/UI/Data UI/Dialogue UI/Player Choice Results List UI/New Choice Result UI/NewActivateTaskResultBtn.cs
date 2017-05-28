using UnityEngine.UI;
using DbUtilities;

namespace DataUI.ListItems {
    public class NewActivateTaskResultBtn : NewChoiceResultBtn {

        private string taskID;
        public string TaskID {
            get { return taskID; }
            set { taskID = value; }
        }

        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        public void InitialiseMe(string taskIDStr, string taskDesc, string playerChoiceIDStr, string questName) {
            transform.Find("TaskID").GetComponent<Text>().text = taskIDStr;
            transform.Find("TaskDescription").GetComponent<Text>().text = taskDesc;
            transform.Find("QuestName").GetComponent<Text>().text = questName;
            taskID = taskIDStr;
            PlayerChoiceID = playerChoiceIDStr;
        }

        protected override void InsertResult() {
            string newResultID = DbCommands.GenerateUniqueID("PlayerChoiceResults", "ResultIDs", "ResultID");
            DbCommands.InsertTupleToTable("PlayerChoiceResults", newResultID, PlayerChoiceID);
            DbCommands.InsertTupleToTable("TasksActivatedByDialogueChoices", newResultID, PlayerChoiceID, taskID);
            dialogueUI.DisplayResultsRelatedToChoices();
            dialogueUI.DeactivateNewChoiceResult();
        }
    }
}