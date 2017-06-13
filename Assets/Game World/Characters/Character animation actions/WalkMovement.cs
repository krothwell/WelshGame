using UnityEngine;

public class WalkMovement : CharacterMovement {

    public override void MakeAction() {
        MyAnimator.SetBool("isWalking", true);
    }

    public override void StopAction() {
        MyAnimator.SetBool("isWalking", false);
        Destroy(gameObject);
    }
}
