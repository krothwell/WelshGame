using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class TaskPartEquipItem : TaskPart {
            private string myItemName;
            public string MyItemName {
                get { return myItemName; }
                set { myItemName = value; }
            }

            public void SetItemNameText(string newText) {
                transform.FindChild("ItemNameLbl").GetComponent<Text>().text = newText;
            }
        }

        
    }
}