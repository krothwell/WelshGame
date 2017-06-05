using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovementDecision : CharacterDecision {
    protected Vector2 targetPosition;
    protected CharMovementController movementController;
    public CharMovementController MovementController {
        get { return movementController; }
    }
    protected CharacterMovement movementType;
    public CharacterMovement MovementType {
        get { return movementType; }
    }

    public RunMovement RunMovementPrefab;
    public WalkMovement WalkMovementPrefab;
	// Update is called once per frame
	void Update () {
        CheckToEndMovement();
        movementController.CheckToMakeMovement();
        
    }

    void Awake() {
        movementController = GetComponent<CharMovementController>();
    }


    public override void ProcessDecision() {
        movementController.SetMovementDecision(this);
        movementController.ProcessMovement(movementType);
    }

    public override void EndDecision() {
        
        movementType.StopAction();
        isEnding = true;
        print("ending decision");
        movementController.StopMoving();
        
        myCharacter.EndSelection();
        if (gameObject != null) {
            Destroy(gameObject);
        }
    }

    public void SetMovementType(bool dblClick) {
        if(dblClick) {
            RunMovement runMovement = Instantiate(RunMovementPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as RunMovement;
            runMovement.transform.SetParent(myCharacter.transform, false);
            movementType = runMovement;
            movementType.SetMovementSpeed(3f);
        } else {
            WalkMovement walkMovement = Instantiate(WalkMovementPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as WalkMovement;
            walkMovement.transform.SetParent(myCharacter.transform, false);
            movementType = walkMovement;
            movementType.SetMovementSpeed(1f);
        }
        movementType.MyAnimator = myCharacter.GetMyAnimator();
    }

    /// <summary>
    /// If the character reaches the target destination or within interaction distance of the object
    /// they are moving towards, or their status has changed then the movement can end.
    /// </summary>
    public abstract void CheckToEndMovement();

    public abstract void SetTarget(Transform targetTransform);

    public void InitialiseMe(Transform targetTransform, bool isRunning) {
        print(targetTransform);
        SetTarget(targetTransform);
        MovementController.Character = myCharacter;
        MovementController.InitialiseMe();
        SetMovementType(isRunning);
    }

}
