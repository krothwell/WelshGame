using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWorldItemDecision : CharacterMovementDecision {
    protected WorldItem itemSelected;
    public override void CheckToEndMovement() {
        if (myCharacter.GetMovementController().GetDistanceFromMyPosition(targetPosition) < myCharacter.GetInteractionDistance()) {
            myCharacter.PickUpItem(itemSelected);
            EndDecision();
        }
    }



    public void SetItemToMoveTo(WorldItem item) {
        print(item + " " + item.transform + " " + item.transform.position);
        itemSelected = item;
        targetPosition = item.transform.position;
        myCharacter.GetMovementController().SetTargetPosition(targetPosition);
    }
}
