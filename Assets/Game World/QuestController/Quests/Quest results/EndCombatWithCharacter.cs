using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCombatWithCharacter : QuestTaskResult {
    NPCs npcsController;
    string charName;
    public override void ActivateResult() {
        Character characterNPC = npcsController.GetCharacterFromName(charName);
        characterNPC.GetCombatController().gameObject.SetActive(false);
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        player.GetCombatController().EndCombat(characterNPC);
    }

    public void InitialiseMe(string charNameIn) {
        charName = charNameIn;
        base.InitialiseMe();
    }
}
