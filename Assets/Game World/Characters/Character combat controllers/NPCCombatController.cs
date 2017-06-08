using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombatController : CharCombatController {
    public GameObject MyCombatStrategyPrefab;
    private CombatStrategy myCombatStrategy;

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
        CurrentEnemyTarget = charIn;
        charIn.GetCombatController().TriggerCombat(myCharacter);
        if (MyCombatStrategyPrefab != null) {
            GameObject combatStrategy = Instantiate(MyCombatStrategyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            print(combatStrategy);
            combatStrategy.transform.SetParent(myCharacter.transform, false);
            myCombatStrategy = combatStrategy.GetComponent<CombatStrategy>();
            myCombatStrategy.MyCharacter = myCharacter;
            myCombatStrategy.StartNextAction();
        }
    }

    public override void GetHit() {
        print(transform.parent + ": I got hit!");

    }

    public override Vector2 GetWeaponReachXY() {
        float xRange = BaseWeaponReach, yRange = BaseWeaponReach / 2.5f;
        Vector2 weaponReach = new Vector2(xRange, yRange);
        return  weaponReach;
    }

    public override void EndCombat(Character charIn) {
        charIn.GetCombatController().EndCombat(myCharacter);
        if (myCombatStrategy != null) {
            myCombatStrategy.DestroyMe();
        }
        myCharacter.SetDefaultAnimation();
        CurrentEnemyTarget = null;
    }
}
