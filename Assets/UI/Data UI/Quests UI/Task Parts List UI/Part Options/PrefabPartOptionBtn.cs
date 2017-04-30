using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// 
        /// </summary>
        public class PrefabPartOptionBtn : MonoBehaviour {
            protected QuestsUI questsUI;

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
                questsUI = FindObjectOfType<QuestsUI>();
            }

            public void InitialiseMe(string path, string label) {
                myPath = path;
                myLabel = label;
                SetText(label);
            }

            public void InsertAsTaskPart() {
                questsUI.InsertPartEquipItem(myPath);
                questsUI.HideNewPartPanel();
            }

            public void SetText(string newText) {
                GetComponent<Text>().text = newText;
            }
        }
    }
}
