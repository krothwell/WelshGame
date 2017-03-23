using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWorldItemDecision : CharacterMovementDecision {
    protected WorldItem itemSelected;
    public override void CheckToEndMovement() {
        if (movementController.GetDistanceFromMyPosition(targetPosition) < myCharacter.GetInteractionDistance()) {
            myCharacter.PickUpItem(itemSelected);
            EndDecision();
        }
    }



    public void SetItemToMoveTo(WorldItem item) {
        itemSelected = item;
        movementController.SetTargetPosition(item.GetComponent<Transform>().position);
    }
}
