using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities;

public abstract class CharAbility : MonoBehaviour {
    protected CharAbilityAction myAction;
    protected Character myCharacter;
    protected Vector2 myRange;
    protected float countDownToFollowThrough;
    public float TimeToComplete, InterruptDelay;
    protected bool isInUse, followingThrough;
	
	void Update () {
		if(isInUse) {
            StartUsingAbility();
        }
	}

    public abstract void InitialiseMe(Character character);

    public void UseAbility() {
        countDownToFollowThrough = TimeToComplete;
        isInUse = true; //Update will detect this is true and run the StartUsingAbility function
        myAction.MakeAction(); //The animation will begin
    }

    protected void StartUsingAbility() {
        if (countDownToFollowThrough <= 0) {
            FollowThroughAbility();
            followingThrough = true;
            StopAbility();
        }
        else {
            countDownToFollowThrough -= Time.deltaTime;
        }
    }

    public void SetCharAction(CharAbilityAction action) {
        myAction = action;
    }

    public abstract void FollowThroughAbility();

    protected void StopAbility() {
        isInUse = false;
        countDownToFollowThrough = TimeToComplete;
        Destroy(gameObject); 
    }

    protected void InterruptAbility() {
        if (!followingThrough) {
            myAction.InterruptAction();
            countDownToFollowThrough = TimeToComplete;
        }
        
    }

    public void SetMyCharacter(Character character) {
        myCharacter = character;
    }

    public abstract void SetMyRange();

    public Vector2 GetMyRange() {
        return myRange;
    }

    public bool IsInRangeOfCharacter(Character character) {
        Vector2 distanceXYfromCharacter = World.GetVector2DistanceFromPositions2D(myCharacter.GetMyPosition(), character.GetMyPosition());
        print(distanceXYfromCharacter + " (disance from characters) " + GetMyRange() + " (ability range)");
        bool inRange = (distanceXYfromCharacter.x <= GetMyRange().x && distanceXYfromCharacter.y <= GetMyRange().y) ? true : false;
        return inRange;
    }
}
