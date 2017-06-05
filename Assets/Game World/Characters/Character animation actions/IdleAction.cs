using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities;

public class IdleAction : CharAction {


    public override void MakeAction() {
        AnimationUtilities.SetTriggerIfExists("Idle", MyAnimator);
        Destroy(gameObject);
    }
}
