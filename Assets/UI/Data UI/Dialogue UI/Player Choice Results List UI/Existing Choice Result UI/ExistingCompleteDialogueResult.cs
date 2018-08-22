using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class ExistingCompleteDialogueResult : ExistingResult, IDeletableUI {
            private string myChoiceID;
            public string MyChoiceID {
                get { return myChoiceID; }
                set { myChoiceID = value; }
            }
            public override void DeleteSelf() {
                print(myChoiceID);
                Debugging.PrintDbTable("PlayerChoices");
                dialogueUI.DeleteCompleteDialogueResult(MyChoiceID);
                Destroy(gameObject);
                Destroy(this);
            }
        }
    }
}