using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities;

public abstract class CharAbility : MonoBehaviour {
    public CharAbilityAction MyActionPrefab;
    protected Character myCharacter;
    protected Vector2 myRange;
    protected float countDownToFollowThrough, countDownToComplete;
    public float TimeToComplete, InterruptDelay, FollowThroughDelay;
    protected bool isInUse, followingThrough;
    public Sprite myIcon;
    public string myName;
	
	void Update () {
		if(isInUse) {
            StartUsingAbility();
            CheckToStopUsingAbility();
        }
	}

    public virtual void InitialiseMe(Character character) {
        SetMyCharacter(character);
    }

    public void UseAbility() {
        countDownToComplete = TimeToComplete;
        countDownToFollowThrough = FollowThroughDelay;
        isInUse = true; //Update will detect this is true and run the StartUsingAbility function
        CharAbilityAction myAction = Instantiate(MyActionPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as CharAbilityAction;
        myAction.transform.SetParent(myCharacter.transform, false);
        myAction.MyAnimator = myCharacter.GetMyAnimator();
        myAction.MakeAction(); //The animation will begin
    }

    protected void StartUsingAbility() {
        if (followingThrough != true) {
            if (countDownToFollowThrough <= 0) {
                FollowThroughAbility();
                followingThrough = true;
            }
            else {
                countDownToFollowThrough -= Time.deltaTime;
            }
        }
    }

    protected void CheckToStopUsingAbility() {
        if(countDownToComplete <= 0) {
            StopAbility();
        } else {
            countDownToComplete -= Time.deltaTime;
        }
    }

    public void SetCharAction(CharAbilityAction action) {
        MyActionPrefab = action;
    }

    public abstract void FollowThroughAbility();

    public void StopAbility() {
        isInUse = false;
        countDownToFollowThrough = FollowThroughDelay;
        Destroy(gameObject); 
    }

    protected void InterruptAbility() {
        if (!followingThrough) {
            MyActionPrefab.InterruptAction();
            countDownToFollowThrough = FollowThroughDelay;
        }
        
    }

    public void SetMyCharacter(Character character) {
        myCharacter = character;
    }

    public abstract void SetMyRange();

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
