﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
using System;
using GameUtilities;

public class PlayerCombatController : CharCombatController {

    public GameObject selectedEnemy;
    CombatUI combatui;
    PlayerCharacter character;
    CombatStateAction combatStateAction;

    void Awake() {
        combatui = FindObjectOfType<CombatUI>();
        character = FindObjectOfType<PlayerCharacter>();
    }

    public void setSelectedEnemy(GameObject enemy) {
        selectedEnemy = enemy;
    }

    public override void TriggerCombat(Character charIn) {
        AddToEnemyList(charIn);
        ToggleInCombat(true);
        combatStateAction.MakeAction();
        character.GetMovementController().StopMoving();
    }

    public override void GetHit() {
        //throw new NotImplementedException();
    }

    public override void EndCombat(Character charIn) {
        print("ending combat...");
        if (CharacterEnemyList.Contains(charIn)) {
            RemoveFromEnemyList(charIn);
            if (CharacterEnemyList.Count < 1) {
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
                currentTarget = characterSelected;
                if (IsCurrentTargetInWeaponRange()) {
                    character.playerStatus = PlayerCharacter.PlayerStatus.movingToWeaponRange;
                    character.GetMovementController().ProcessMovement();
                }
            }
        } else {
            character.MakeMovementDecision();
        }
    }

    public override Vector2 GetWeaponReachXY() {
        throw new NotImplementedException();
    }

    public bool IsCharacterEnemy(Character characterIn) {
        return characterIn.GetCombatController().IsAttacking(character);
    }

    void AttemptStrikeToSelectedEnemy() {
        //if (distanceX > strikeRangeX || distanceY > strikeRangeY) {
        //    print("Enemy too far away");
        //}
        //else {
            //print("attempting strike");
            //Vector2 enemyPosition = selectedEnemy.GetComponent<Transform>().position;
            //SetMyDirection(enemyPosition, myCurrentPosition);
            //SetDistanceFromNewPosition(enemyPosition, myCurrentPosition);
            //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            ////dialogueUI.SetInUse();
            ////dialogueUI.SetRandomVocab();
        //}
    }

    public void StrikeSelectedEnemy() {
        //print("striking enemy");
        //animToggles.Strike();
        //selectedEnemy.GetComponent<Character>().SetUnhovered();
        //selectedEnemy = null;
    }
}