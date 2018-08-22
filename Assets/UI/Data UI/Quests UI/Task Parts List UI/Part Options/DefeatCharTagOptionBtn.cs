using UnityEngine;
using UnityEngine.UI;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// 
        /// </summary>
        public class DefeatCharTagOptionBtn : MonoBehaviour {
            protected TaskPartsListUI taskPartsListUI;

            private string myTag;
            public string MyTag {
                get { return myTag; }
                set { myTag = value; }
            }

            // Use this for initialization
            void Start() {
                taskPartsListUI = FindObjectOfType<TaskPartsListUI>();
            }

            public void InsertAsTaskPart() {
                taskPartsListUI.InsertPartEquipItem(myTag);
                taskPartsListUI.HideNewPartPanel();
            }

            public void SetText(string newText) {
                GetComponent<Text>().text = newText;
            }

            public void InitialiseMe(string tag_) {
                myTag = tag_;
                SetText(myTag);
            }

            public void InsertMe() {
                string currentTaskID = FindObjectOfType<QuestsUI>().GetCurrentTaskID();
                string partID = DbCommands.GenerateUniqueID("QuestTaskParts", "PartIDs", "PartID");
                taskPartsListUI.InsertPart(partID);
                DbCommands.InsertTupleToTable("QuestTaskPartsDefeatCharTagged",
                                                myTag,
                                                partID
                                             );
                taskPartsListUI.DisplayPartsRelatedToTask(currentTaskID);
                taskPartsListUI.HideNewPartPanel();
            }
        }
    }
}
