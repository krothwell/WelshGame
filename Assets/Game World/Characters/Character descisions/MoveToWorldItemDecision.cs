using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWorldItemDecision : CharacterMovementDecision {
    protected WorldItem itemSelected;
    public override void CheckToEndMovement() {
        if (myCharacter.MovementController.GetDistanceFromMyPosition(targetPosition) < myCharacter.GetInteractionDistance()) {
            myCharacter.PickUpItem(itemSelected);
            EndDecision();
        }
    }



    public override void SetTarget(Transform targetTransform) {
        //print(item + " " + item.transform + " " + item.transform.position);
        itemSelected = targetTransform.GetComponentInParent<WorldItem>();
        print(itemSelected);
        targetPosition = itemSelected.transform.position;
        print(targetPosition);
        movementController.SetTargetPosition(targetPosition);
    }
}
