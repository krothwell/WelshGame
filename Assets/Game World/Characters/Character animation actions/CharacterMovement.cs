using UnityEngine;

public abstract class CharacterMovement:CharActionContinuous {
    protected float movementSpeed;

    public float GetMovementSpeed() {
        return movementSpeed;
    }

    public void SetMovementSpeed(float speed) {
        movementSpeed = speed;
    }

}
