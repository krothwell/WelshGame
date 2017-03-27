using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterEffectAbility : CharAbility {
    protected Character targetCharacter;

    public void SetTargetCharacter(Character characterTarget) {
        targetCharacter = characterTarget;
        myCharacter.GetMovementController().SetMyDirection(targetCharacter.GetMyPosition(), myCharacter.GetMyPosition());
    }

    public override void InitialiseMe(Character mCharacter) {
        SetMyCharacter(mCharacter);
        SetCharAction(new StrikeAbilityAction(myCharacter.GetMyAnimator()));
        SetMyRange();
    }
}
