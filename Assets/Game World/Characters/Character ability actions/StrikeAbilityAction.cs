using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeAbilityAction : CharAbilityAction {

    public StrikeAbilityAction(Animator anim) : base (anim) { }

    public override void MakeAction() {
        animator.SetTrigger("Strike");
    }
}
