using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities;

public abstract class CharCombatController : MonoBehaviour, IAttackable {
    protected float health;
    public float Health {
        get { return health; }
        set { health = value; }
    }
    public float BaseHealth;
    protected bool isInCombat;
    protected Animator myAnimator;
    public Character CurrentEnemyTarget;
    protected Character myCharacter;
    public List<Character> CharacterEnemyList;
    private GameObject selectedAbility;

    // Use this for initialization

    protected void Awake() {
        if (health == 0) {
            health = BaseHealth;
        }
    }

    public bool IsAttacking(Character characterIn) {
        return (CurrentEnemyTarget == characterIn) ? true : false;
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
        //print(myCharacter);
        //print(CurrentEnemyTarget);
        if (CurrentEnemyTarget) {
            Vector2 distanceXYfromCharacter = World.GetVector2DistanceFromPositions2D(myCharacter.GetMyPosition(), CurrentEnemyTarget.transform.position);
            //print(distanceXYfromCharacter);
            Vector2 weaponReachXY = GetWeaponReachXY();
            return (distanceXYfromCharacter.x < weaponReachXY.x && distanceXYfromCharacter.y < weaponReachXY.y);
        } else {
            return false;
        }
    }

    public abstract Vector2 GetWeaponReachXY();


    public void SetCurrentEnemyTarget(Character charTarget) {
        CurrentEnemyTarget = charTarget;
    }

    public Character GetCurrentEnemyTarget() {
        return CurrentEnemyTarget;
    }

    public abstract void TriggerCombat(Character charIn);

    public abstract void EndCombat(Character charIn);

    public abstract void GetHit(WorldDamage damage);
    public void DeductHealth(WorldDamage damage) {
        Health -= damage.CalculateDamage();
    }
    protected void AddToEnemyList(Character charIn) {
        CharacterEnemyList.Add(charIn);
    }

    protected void RemoveFromEnemyList(Character charIn) {
        CharacterEnemyList.Remove(charIn);
    }

    public void SetSelectedAbility (GameObject ability) {
        selectedAbility = ability;
        //ability.transform.SetParent(transform, false);
    }

    public abstract WorldDamage GetWeaponDamage();
}

