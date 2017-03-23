using UnityEngine;
using System.Collections;

namespace GameUI {
    namespace ListItems {
        public class EndDialogueBtn : MonoBehaviour {
            DialogueUI dialogueUI;
            void Start() {
                dialogueUI = FindObjectOfType<DialogueUI>();
            }

            public void CloseDialogue() {
                dialogueUI.SetNotInUse();
            }
        }
    }
}