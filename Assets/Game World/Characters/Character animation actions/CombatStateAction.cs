using UnityEngine;
using GameUtilities;

public class CombatStateAction : CharActionContinuous {

    public override void MakeAction() {
        AnimationUtilities.SetParameterIfExists("isFighting", MyAnimator, true);
    }

    public override void StopAction() {
        AnimationUtilities.SetParameterIfExists("isFighting", MyAnimator, false);
        Destroy(gameObject);
    }
}
