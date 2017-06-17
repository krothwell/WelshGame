using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : CharMovementController {
    public float WalkSpeed;
    public float RunSpeed;
    //PlayerCharacter playerCharacter;
    private NPCCombatController npcCombatController;
    public NPCCombatController NpcCombatController {
        get { return npcCombatController; }
        set { npcCombatController = value; }
    }
    // Use this for initialization
    void Awake () {
        //playerCharacter = FindObjectOfType<PlayerCharacter>();
	}

    //public void ProcessMovement() {
    //    if (!isMoving) {
    //        //Decide to walk or run... should be determined by if npc is attacking player, and if so , the distance from the player
    //        if (combatController.IsInCombat()) {
    //            if (movement != null) {
    //                movement.StopAction();
    //            }
    //            if (GetDistanceFromMyPosition(playerCharacter.GetMyPosition()) < 1f) {
    //                movement = new WalkMovement(character.GetMyAnimator(), WalkSpeed);

    //            }
    //            else {
    //                movement = new RunMovement(character.GetMyAnimator(), RunSpeed);
    //            }
    //            movement.MakeAction();
    //            ToggleIsMoving(true);
    //            SetTargetPosition(combatController.GetCombatSlotPosition());
    //        }
    //        else {
    //            //not sure what else yet
    //        }
    //    }
    //}

    //public void DecideMovement() {
    //    throw new NotImplementedException();
    //}
}
