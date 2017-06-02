using UnityEngine;
using GameUtilities;

public class CombatStateAction : CharActionContinuous {

	public CombatStateAction(Animator anim) : base(anim) {
    }

    public override void MakeAction() {
        AnimationUtilities.SetParameterIfExists("isFighting", animator, true);
    }

    public override void StopAction() {
        AnimationUtilities.SetParameterIfExists("isFighting", animator, false);
    }
}
