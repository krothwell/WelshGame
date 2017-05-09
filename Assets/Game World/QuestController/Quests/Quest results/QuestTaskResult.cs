using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestTaskResult : MonoBehaviour {

    public abstract void ActivateResult();

    public virtual void InitialiseMe() {
        ActivateResult();
        Destroy(gameObject);
        Destroy(this);
    }


}
