using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocationDecision : CharacterMovementDecision {


    public override void CheckToEndMovement() {
        if (myCharacter.GetMyPosition() == movementController.GetTargetPosition()) {
            print("ending decision because I think I'm at the target position!");
            EndDecision();
        }
    }


    public override void SetTarget(Transform targetTransform) {
        targetPosition = targetTransform.position;
        movementController.SetTargetPosition(targetPosition);
    }


}
