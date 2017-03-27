using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToEnemyDecision : CharacterMovementDecision {
    Character characterSelected;

    public override void CheckToEndMovement() {
        if (myCharacter.GetCombatController().IsCurrentTargetInWeaponRange()) {
            myCharacter.GetCombatController().TriggerStrategyMode();
            EndDecision();
        }
    }

    public void SetCharacterToMoveTo(Character character) {
        characterSelected = character;
        myCharacter.GetMovementController().SetTargetPosition(characterSelected.GetMyPosition());
        character.GetCombatController().SetCurrentTarget(character);
    }
}
