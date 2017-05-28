//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityUtilities;
//using GameUI;

//public class PlayerMovementController : CharMovementController {
//    float playerMovementDelay;
    
//    private MouseSelection mouseSelection;
//    PlayerCharacter playerCharacter;
//    float walkSpeed;
//    float runSpeed;
//    bool isDecidingMovement = false, isDecisionRun = false;
//    CombatUI combatui;

//    void Awake() {
//        ResetPlayerMovementDelay();
//        mouseSelection = FindObjectOfType<MouseSelection>();
//        walkSpeed = 1f;
//        runSpeed = 3f;
//        combatui = FindObjectOfType<CombatUI>();
        
//    }

//    void Update() {
//        //SetIsDecisionRun();

//     }

    //public void SetCharacter(Character character) {
    //    playerCharacter = (character as PlayerCharacter);
    //}



    //public override void DecideMovement() {
    //    if (playerCharacter.GetCurrentSelection() != null) {
    //        isDecisionRun = playerCharacter.GetCurrentSelection().DoubleClicks;
    //    }
    //    bool isDecisionWalk = false;
    //    if (isDecidingMovement) {

    //        if (playerMovementDelay >= 0f) {
    //            playerMovementDelay -= Time.deltaTime;
    //            //print(playerMovementDelay);
    //        }
    //        else {
    //            isDecisionWalk = true;
    //            isDecidingMovement = false;
    //            ResetPlayerMovementDelay();
    //        }

    //        if (isDecisionRun || isDecisionWalk) {
    //            if (movement != null) {
    //                movement.StopAction();
    //            }
    //            if (isDecisionRun) {
    //                movement = new RunMovement(character.GetMyAnimator(), runSpeed);
    //            }
    //            else if (isDecisionWalk) {
    //                movement = new WalkMovement(character.GetMyAnimator(), walkSpeed);
    //            }
    //            movement.MakeAction();
    //            if (movement != null) {
    //                ToggleIsMoving(true);
    //            }
    //        }
    //    }
        
    ////}

    //private void ResetPlayerMovementDelay() {
    //    playerMovementDelay = 0.2f;
//    //}
//}
