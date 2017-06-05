using UnityEngine;

public class WalkMovement : CharacterMovement {
    private float walkSpeed;

    public override void MakeAction() {
        MyAnimator.SetBool("isWalking", true);
    }

    public override void StopAction() {
        MyAnimator.SetBool("isWalking", false);
        Destroy(gameObject);
    }
}
