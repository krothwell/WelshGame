using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;
using GameUI;

public class PlayerMovementController : CharMovementController {
    float playerMovementDelay;
    
    private MouseSelection mouseSelection;
    PlayerCharacter playerCharacter;
    float walkSpeed;
    float runSpeed;
    bool isDecidingMovement = false, isDecisionRun = false;
    CombatUI combatui;

    void Awake() {
        ResetPlayerMovementDelay();
        mouseSelection = FindObjectOfType<MouseSelection>();
        walkSpeed = 1f;
        runSpeed = 3f;
        combatui = FindObjectOfType<CombatUI>();
        
    }

    void Update() {
        SetIsDecisionRun();
        DecideMovement();
        CheckToMakeMovement();

     }

    public void SetCharacter(Character character) {
        playerCharacter = (character as PlayerCharacter);
    }

    public float GetMySpeed() {
        if (movement == null) {
            return 0;
        }
        else {
            return movement.GetMovementSpeed();
        }
    }

    public void SetIsDecisionRun() {
        if (Input.GetMouseButtonUp(0)) {
            isDecisionRun = mouseSelection.GetIsDoubleClick();
        }
    }

    public override void ProcessMovement() {
        SetMyDirection(targetPosition, myPosition);
        isDecidingMovement = true;
    }

    public override void DecideMovement() {
        bool isDecisionWalk = false;
        if (isDecidingMovement) {

            if (playerMovementDelay >= 0f) {
                playerMovementDelay -= Time.deltaTime;
                //print(playerMovementDelay);
            }
            else {
                isDecisionWalk = true;
                isDecidingMovement = false;
                ResetPlayerMovementDelay();
            }

            if (isDecisionRun || isDecisionWalk) {
                if (movement != null) {
                    movement.StopAction();
                }
                if (isDecisionRun) {
                    movement = new RunMovement(character.GetMyAnimator(), runSpeed);
                }
                else if (isDecisionWalk) {
                    movement = new WalkMovement(character.GetMyAnimator(), walkSpeed);
                }
                movement.MakeAction();
                if (movement != null) {
                    ToggleIsMoving(true);
                }
            }
        }
        
    }

    private void ResetPlayerMovementDelay() {
        playerMovementDelay = 0.2f;
    }
}
