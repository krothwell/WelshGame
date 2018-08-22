using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class ExistingStartDialogueTaskResult : ExistingTaskResult {
            protected Text dialogueIDTxt;
            protected Text dialogueDescTxt;

            public void InitialiseMe(string resID, string dialogueID, string dialogueDesc) {
                base.InitialiseMe(resID);
                dialogueIDTxt = transform.Find("DialogueIDLbl").GetComponent<Text>();
                dialogueDescTxt = transform.Find("DialogueDescriptionLbl").GetComponent<Text>();
                dialogueIDTxt.text = dialogueID;
                dialogueDescTxt.text = dialogueDesc;
            }
        }
    }
}