using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterEffectAbility : CharAbility {
    protected Character targetCharacter;

    public void SetTargetCharacter(Character character) {
        targetCharacter = character;
    }

    public void SetTargetCharacter() {
        targetCharacter = myCharacter.GetCombatController().GetCurrentEnemyTarget();
    }

    public override void InitialiseMe(Character mCharacter) {
        SetMyCharacter(mCharacter);
        SetCharAction(new StrikeAbilityAction(myCharacter.GetMyAnimator()));
        SetMyRange();
    }
}
