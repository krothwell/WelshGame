using UnityEngine;

public abstract class CharAction:MonoBehaviour {
    Animator myAnimator;
    public Animator MyAnimator {
        get { return myAnimator; }
        set { myAnimator = value; }
    }

    public abstract void MakeAction();

}
