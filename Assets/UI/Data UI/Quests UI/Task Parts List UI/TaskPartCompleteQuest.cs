using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class TaskPartCompleteQuest : TaskPart {

            private string questName;
            public string QuestName {
                get { return questName; }
                set { questName = value; }
            }

            private string questDescription;
            public string QuestDescription {
                get { return questDescription; }
                set { questDescription = value; }
            }

            public void InitialiseMe(string questName_, string description_, string partID_) {
                SetDescriptionText(questName_, description_);
                MyID = partID_;
                questDescription = description_;
                questName = questName_;
            }

            public void SetDescriptionText(string questName_, string description_) {
                transform.Find("QuestName").GetComponent<Text>().text = questName_;
                transform.Find("Description").GetComponent<Text>().text = description_;
            }
        }
    }
}