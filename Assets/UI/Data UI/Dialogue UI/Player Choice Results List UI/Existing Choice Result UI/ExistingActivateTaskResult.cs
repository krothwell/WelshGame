using UnityEngine.UI;

namespace DataUI.ListItems {
    public class ExistingActivateTaskResult : ExistingResult {

        public void InitialiseMe(string taskID, string taskDesc) {
            transform.FindChild("TaskID").GetComponent<Text>().text = taskID;
            transform.FindChild("TaskDescription").GetComponent<Text>().text = taskDesc;
        }
    }
}