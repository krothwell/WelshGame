using UnityEngine;
using UnityEngine.UI;
using DbUtilities;


namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// 
        /// </summary>
        public class PrefabPartOptionBtn : MonoBehaviour {
            protected TaskPartsListUI taskPartsListUI;


            private string myPath;
            public string MyPath {
                get { return myPath; }
                set { myPath = value; }
            }

            private string myLabel;
            public string MyLabel {
                get { return myLabel; }
                set { myLabel = value; }
            }

            // Use this for initialization
            void Start() {
                taskPartsListUI = FindObjectOfType<TaskPartsListUI>();
            }

            public void InitialiseMe(string path, string label) {
                myPath = path;
                myLabel = label;
                SetText(label);
            }

            public void InsertMe() {
                string currentTaskID = FindObjectOfType<QuestsUI>().GetCurrentTaskID();
                string partID = DbCommands.GenerateUniqueID("QuestTaskParts", "PartIDs", "PartID");
                taskPartsListUI.InsertPart(partID);
                DbCommands.InsertTupleToTable("QuestTaskPartsPrefab",
                                                myPath,
                                                myLabel,
                                                partID
                                             );
                taskPartsListUI.DisplayPartsRelatedToTask(currentTaskID);
                taskPartsListUI.HideNewPartPanel();
            }

            public void SetText(string newText) {
                GetComponent<Text>().text = newText;
            }
        }
    }
}
