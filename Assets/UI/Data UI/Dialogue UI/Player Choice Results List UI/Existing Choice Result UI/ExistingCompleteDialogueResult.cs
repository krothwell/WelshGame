using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataUI {
    namespace ListItems {
        public class ExistingCompleteDialogueResult : ExistingResult, IDeletableUI {
            private string myChoiceID;
            public string MyChoiceID {
                get { return myChoiceID; }
                set { myChoiceID = value; }
            }
            public void DeleteSelf() {
                dialogueUI.DeleteCompleteDialogueResult(MyChoiceID);
                Destroy(gameObject);
                Destroy(this);
            }
        }
    }
}