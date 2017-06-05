using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPCCharacter : Character {

    private Vector2 startingPosition;
    public Vector2 StartingPosition {
        get { return startingPosition; }
        set { startingPosition = value; }
    }

    public override void EndSelection() {
        MyDecision = null;
    }

    new void Start() {
        base.Start();
        startingPosition = transform.localPosition;

    }

    void Awake() {
        combatController = GetComponentInChildren<NPCCombatController>();
        InitialiseMe();
    }


}
