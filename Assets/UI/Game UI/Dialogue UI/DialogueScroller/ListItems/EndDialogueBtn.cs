using UnityEngine;
using System.Collections;

namespace GameUI {
    namespace ListItems {
        public class EndDialogueBtn : MonoBehaviour {
            PlayerCharacter mainCharacter;
            DialogueUI dialogueUI;
            void Start() {
                dialogueUI = FindObjectOfType<DialogueUI>();
                mainCharacter = FindObjectOfType<PlayerCharacter>();
            }

            public void CloseDialogue() {
                dialogueUI.SetNotInUse();
            }
        }
    }
}