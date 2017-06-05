using UnityEngine;

public class RunMovement:CharacterMovement {
    private float runSpeed;

    public override void MakeAction() {
        MyAnimator.SetBool("isRunning", true);
    }

    public override void StopAction() {
        MyAnimator.SetBool("isRunning", false);
        Destroy(gameObject);
    }
}
