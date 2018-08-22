using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombatController : CharCombatController {
    public GameObject MyCombatStrategyPrefab;
    private CombatStrategy myCombatStrategy;
    protected NPCWeaponItem myWeapon;

    void Start() {
        myWeapon = GetComponent<NPCWeaponItem>();
    }

    void OnTriggerEnter2D(Collider2D inCollider) {
        if (inCollider.transform.name == "Perimeter") {
            PlayerCharacter playerCharacter = inCollider.transform.parent.GetComponent<PlayerCharacter>();
            if (playerCharacter != null) {
                TriggerCombat(playerCharacter);
            }
        }
    }


    void OnTriggerExit2D (Collider2D outCollider) {
        if (outCollider.transform.name == "Perimeter") {
            PlayerCharacter playerCharacter = outCollider.transform.parent.GetComponent<PlayerCharacter>();
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

    public override void GetHit(WorldDamage damage) {
        DeductHealth(damage);
        if(Health<= 0 ) {
            SetDefeated();
        }
    }


    public override WorldDamage GetWeaponDamage() {
        WorldDamage wd = new WorldDamage();
        wd.BaseWeaponDamage = myWeapon.BaseDamage;
        return wd;
    }

    public override Vector2 GetWeaponReachXY() {
        return myWeapon.GetWeaponRange();
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
