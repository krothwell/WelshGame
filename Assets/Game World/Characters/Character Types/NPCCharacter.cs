using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPCCharacter : Character {

    public override void EndSelection() {
        MyDecision = null;
    }

    void Awake() {
        combatController = GetComponentInChildren<NPCCombatController>();
        InitialiseMe();
    }


}
