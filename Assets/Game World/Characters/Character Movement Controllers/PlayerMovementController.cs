using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;
using GameUI;

public class PlayerMovementController : CharMovementController {
    Vector2 camPosition;
    float playerMovementDelay;
    private float interactionDistance;
    private MouseSelection mouseSelection;
    PlayerCharacter playerCharacter;
    float walkSpeed;
    float runSpeed;
    bool isDecidingMovement = false, isDecisionRun = false;
    CombatUI combatui;

    void Awake() {
        ResetPlayerMovementDelay();
        mouseSelection = FindObjectOfType<MouseSelection>();
        interactionDistance = 1f;
        walkSpeed = 1.5f;
        runSpeed = 3.5f;
        combatui = FindObjectOfType<CombatUI>();
    }

    void Update() {
        DecideMovement();
        CheckToEndMovement();
        CheckToMakeMovement();
     }

    public void SetCharacter(Character character) {
        playerCharacter = (character as PlayerCharacter);
    }


    /// <summary>
    /// If the player reaches the target destination or within interaction distance of the object
    /// they are moving towards, or their status has changed then the movement can end.
    /// </summary>
    protected override void CheckToEndMovement() {
        if (isMoving) {
            if (playerCharacter.playerStatus == PlayerCharacter.PlayerStatus.movingToLocation) {
                if (myPosition == targetPosition) {
                    StopMoving();
                }
            }

            if (playerCharacter.playerStatus == PlayerCharacter.PlayerStatus.movingToObject) {
                if (GetDistanceFromMyPosition(playerCharacter.GetObjSelectedByPlayer().GetComponent<Transform>().position) < interactionDistance) {
                    StopMoving();
                }
            }

            if (playerCharacter.playerStatus == PlayerCharacter.PlayerStatus.movingToWeaponRange) {
                if (character.GetCombatController().IsCurrentTargetInWeaponRange()) {
                    StopMoving();
                    combatui.ToggleCombatUI();
                }
            }
        }
    }

    public float GetMySpeed() {
        return movement.GetMovementSpeed();
    }

    public override void StopMoving() {
        ToggleIsMoving(false);
        rerouteCount = 0;
        redirecting = false;
        
        if (playerCharacter.playerStatus == PlayerCharacter.PlayerStatus.movingToObject) {
            playerCharacter.playerStatus = PlayerCharacter.PlayerStatus.passive;
            playerCharacter.PickUpObject();
        }
        else if (playerCharacter.playerStatus == PlayerCharacter.PlayerStatus.movingToCharacter) {
            playerCharacter.playerStatus = PlayerCharacter.PlayerStatus.speakingToCharacter;
            playerCharacter.SpeakToCharacter();
        } else {
            playerCharacter.playerStatus = PlayerCharacter.PlayerStatus.passive;
        }
        isDecisionRun = isDecidingMovement = false;

    }

    public override void ProcessMovement() {
        if (playerCharacter.playerStatus != PlayerCharacter.PlayerStatus.passive) {
            SetTargetPosition(MouseSelection.GetMouseCoords2D());
            SetMyDirection(targetPosition, myPosition);
            isDecisionRun = mouseSelection.GetIsDoubleClick();
            isDecidingMovement = !isDecisionRun;
        }
    }

    private void DecideMovement() {
        bool isDecisionWalk = false;
        if (isDecidingMovement) {

            if (playerMovementDelay >= 0f) {
                playerMovementDelay -= Time.deltaTime;
            }
            else {
                isDecisionWalk = true;
                isDecidingMovement = false;
                ResetPlayerMovementDelay();
            }
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
            ToggleIsMoving(true);
        }
    }

    private void ResetPlayerMovementDelay() {
        playerMovementDelay = 0.2f;
    }
}
