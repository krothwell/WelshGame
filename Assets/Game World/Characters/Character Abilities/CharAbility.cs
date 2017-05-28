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
    protected Sprite myIcon;
    protected string myName;
	
	void Update () {
		if(isInUse) {
            StartUsingAbility();
        }
	}

    public virtual void InitialiseMe(Character character, string nameStr, Sprite spriteIcon) {
        SetMyCharacter(character);
        SetMyName(nameStr);
        SetMyIcon(spriteIcon);
    }

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

    public void SetMyName (string nameStr) {
        myName = nameStr;
    }

    public void SetMyIcon (Sprite icon) {
        myIcon = icon;
    }

    public string GetMyName() {
        return myName;
    }

    public Sprite GetMyIcon() {
        return myIcon;
    }

    public Vector2 GetMyRange() {
        return myRange;
    }

    public bool IsInRangeOfCharacter(Character character) {
        bool inRange;
        Vector2 distanceXYfromCharacter = World.GetVector2DistanceFromPositions2D(myCharacter.GetMyPosition(), character.GetMyPosition());
        print(distanceXYfromCharacter + " (disance from characters) " + GetMyRange() + " (ability range)");
        inRange = (distanceXYfromCharacter.x <= GetMyRange().x && distanceXYfromCharacter.y <= GetMyRange().y) ? true : false;
        return inRange;
    }
}
