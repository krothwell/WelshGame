using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities;

public class SitAction : CharActionContinuous {

    public override void MakeAction() {
        AnimationUtilities.SetParameterIfExists("isSitting", MyAnimator, true);
    }

    public override void StopAction() {
        AnimationUtilities.SetParameterIfExists("isSitting", MyAnimator, false);
        Destroy(gameObject);
    }
}
