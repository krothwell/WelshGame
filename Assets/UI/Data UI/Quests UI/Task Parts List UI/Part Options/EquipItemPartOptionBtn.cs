﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        /// <summary>
        /// 
        /// </summary>
        public class EquipItemPartOptionBtn : MonoBehaviour {
            protected QuestsUI questsUI;

            private string myName;
            public string MyName {
                get { return myName; }
                set { myName = value; }
            }

            // Use this for initialization
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
            }

            public void InsertAsTaskPart() {
                questsUI.InsertPartEquipItem(myName);
                questsUI.HideNewPartPanel();
            }

            public void SetText(string newText) {
                GetComponent<Text>().text = newText;
            }
        }
    }
}
