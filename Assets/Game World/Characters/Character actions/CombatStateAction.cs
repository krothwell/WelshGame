using UnityEngine;

public class CombatStateAction : CharActionContinuous {

	public CombatStateAction(Animator anim) : base(anim) {
    }

    public override void MakeAction() {
        animator.SetBool("isFighting", true);
    }

    public override void StopAction() {
        animator.SetBool("isFighting", false);
    }
}
