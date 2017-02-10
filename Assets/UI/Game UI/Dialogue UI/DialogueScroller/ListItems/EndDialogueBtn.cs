using UnityEngine;
using System.Collections;

namespace GameUI {
    namespace ListItems {
        public class EndDialogueBtn : MonoBehaviour {
            PlayerController mainCharacter;
            DialogueUI dialogueUI;
            void Start() {
                dialogueUI = FindObjectOfType<DialogueUI>();
                mainCharacter = FindObjectOfType<PlayerController>();
            }

            public void CloseDialogue() {
                dialogueUI.SetNotInUse();
                mainCharacter.playerStatus = PlayerController.PlayerStatus.passive;
            }
        }
    }
}