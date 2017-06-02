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
            //EndCurrentSelection();
            BuildCircle(MouseSelection.GetMouseCoords2D());
            //clicked = true;
            BuildSelectionPlayerDecision(DefaultSelectionDecisionPrefab);
            CharacterMovementDecision movementDecision = (CharacterMovementDecision)myDecision;
            movementDecision.InitialiseMe(selectionCircle.transform, doubleClicks);
            QueueDecisionToRun();
        }
    }
}
