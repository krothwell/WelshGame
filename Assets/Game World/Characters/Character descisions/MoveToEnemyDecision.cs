using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToEnemyDecision : CharacterMovementDecision {
    Character characterSelected;

    public override void EndMovementSequence() {
        if (myCharacter.GetCombatController().GetCurrentEnemyTarget() == null) {

            EndDecision();
        }
        else if (myCharacter.GetCombatController().IsCurrentTargetInWeaponRange()) {
            //print("in weapon range");
            //myCharacter.GetCombatController().TriggerStrategyMode();
            print("got to enemy");
            EndDecision();
        }
    }

    public override void SetTarget(Transform targetTransform) {
        //print(targetTransform);
        
        characterSelected = targetTransform.GetComponentInParent<Character>();
        myCharacter.GetCombatController().SetCurrentEnemyTarget(characterSelected);
        movementController.SetTargetPosition(characterSelected.GetMyPosition());
        
    }
}
