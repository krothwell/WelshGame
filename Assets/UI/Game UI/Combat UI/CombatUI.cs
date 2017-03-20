using UnityEngine;
using System.Collections;
using DbUtilities;

namespace GameUI {
    /// <summary>
    /// Responsible for knowing when the player is in a combat situation, 
    /// deciding when the player can enter combat mode, displaying/using combat
    /// abilities which call methods in GameUI.DialogueUI to test the player 
    /// (and provide an outcome dealt with by the DialogueUI). 
    /// </summary>
    public class CombatUI : MonoBehaviour {
        public enum CombatAbilities { passive, strike }
        public CombatAbilities currentAbility;
        GameObject abilitiesPanel;
        DefaultGameHUD explorerUI;
        public Texture2D[] cursors;
        public bool CombatUIactive;
        PlayerCharacter mainChar;
        //DialogueUI dialogueUI;
        // Use this for initialization
        void Start() {
            //dialogueUI = FindObjectOfType<DialogueUI>();
            abilitiesPanel = transform.FindChild("AbilitiesPanel").gameObject;
            explorerUI = FindObjectOfType<DefaultGameHUD>();
            currentAbility = CombatAbilities.passive;
            mainChar = FindObjectOfType<PlayerCharacter>();
        }

        // Update is called once per frame
        void Update() {
            if (mainChar.GetCombatController().IsInCombat()) {
                if (Input.GetKeyUp(KeyCode.Space)) {
                    ToggleCombatMode();
                }
            }
        }

        public void ToggleCombatMode() {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            ShowAbilities();
        }

        private void ShowAbilities() {
            if (abilitiesPanel.activeSelf) {
                abilitiesPanel.SetActive(false);
                explorerUI.ShowMe();
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                CombatUIactive = false;
                currentAbility = CombatAbilities.passive;
            }
            else {
                abilitiesPanel.SetActive(true);
                explorerUI.HideMe();
                CombatUIactive = true;
            }
        }

        public void SetStrikeAbility() {
            currentAbility = CombatAbilities.strike;
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