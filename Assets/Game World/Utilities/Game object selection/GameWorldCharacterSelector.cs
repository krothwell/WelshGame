
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
        //EndCurrentSelection();
        if (abilitySelected) {
            if (abilityConfirm) {
                ChangeColourToAbilityConfirmed();
                (abilitySelected as CharacterEffectAbility).SetTargetCharacter(GetComponent<Character>());
                playerCharacter.GetCombatController().SetCurrentEnemyTarget(GetComponent<Character>());
                combatUI.ConfirmAbility();
                abilityConfirm = false;
            }
            else if (abilityInRange) {
                abilityConfirm = true;
            }
        } else {
            if (GetComponent<Character>().GetCombatController() != null) {
                if (GetComponent<Character>().GetCombatController().GetCurrentEnemyTarget() == playerCharacter) {
                    playerCharacter.GetCombatController().SetCurrentEnemyTarget(GetComponent<Character>());
                    BuildSelectionPlayerDecision(moveToEnemyDecisionPrefab);
                    CharacterMovementDecision movementDecision = (CharacterMovementDecision)myDecision;
                    movementDecision.InitialiseMe(selectionCircle.transform, doubleClicks);
                } else {
                    BuildSelectionPlayerDecision(DefaultSelectionDecisionPrefab);
                    CharacterMovementDecision movementDecision = (CharacterMovementDecision)myDecision;
                    movementDecision.InitialiseMe(selectionCircle.transform, doubleClicks);
                }
            }
            else {
                BuildSelectionPlayerDecision(DefaultSelectionDecisionPrefab);
                CharacterMovementDecision movementDecision = (CharacterMovementDecision)myDecision;
                print(selectionCircle);
                movementDecision.InitialiseMe(selectionCircle.transform, doubleClicks);
            }
            QueueDecisionToRun();
        }
        
    }

    public void DisplayOutOfRange() {
        BuildCircle();
        ChangeColourToOutOfRange();
    }

    void ChangeColourToOutOfRange() {
        myAnimator.StopPlayback();
        selectionCircle.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.1f); ;
    }

    void ChangeColourToAbilityConfirmed() {
        selectionCircle.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255); ;
    }
}
