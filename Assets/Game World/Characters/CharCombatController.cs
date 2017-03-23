using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities;

public abstract class CharCombatController : MonoBehaviour, IAttackable {
    protected bool isInCombat;
    protected Animator myAnimator;
    public Character CurrentTarget;
    protected Character myCharacter;
    public List<Character> CharacterEnemyList;
    public float BaseWeaponReach = 0.1f;
    private GameObject selectedAbility;

    // Use this for initialization

    public bool IsAttacking(Character characterIn) {
        return (CurrentTarget == characterIn) ? true : false;
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
        Vector2 distanceXYfromCharacter = World.GetVector2DistanceFromPositions2D(myCharacter.GetMyPosition(), CurrentTarget.transform.position);
        print(distanceXYfromCharacter);
        Vector2 weaponReachXY = GetWeaponReachXY();
        return (distanceXYfromCharacter.x < weaponReachXY.x && distanceXYfromCharacter.y < weaponReachXY.y);
    }

    public abstract Vector2 GetWeaponReachXY();


    public void SetCurrentTarget(Character charTarget) {
        CurrentTarget = charTarget;
    }

    public Character GetCurrentTarget() {
        return CurrentTarget;
    }

    public abstract void TriggerCombat(Character charIn);

    public abstract void TriggerStrategyMode();

    public abstract void EndCombat(Character charIn);

    public abstract void GetHit();

    public abstract void ProcessAction();

    protected void AddToEnemyList(Character charIn) {
        CharacterEnemyList.Add(charIn);
    }

    protected void RemoveFromEnemyList(Character charIn) {
        CharacterEnemyList.Remove(charIn);
    }

    public void SetSelectedAbility (GameObject ability) {
        selectedAbility = ability;
        ability.transform.SetParent(transform, false);
    }
}

