using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataUI {
    namespace ListItems {
        public class ExistingActivateQuestResult : ExistingResult, IDeletableUI {
            private string myName;
            public string MyName {
                get { return myName; }
                set { myName = value; }
            }
            private string myDescription;
            public string MyDescription {
                get { return myDescription; }
                set { myDescription = value; }
            }
            public void DeleteSelf() {
                dialogueUI.DeleteActivateQuestPlayerChoice(MyID);
                Destroy(gameObject);
            }
        }
    }
}
