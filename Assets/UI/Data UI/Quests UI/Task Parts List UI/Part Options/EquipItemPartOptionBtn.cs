using UnityEngine;
using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// 
        /// </summary>
        public class EquipItemPartOptionBtn : MonoBehaviour {
            protected TaskPartsListUI taskPartsListUI;

            private string myName;
            public string MyName {
                get { return myName; }
                set { myName = value; }
            }

            // Use this for initialization
            void Start() {
                taskPartsListUI = FindObjectOfType<TaskPartsListUI>();
            }

            public void InsertAsTaskPart() {
                taskPartsListUI.InsertPartEquipItem(myName);
                taskPartsListUI.HideNewPartPanel();
            }

            public void SetText(string newText) {
                GetComponent<Text>().text = newText;
            }
        }
    }
}
