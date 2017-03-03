using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataUI.ListItems {
    public abstract class NewChoiceResultBtn : UITextPanelListItem {

        protected DialogueUI dialogueUI;

        private string playerChoiceID;
        public string PlayerChoiceID {
            get { return playerChoiceID; }
            set { playerChoiceID = value; }
        }

        protected void OnMouseUp() {
            InsertResult();
        }

        protected abstract void InsertResult();
    }
}
