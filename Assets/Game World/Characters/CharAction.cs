using UnityEngine;

public abstract class CharAction {
    protected Animator animator;
    public CharAction (Animator anim) {
        animator = anim;
    }

    public abstract void MakeAction();

}
