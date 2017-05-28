using UnityEngine.UI;

namespace DataUI.ListItems {
    public class ExistingActivateTaskResult : ExistingResult {

        public void InitialiseMe(string taskID, string taskDesc, string questName) {
            transform.Find("TaskID").GetComponent<Text>().text = taskID;
            transform.Find("TaskDescription").GetComponent<Text>().text = taskDesc;
            transform.Find("QuestName").GetComponent<Text>().text = questName;
        }
    }
}