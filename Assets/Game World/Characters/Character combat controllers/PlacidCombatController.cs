using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacidCombatController : CharCombatController {

    void OnTriggerEnter2D(Collider2D incursionCollider) {
        if (incursionCollider.transform.name == "Perimeter") {
            PlayerCharacter playerCharacter = incursionCollider.transform.parent.GetComponent<PlayerCharacter>();
            if (playerCharacter != null) {
                TriggerCombat(playerCharacter);
            }
        }
    }


    void OnTriggerExit2D (Collider2D incursionCollider) {
        if (incursionCollider.transform.name == "Perimeter") {
            PlayerCharacter playerCharacter = incursionCollider.transform.parent.GetComponent<PlayerCharacter>();
            if (playerCharacter != null) {
                EndCombat(playerCharacter);
            }
        }
    }

    public override void TriggerCombat(Character charIn) {
        charIn.GetCombatController().TriggerCombat(myCharacter);
    }

    public override void GetHit() {
        print(transform.parent + ": I got hit!");

    }

    public override Vector2 GetWeaponReachXY() {
        throw new NotImplementedException();
    }

    public override void EndCombat(Character charIn) {
        charIn.GetCombatController().EndCombat(myCharacter);
    }

    public override void ProcessAction() {

    }
}
