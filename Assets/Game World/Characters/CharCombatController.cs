﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities;

public abstract class CharCombatController : MonoBehaviour, IAttackable {
    protected bool isInCombat;
    protected Animator myAnimator;
    public Character currentTarget;
    protected Character myCharacter;

    public List<Character> CharacterEnemyList;

    // Use this for initialization

    public bool IsAttacking(Character characterIn) {
        return (currentTarget == characterIn) ? true : false;
    }

    public void ToggleInCombat(bool active) {
        isInCombat = active;
    }
    
    public bool IsInCombat() {
        return isInCombat;
    }

    public void SetAnimator(Animator anim) {
        myAnimator = anim;
    }

    public void SetCharacter(Character charIn) {
        myCharacter = charIn;
    }

    public bool IsCurrentTargetInWeaponRange() {
        Vector2 distanceXYfromCharacter = World.GetVector2DistanceFromPositions2D(myCharacter.GetMyPosition(), currentTarget.transform.position);
        Vector2 weaponReachXY = GetWeaponReachXY();
        return (distanceXYfromCharacter.x > weaponReachXY.x || distanceXYfromCharacter.y > weaponReachXY.y);
    }

    public abstract Vector2 GetWeaponReachXY();


    public void SetCurrentTarget(Character charTarget) {
        currentTarget = charTarget;
    }

    public abstract void TriggerCombat(Character charIn);

    public abstract void EndCombat(Character charIn);

    public abstract void GetHit();

    public abstract void ProcessAction();

    protected void AddToEnemyList(Character charIn) {
        CharacterEnemyList.Add(charIn);
    }

    protected void RemoveFromEnemyList(Character charIn) {
        CharacterEnemyList.Remove(charIn);
    }
}