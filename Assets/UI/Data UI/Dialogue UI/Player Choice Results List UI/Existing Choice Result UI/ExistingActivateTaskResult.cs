using UnityEngine.UI;

namespace DataUI.ListItems {
    public class ExistingActivateTaskResult : ExistingResult {

        public void InitialiseMe(string taskID, string taskDesc, string questName) {
            transform.FindChild("TaskID").GetComponent<Text>().text = taskID;
            transform.FindChild("TaskDescription").GetComponent<Text>().text = taskDesc;
            transform.FindChild("QuestName").GetComponent<Text>().text = questName;
        }
    }
}