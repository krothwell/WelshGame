using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CombatStrategyAbilityAction : CombatStrategyAction {
    protected CharAbility myAbility;

    public override void DoAction() {
        print(myCharacter);
        //myCharacter.GetCombatController().SetSelectedAbility(myAbility.gameObject);
        myAbility.GetComponent<CharAbility>().UseAbility();
    }

    public new void EndAction() { 
        myAbility.StopAbility();
        base.EndAction();
    }
}