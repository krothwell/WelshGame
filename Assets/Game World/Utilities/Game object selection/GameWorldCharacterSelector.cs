
using UnityEngine;


public class GameWorldCharacterSelector : GameWorldSelector {
    public GameObject moveToEnemyDecisionPrefab;
    bool abilityInRange, abilityConfirm;
    public override void DisplayCircle() {
        if (abilitySelected != null) {
            if(abilitySelected.IsInRangeOfCharacter(GetComponent<Character>())) {
                abilityInRange = true;
                BuildCircle();
            } else {
                DisplayOutOfRange();
            }
        } else {
            BuildCircle();
        }

    }

    public override void SetSelected() {
        if (abilitySelected) {
            if (abilityConfirm) {
                ChangeColourToAbilityConfirmed();
                (abilitySelected as CharacterEffectAbility).SetTargetCharacter(GetComponent<Character>());
                playerCharacter.GetCombatController().SetCurrentTarget(GetComponent<Character>());
                combatUI.ConfirmAbility();
                abilityConfirm = false;
            }
            else if (abilityInRange) {
                Select();
                abilityConfirm = true;
            }
        } else {
            Select();
            if (GetComponent<Character>().GetCombatController() != null) {
                if (GetComponent<Character>().GetCombatController().GetCurrentEnemyTarget() == playerCharacter) {
                    playerCharacter.GetCombatController().SetCurrentTarget(GetComponent<Character>());
                    BuildSelectionPlayerDecision(moveToEnemyDecisionPrefab);
                    (myDecision as MoveToEnemyDecision).SetCharacterToMoveTo(GetComponent<Character>());
                }
            }
            else {
                BuildSelectionPlayerDecision(selectionDecisionPrefab);
                (myDecision as MoveToCharacterDecision).SetCharacterToMoveTo(GetComponent<Character>());
            }
            QueueDecisionToRun();
        }
        
    }

    public void DisplayOutOfRange() {
        BuildCircle();
        ChangeColourToOutOfRange();
    }

    void ChangeColourToOutOfRange() {
        myAnimator.Stop();
        selectionCircle.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.1f); ;
    }

    void ChangeColourToAbilityConfirmed() {
        selectionCircle.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255); ;
    }
}
