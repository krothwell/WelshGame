using UnityEngine;

public class RunMovement:CharacterMovement {
    private float runSpeed;

    public RunMovement(Animator anim, float runSpd) : base(anim, runSpd){
    }

    public override void MakeAction() {
        animator.SetBool("isRunning", true);
    }

    public override void StopAction() {
        animator.SetBool("isRunning", false);
    }
}
