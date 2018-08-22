using UnityEngine;
using UnityEngine.UI;
using DbUtilities;


namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// 
        /// </summary>
        public class CompleteQuestPartOptionBtn : MonoBehaviour {
            protected TaskPartsListUI taskPartsListUI;


            private string myQuestName;
            public string MyQuestName {
                get { return myQuestName; }
                set { myQuestName = value; }
            }

            private string myDescription;
            public string MyDescription {
                get { return myDescription; }
                set { myDescription = value; }
            }

            // Use this for initialization
            void Start() {
                taskPartsListUI = FindObjectOfType<TaskPartsListUI>();
            }

            public void InitialiseMe(string myQuestName_, string description_) {
                myQuestName = myQuestName_;
                myDescription = description_;
                SetText(myQuestName_,description_);
            }

            public void InsertMe() {
                string currentTaskID = FindObjectOfType<QuestsUI>().GetCurrentTaskID();
                string partID = DbCommands.GenerateUniqueID("QuestTaskParts", "PartIDs", "PartID");
                taskPartsListUI.InsertPart(partID);
                DbCommands.InsertTupleToTable("QuestTaskPartsCompleteQuest",
                                                myQuestName,
                                                partID
                                             );
                taskPartsListUI.DisplayPartsRelatedToTask(currentTaskID);
                taskPartsListUI.HideNewPartPanel();
            }

            public void SetText(string title_, string description_) {
                transform.Find("Title").GetComponent<Text>().text = title_;
                transform.Find("Description").GetComponent<Text>().text = description_;
            }
        }
    }
}
