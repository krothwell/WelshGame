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
            //print("in weapon range");
            //myCharacter.GetCombatController().TriggerStrategyMode();
            EndDecision();
        }
    }

    public override void SetTarget(Transform targetTransform) {
        print(targetTransform);
        characterSelected = targetTransform.GetComponentInParent<Character>();
        myCharacter.GetCombatController().SetCurrentEnemyTarget(characterSelected);
        print(myCharacter);
        print(movementController);
        print(characterSelected);
        movementController.SetTargetPosition(characterSelected.GetMyPosition());
        
    }
}
