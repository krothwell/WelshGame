using UnityEngine;

public class WalkMovement : CharacterMovement {
    private float walkSpeed;

    public WalkMovement(Animator anim, float walkSpd) : base(anim, walkSpd) {
    }

    public override void MakeAction() {
        animator.SetBool("isWalking", true);
    }

    public override void StopAction() {
        animator.SetBool("isWalking", false);
    }
}
