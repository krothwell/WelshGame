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
    CombatStateAction combatStateAction;

    void Awake() {
        combatui = FindObjectOfType<CombatUI>();
        character = FindObjectOfType<PlayerCharacter>();
        inventory = FindObjectOfType<PlayerInventoryUI>();
    }

    public void setSelectedEnemy(GameObject enemy) {
        selectedEnemy = enemy;
    }

    public override void TriggerCombat(Character charIn) {
        AddToEnemyList(charIn);
        ToggleInCombat(true);
        combatStateAction.MakeAction();
        TriggerStrategyMode();
    }

    public override void TriggerStrategyMode() {
        combatui.ToggleCombatUI();
    }

    public override void GetHit() {
        //throw new NotImplementedException();
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


    public void SetCombatStateAction() {
        combatStateAction = new CombatStateAction(myAnimator);
    }

    public override void ProcessAction() {
        GameObject objSelected = character.GetObjSelectedByPlayer();
        if (objSelected.GetComponent<Character>() != null) {
            Character characterSelected = objSelected.GetComponent<Character>();
            if (characterSelected.GetCombatController() != null) {
                CurrentEnemyTarget = characterSelected;
                if (IsCurrentTargetInWeaponRange()) {
                    combatui.ToggleCombatUI();
                } else { 
                    character.playerStatus = PlayerCharacter.PlayerStatus.movingToWeaponRange;
                    character.GetMovementController().ProcessMovement();
                }
            }
        } else {
            //character.MakeMovementDecision();
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
        print(reach);
        return reach;
        //return weaponRange + BaseWeaponReach;
    }

    public bool IsCharacterEnemy(Character characterIn) {
        return characterIn.GetCombatController().IsAttacking(character);
    }
}
