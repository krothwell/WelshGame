using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStrategyMoveDecisionAction : CombatStrategyDecisionAction {
    public override void InitialiseAction(Character charIn) {
        myCharacter = charIn;
        print(myAction);
        myDecision = myAction.GetComponent<CharacterDecision>();
        print(myDecision);
        myDecision.InitialiseMe(myCharacter);
        CharacterMovementDecision myMovementDecision = (CharacterMovementDecision)myDecision;
        myMovementDecision.InitialiseMe(myCharacter.GetCombatController().CurrentEnemyTarget.transform, true);
    }
}
