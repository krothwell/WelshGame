using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharActionContinuous : CharAction {

    public CharActionContinuous(Animator anim):base(anim) {
    }

    public abstract void StopAction();
}
