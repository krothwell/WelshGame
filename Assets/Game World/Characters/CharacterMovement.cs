using UnityEngine;

public abstract class CharacterMovement:CharActionContinuous {
    protected float movementSpeed;
    public CharacterMovement(Animator anim, float speed):base(anim) {
        SetMovementSpeed(speed);
    }

    public float GetMovementSpeed() {
        return movementSpeed;
    }

    public void SetMovementSpeed(float speed) {
        movementSpeed = speed;
    }

}
