using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharAbilityAction : CharActionContinuous {

    public CharAbilityAction(Animator anim) : base(anim) {
    }

    public override void StopAction() {
        animator.SetTrigger("Stopped");
    }

    public void InterruptAction() {
        animator.SetTrigger("Interrupted");
    }
}
