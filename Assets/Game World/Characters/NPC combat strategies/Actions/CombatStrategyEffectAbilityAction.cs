using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStrategyEffectAbilityAction : CombatStrategyAbilityAction {

	public override void InitialiseAction(Character characterIn) {
        myCharacter = characterIn;
        myAbility = myAction.GetComponent<CharAbility>();
        CharacterEffectAbility charEffectAbility = (CharacterEffectAbility) myAbility;
        //print(myAbility);
        //print(characterIn);
        charEffectAbility.SetTargetCharacter(myCharacter.GetCombatController().CurrentEnemyTarget);
        charEffectAbility.InitialiseMe(characterIn);


    }
}
