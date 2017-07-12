using UnityEngine;

public class WalkMovement : CharacterMovement {

    public override void MakeAction() {
        if (!MyAnimator.GetBool("isWalking")) {
            MyAnimator.SetBool("isWalking", true);
        }
    }

    public override void StopAction() {
        print("stopping walking ");
        MyAnimator.SetBool("isWalking", false);
        Destroy(gameObject);
    }
}
