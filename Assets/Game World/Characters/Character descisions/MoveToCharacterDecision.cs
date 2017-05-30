using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCharacterDecision : CharacterMovementDecision {

    Character characterSelected;

    public override void CheckToEndMovement() {
        if (myCharacter.MovementController.GetDistanceFromMyPosition(targetPosition) < myCharacter.GetInteractionDistance()) {
            print("close to character");
            myCharacter.SpeakToCharacter(characterSelected);
            EndDecision();
        }
    }


    public override void SetTarget(Transform targetTransform) {
        characterSelected = targetTransform.GetComponentInParent<Character>();
        print(characterSelected);
        targetPosition = characterSelected.GetMyPosition();
        movementController.SetTargetPosition(targetPosition);
    }
}
