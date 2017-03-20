using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharAbility : MonoBehaviour {
    CharAbilityAction myAction;
    protected float countDownToFollowThrough;
    public float TimeToComplete, InterruptDelay;
    protected bool isInUse;
	
	void Update () {
		if(isInUse) {
            StartUsingAbility();
        }
	}

    public void UseAbility() {
        countDownToFollowThrough = TimeToComplete;
        isInUse = true; //Update will detect this is true and run the StartUsingAbility function
        myAction.MakeAction(); //The animation will begin
    }

    protected void StartUsingAbility() {
        if (countDownToFollowThrough <= 0) {
            FollowThroughAbility();
        } else {
            countDownToFollowThrough -= Time.deltaTime;
        }
    }

    public void SetCharAction(CharAction action) {
        myAction = action;
    }

    public abstract void FollowThroughAbility();

    protected void StopAbility() {
        isInUse = false;
        countDownToFollowThrough = TimeToComplete; 
        myAction
    }
}
