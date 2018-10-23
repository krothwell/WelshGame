using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWorldItemDecision : CharacterMovementDecision {
    protected WorldItem itemSelected;
    public override void EndMovementSequence() {
        if (myCharacter.MovementController.GetDistanceFromMyPosition(targetPosition) < myCharacter.GetInteractionDistance()) {
            Debug.Log(myCharacter.MovementController.GetDistanceFromMyPosition(targetPosition));
            myCharacter.PickUpItem(itemSelected);
            EndDecision();
        }
    }



    public override void SetTarget(Transform targetTransform) {
        
        itemSelected = targetTransform.GetComponentInParent<WorldItem>();
        print(itemSelected + " " + itemSelected.transform.position);
        print(myCharacter + " " + myCharacter.GetMyPosition());
        targetPosition = itemSelected.transform.localPosition;
        print(targetPosition);
        movementController.SetTargetPosition(targetPosition);
    }
}
