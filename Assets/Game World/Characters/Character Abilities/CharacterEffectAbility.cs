using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterEffectAbility : CharAbility {
    protected Character targetCharacter;

    public void SetTargetCharacter(Character characterTarget) {
        targetCharacter = characterTarget;
        myCharacter.GetMovementController().SetMyDirection(targetCharacter.GetMyPosition(), myCharacter.GetMyPosition());
    }

    public override void InitialiseMe(Character mCharacter, string nameStr, Sprite iconSprite) {
        base.InitialiseMe(mCharacter, nameStr, iconSprite);
        SetCharAction(new StrikeAbilityAction(myCharacter.GetMyAnimator()));
        SetMyRange();
    }
}
