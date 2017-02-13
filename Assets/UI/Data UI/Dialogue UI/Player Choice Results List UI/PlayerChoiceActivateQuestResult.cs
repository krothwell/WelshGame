using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataUI {
    namespace ListItems {
        public class PlayerChoiceActivateQuestResult : PlayerChoiceResult, IDeletableUI {

            public void DeleteSelf() {
                dialogueUI.DeleteActivateQuestPlayerChoice(MyID);
                Destroy(gameObject);
            }
        }
    }
}
