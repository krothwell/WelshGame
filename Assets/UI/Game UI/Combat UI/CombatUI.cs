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
        private CharacterDecision pendingDecision;
        public Texture2D[] cursors;
        private bool combatUIactive;
        PlayerCharacter playerCharacter;
        DialogueUI dialogueUI;
        // Use this for initialization
        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
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
                if (pendingDecision != null) {
                    pendingDecision.ProcessDecision();
                }
                SetCursorDefault();
            } else {
                World.PauseGame();
                DisplayComponents();
                combatUIactive = true;
                pendingDecision = null;
            }
        }

        public void EndCurrentSelection() {
            playerCharacter.EndSelection();
        }

        public bool IsCombatUIActive() {
            return combatUIactive;
        }

        public void QueueDecision(CharacterDecision decision) {
            pendingDecision = decision;
        }

        public void SetAbility(GameObject abilityPrefab, string abilityName, Sprite abilityIcon) {
            GameObject abilitySelected = Instantiate(abilityPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            abilitySelected.transform.SetParent(playerCharacter.GetCombatController().transform, false);
            currentAbility = abilitySelected.GetComponent<CharAbility>();
            currentAbility.InitialiseMe(playerCharacter, abilityName, abilityIcon);
            SetAttackCursor();
        }

        public void ConfirmAbility() {
            dialogueUI.ProcessAbilityTest(currentAbility);
            SetCursorDefault();
        }

        public void DeselectAbility() {
            if (currentAbility != null) {
                ToggleSelectionTo(null, SelectedAbilityOption);
                //Destroy(currentAbility.gameObject);
                RemoveCurrentAbility();
            }
        }

        public void UseSelectedAbility() {
            if (currentAbility != null) {
                ToggleCombatUI();
                currentAbility.UseAbility();
                DeselectAbility();
                playerCharacter.GetCurrentSelectionCircle().DestroyMe();
            }
        }

        public void RemoveCurrentAbility() {
            currentAbility = null;
            SetCursorDefault();
        }

        public CharAbility GetCurrentAbility() {
            return currentAbility;
        }

        public void SetAttackCursor() {
            Texture2D cursorTexture = cursors[0];
            CursorMode cursorMode = CursorMode.ForceSoftware;
            Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode);
        }

        public void SetCursorDefault() {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}