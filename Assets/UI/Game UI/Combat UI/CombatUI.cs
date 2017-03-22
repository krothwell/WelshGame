using UnityEngine;
using System.Collections;
using DbUtilities;
using GameUtilities;

namespace GameUI {
    /// <summary>
    /// Responsible for knowing when the player is in a combat situation, 
    /// deciding when the player can enter combat mode, displaying/using combat
    /// abilities which call methods in GameUI.DialogueUI to test the player 
    /// (and provide an outcome dealt with by the DialogueUI). 
    /// </summary>
    public class CombatUI : UIController {
        public string SelectedAbilityOption;
        //public enum CombatAbilities { passive, strike }
        public CharAbility currentAbility;
        DefaultGameHUD explorerUI;
        public Texture2D[] cursors;
        private bool combatUIactive;
        PlayerCharacter playerCharacter;
        // Use this for initialization
        void Start() {
            //dialogueUI = FindObjectOfType<DialogueUI>();
            explorerUI = FindObjectOfType<DefaultGameHUD>();
            //currentAbility = CombatAbilities.passive;
            playerCharacter = FindObjectOfType<PlayerCharacter>();
            SelectedAbilityOption = "selectedAbilityOption";
            CreateSelectionToggleGroup(SelectedAbilityOption);
        }

        // Update is called once per frame
        void Update() {
            if (playerCharacter.GetCombatController().IsInCombat()) {
                if (Input.GetKeyUp(KeyCode.Space)) {
                    ToggleCombatUI();
                }
            }
        }

        public void ToggleCombatUI() {
            if (combatUIactive) {
                HideComponents();
                World.UnpauseGame();
                combatUIactive = false;

            } else {
                World.PauseGame();
                DisplayComponents();
                combatUIactive = true;
                playerCharacter.DestroyCurrentSelectionCircle();
            }
        }

        public bool IsCombatUIActive() {
            return combatUIactive;
        }

        public void SetAbility(GameObject abilityPrefab) {
            GameObject abilitySelected = Instantiate(abilityPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            abilitySelected.transform.SetParent(playerCharacter.GetCombatController().transform, false);
            currentAbility = abilitySelected.GetComponent<CharAbility>();
        }

        public void DeselectAbility() {
            if (currentAbility != null) {
                Destroy(currentAbility.gameObject);
                currentAbility = null;
            }
        }

        public CharAbility GetCurrentAbility() {
            return currentAbility;
        }

        
        public void SetRandomVocab() {
    		//string[] testStrings = DbCommands.GetRandomTupleFromTable("VocabTranslations");
    		//testEnglish = testStrings[0];
    		//testWelsh = testStrings[1];
    		//lowerUItxt.text = "Translate the following into Welsh: " + testEnglish;
	    }

        public void SetAttackCursor() {
            Texture2D cursorTexture = cursors[0];
            CursorMode cursorMode = CursorMode.ForceSoftware;
            Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode);
        }
    }
}