using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovementDecision : CharacterDecision {
    protected Vector2 targetPosition;
	// Update is called once per frame
	void Update () {
        CheckToEndMovement();
    }

    public override void ProcessDecision() {
        myCharacter.GetMovementController().SetMovementDecision(this);
        myCharacter.GetMovementController().ProcessMovement();
    }

    public override void EndDecision() {
        myCharacter.GetMovementController().StopMoving();
        myCharacter.EndSelection();
        Destroy(gameObject);
        Destroy(this);
    }

    /// <summary>
    /// If the character reaches the target destination or within interaction distance of the object
    /// they are moving towards, or their status has changed then the movement can end.
    /// </summary>
    public abstract void CheckToEndMovement();

}
