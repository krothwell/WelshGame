using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
using GameUtilities;

public class GameWorldCharacterSelector : GameWorldSelector {
    CombatUI combatUI;
    void Start() {
        combatUI = FindObjectOfType<CombatUI>();
    }
    public override void DisplayCircle() {
        CharAbility abilitySelected = combatUI.GetCurrentAbility();
        if (abilitySelected != null) {
            if(abilitySelected.IsInRangeOfCharacter(GetComponent<Character>())) {
                BuildCircle();
            } else {
                DisplayOutOfRange();
            }
        } else {
            BuildCircle();
        }

    }

    public void DisplayOutOfRange() {
        print("out of range");
    }
}
