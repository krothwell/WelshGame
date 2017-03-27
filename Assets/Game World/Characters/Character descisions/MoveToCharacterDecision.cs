﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCharacterDecision : CharacterMovementDecision {

    Character characterSelected;

    public override void CheckToEndMovement() {
        if (myCharacter.GetMovementController().GetDistanceFromMyPosition(targetPosition) < myCharacter.GetInteractionDistance()) {
            myCharacter.SpeakToCharacter(characterSelected);
            EndDecision();
        }
    }


    public void SetCharacterToMoveTo(Character character) {
        characterSelected = character;
        targetPosition = characterSelected.GetMyPosition();
        myCharacter.GetMovementController().SetTargetPosition(targetPosition);
    }
}