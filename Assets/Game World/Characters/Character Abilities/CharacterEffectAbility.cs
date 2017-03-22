using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterEffectAbility : CharAbility {
    protected Character targetCharacter;

    public void SetMyCharacter(Character character) {
        myCharacter = character;
    }

    public void SetTargetCharacter() {
        targetCharacter = myCharacter.GetCombatController().GetCurrentTarget();
    }
}
