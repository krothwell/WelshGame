using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocationDecision : CharacterMovementDecision {


    public override void CheckToEndMovement() {
        if (myCharacter.GetMyPosition() == movementController.GetTargetPosition()) {
            EndDecision();
        }
    }


    public override void SetTarget(Transform targetTransform) {
        targetPosition = targetTransform.position;
        movementController.SetTargetPosition(targetPosition);
    }


}
