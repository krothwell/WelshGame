using UnityEngine;
using System.Collections;

namespace GameUI {
    namespace ListItems {
        public class EndDialogueBtn : MonoBehaviour {
            LowerUI lowerUI;
            PlayerController mainCharacter;
            void Start() {
                lowerUI = FindObjectOfType<LowerUI>();
                mainCharacter = FindObjectOfType<PlayerController>();
            }

            public void CloseDialogue() {
                lowerUI.SetNotInUse();
                mainCharacter.playerStatus = PlayerController.PlayerStatus.passive;
            }
        }
    }
}