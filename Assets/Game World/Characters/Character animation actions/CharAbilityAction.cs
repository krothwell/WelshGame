using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharAbilityAction : CharActionContinuous {

    public override void StopAction() {
        MyAnimator.SetTrigger("Stopped");
        Destroy(gameObject);
    }

    public void InterruptAction() {
        MyAnimator.SetTrigger("Interrupted");
    }
}
