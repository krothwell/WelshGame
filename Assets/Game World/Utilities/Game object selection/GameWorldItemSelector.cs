using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldItemSelector : GameWorldSelector{

    public override void DisplayCircle() {
        if (abilitySelected == null) {
            BuildCircle();
        }
    }
    

    public override void SetSelected() {
        if (abilitySelected == null) {
            BuildCircle();
            BuildSelectionPlayerDecision(DefaultSelectionDecisionPrefab);
            //clicked = true;
            CharacterMovementDecision movementDecision = (CharacterMovementDecision)myDecision;
            movementDecision.InitialiseMe(selectionCircle.transform, doubleClicks);
            QueueDecisionToRun();
        }
    }
    
}
