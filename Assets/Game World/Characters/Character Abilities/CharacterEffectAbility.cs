using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterEffectAbility : CharAbility {
    protected Character targetCharacter;

    public void SetTargetCharacter(Character characterTarget) {
        targetCharacter = characterTarget;
        print(characterTarget);
        //myCharacter.GetCombatController().SetCurrentEnemyTarget(characterTarget);
        //myCharacter.MovementController.SetMyDirection(targetCharacter.GetMyPosition(), myCharacter.GetMyPosition());
    }

    public override void InitialiseMe(Character mCharacter) {
        base.InitialiseMe(mCharacter);
        SetMyRange();
    }

    public void EndSelection() {
        targetCharacter.EndSelection();
    }
}
