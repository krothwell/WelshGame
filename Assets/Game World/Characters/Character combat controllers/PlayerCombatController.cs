using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
using System;
using GameUtilities;

public class PlayerCombatController : CharCombatController {
    PlayerInventoryUI inventory;
    public GameObject selectedEnemy;
    CombatUI combatui;
    PlayerCharacter character;
    public CombatStateAction CombatStateActionPrefab;
    private CombatStateAction combatStateAction;

    void Awake() {
        combatui = FindObjectOfType<CombatUI>();
        character = FindObjectOfType<PlayerCharacter>();
        inventory = FindObjectOfType<PlayerInventoryUI>();
        
        
    }

    public void setSelectedEnemy(GameObject enemy) {
        selectedEnemy = enemy;
    }

    public override void TriggerCombat(Character charIn) {
        combatStateAction = Instantiate(CombatStateActionPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as CombatStateAction;
        combatStateAction.transform.SetParent(myCharacter.transform, false);
        combatStateAction.MyAnimator = GetComponent<Animator>();
        AddToEnemyList(charIn);
        ToggleInCombat(true);
        combatStateAction.MakeAction();
        TriggerStrategyMode();
    }

    public void TriggerStrategyMode() {
        combatui.ToggleCombatUI();
    }

    public override void GetHit() {
        print("Player hit!");
    }

    public override void EndCombat(Character charIn) {
        print("ending combat...");
        if (CharacterEnemyList.Contains(charIn)) {
            RemoveFromEnemyList(charIn);
            if (CharacterEnemyList.Count < 1) {
                ToggleInCombat(false);
                combatStateAction.StopAction();
            }
        }
    }

    public override Vector2 GetWeaponReachXY() {
        WeaponItem weapon = (inventory.GetItemFromEquippedDict(WorldItems.WorldItemTypes.WeaponWearable) as WeaponItem);
        float xRange = BaseWeaponReach, yRange = BaseWeaponReach/2.5f;
        if (weapon != null) {
            Vector2 weaponRange = (weapon.GetWeaponRange());
            xRange += weaponRange.x;
            yRange += weaponRange.y;
        }
        Vector2 reach = new Vector2(xRange, yRange);
        //print(reach);
        return reach;
        //return weaponRange + BaseWeaponReach;
    }

    public bool IsCharacterEnemy(Character characterIn) {
        return characterIn.GetCombatController().IsAttacking(character);
    }
}
