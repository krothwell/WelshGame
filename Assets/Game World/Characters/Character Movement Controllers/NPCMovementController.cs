using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : CharMovementController {
    public float WalkSpeed;
    public float RunSpeed;
    NPCCloseCombatController combatController;
    PlayerCharacter playerCharacter;
	// Use this for initialization
	void Awake () {
        playerCharacter = FindObjectOfType<PlayerCharacter>();
	}



    public void SetMyCombatController (NPCCloseCombatController cc) {
        combatController = cc;
    }

    public override void ProcessMovement() {
        if (!isMoving) {
            //Decide to walk or run... should be determined by if npc is attacking player, and if so , the distance from the player
            if (combatController.IsInCombat()) {
                if (movement != null) {
                    movement.StopAction();
                }
                if (GetDistanceFromMyPosition(playerCharacter.GetMyPosition()) < 1f) {
                    movement = new WalkMovement(character.GetMyAnimator(), WalkSpeed);

                }
                else {
                    movement = new RunMovement(character.GetMyAnimator(), RunSpeed);
                }
                movement.MakeAction();
                ToggleIsMoving(true);
                SetTargetPosition(combatController.GetCombatSlotPosition());
            }
            else {
                //not sure what else yet
            }
        }
    }

    public override void DecideMovement() {
        throw new NotImplementedException();
    }
}
