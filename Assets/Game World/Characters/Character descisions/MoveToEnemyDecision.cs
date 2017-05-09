using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToEnemyDecision : CharacterMovementDecision {
    Character characterSelected;

    public override void CheckToEndMovement() {
        if (myCharacter.GetCombatController().GetCurrentEnemyTarget() == null) {
            EndDecision();
        }
        else if (myCharacter.GetCombatController().IsCurrentTargetInWeaponRange()) {
            print("in weapon range");
            myCharacter.GetCombatController().TriggerStrategyMode();
            EndDecision();
        }
    }

    public void SetCharacterToMoveTo(Character character) {
        characterSelected = character;
        myCharacter.GetCombatController().SetCurrentEnemyTarget(character);
        myCharacter.GetMovementController().SetTargetPosition(characterSelected.GetMyPosition());
        
    }
}
