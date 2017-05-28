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


    public void SetTargetPosition(Vector2 target) {
        targetPosition = target;
        movementController.SetTargetPosition(targetPosition);
    }
}
