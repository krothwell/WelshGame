using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;
public class GameWorldLocationSelector : GameWorldSelector {

    public override void DisplayCircle() {
        if (abilitySelected == null) {
            if (clicked) {
                BuildCircle(MouseSelection.GetMouseCoords2D());
            }
        }
    }


    public override void SetSelected() {
        if (abilitySelected == null) {
            EndCurrentSelection();
            BuildCircle(MouseSelection.GetMouseCoords2D());
            Select();
            BuildSelectionPlayerDecision(DefaultSelectionDecisionPrefab);
            MoveToLocationDecision movementDecision = (MoveToLocationDecision)myDecision;
            movementDecision.SetTargetPosition(selectionCircle.transform.position);
            movementDecision.MovementController.Character = playerCharacter;
            movementDecision.MovementController.InitialiseMe();
            movementDecision.SetMovementType(doubleClicks);
            playerCharacter.MyDecision = myDecision;
            QueueDecisionToRun();
        }
    }
}
