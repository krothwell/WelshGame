using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatStrategyDecisionAction : CombatStrategyAction {
    protected CharacterDecision myDecision;

	public override void DoAction() {
        myCharacter.MyDecision = myDecision;
        myDecision.GetComponent<CharacterDecision>().ProcessDecision();
    }
}
