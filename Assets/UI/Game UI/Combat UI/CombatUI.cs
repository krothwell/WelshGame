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
        NPCs npcController;
        AbilitiesUI abilitiesUI;
        UnderAttackUI underAttackUI;
        PlayerVitalsUI playerVitalsUI;
        // Use this for initialization
        void Start() {
            abilitiesUI = FindObjectOfType<AbilitiesUI>();
            underAttackUI = FindObjectOfType<UnderAttackUI>();
            playerVitalsUI = FindObjectOfType<PlayerVitalsUI>();
            dialogueUI = FindObjectOfType<DialogueUI>();
            npcController = FindObjectOfType<NPCs>();
            //currentAbility = CombatAbilities.passive;
            playerCharacter = FindObjectOfType<PlayerCharacter>();
            SelectedAbilityOption = "selectedAbilityOption";
            CreateSelectionToggleGroup(SelectedAbilityOption);
            HideAbilities();
            HideUnderAttack();
            HidePlayerVitals();

        }

        // Update is called once per frame
        void Update() {
            if (playerCharacter.GetCombatController().IsInCombat()) {
                if (!dialogueUI.IsInUse) {
                    if (Input.GetKeyUp(KeyCode.Space)) {
                        ToggleCombatMode();
                    }
                }
            }
        }

        public void ToggleCombatMode() {
            if (combatUIactive) {
                DeselectAbility();
                abilitiesUI.HideComponents();
                World.UnpauseGame();
                combatUIactive = false;
                if (pendingDecision != null) {
                    pendingDecision.ProcessDecision();
                }
                SetCursorDefault();
            } else {
                World.PauseGame();
                abilitiesUI.DisplayComponents();
                underAttackUI.DisplayComponents();
                playerVitalsUI.DisplayComponents();
                combatUIactive = true;
                pendingDecision = null;
            }
        }

        public void HideUnderAttack() {
            if (underAttackUI) {
                underAttackUI.HideComponents();
            }
        }

        public void HidePlayerVitals() {
            playerVitalsUI.HideComponents();
        }

        public void DisplayUnderAttack() {
            underAttackUI.DisplayComponents();
        }

        public void DisplayAbilities() {
            abilitiesUI.DisplayComponents();
        }

        public void HideAbilities() {
            if (abilitiesUI) {
                abilitiesUI.HideComponents();
            }
        }


        public void AddToUnderAttack(Character charIn) {
            underAttackUI.InsertAttacker(charIn);
        }

        public void RemoveFromUnderAttack(Character charIn) {
            underAttackUI.RemoveAttacker(charIn);
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

        public void SetAbility(GameObject abilityPrefab) {
            GameObject abilitySelected = Instantiate(abilityPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            abilitySelected.transform.SetParent(playerCharacter.GetCombatController().transform, false);
            currentAbility = abilitySelected.GetComponent<CharAbility>();
            currentAbility.InitialiseMe(playerCharacter);
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

        public void UseSelectedAbility(DialogueTestDataController testDataController) {
            print("using selected ability: " + currentAbility);
            if (testDataController.GetAnswerPercentageCorrect() >= 65) {
                if (currentAbility != null) {
                    currentAbility.UseAbility();
                    
                    //DeselectAbility();
                    //print(playerCharacter.GetCurrentSelection());
                    //playerCharacter.GetCurrentSelection().EndCurrentSelection();
                }
            }
            ToggleCombatMode();
        }

        public void RemoveCurrentAbility() {
            currentAbility = null;
            SetCursorDefault();
        }

        public CharAbility GetCurrentAbility() {
            return currentAbility;
        }

        public void SetPlayerHealthDisplay(float value) {
            playerVitalsUI.SetHealth(value);
        }

        public void SetAttackCursor() {
            Texture2D cursorTexture = cursors[0];
            CursorMode cursorMode = CursorMode.ForceSoftware;
            Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode);
        }

        public void SetCursorDefault() {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void EndCombatWithCharacter(string charName) {
            Character characterNPC = npcController.GetCharacterFromName(charName);
            characterNPC.GetCombatController().gameObject.SetActive(false);
            playerCharacter.GetCombatController().EndCombat(characterNPC);
        }
    }

}