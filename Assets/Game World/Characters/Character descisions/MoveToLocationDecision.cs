using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocationDecision : CharacterMovementDecision {


    public override void CheckToEndMovement() {
        if (myCharacter.GetMyPosition() == myCharacter.GetMovementController().GetTargetPosition()) {
            EndDecision();
        }
    }


    public void SetTargetPosition(Vector2 target) {
        targetPosition = target;
        myCharacter.GetMovementController().SetTargetPosition(targetPosition);
    }
}
