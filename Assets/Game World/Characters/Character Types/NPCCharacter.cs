using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPCCharacter : Character {
    private GameWorldSelector mySelector;
    public GameWorldSelector MySelector {
        get { return mySelector; }
        set { mySelector = value; }
    }
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
        mySelector = GetComponent<GameWorldSelector>();

    }

    void Awake() {
        combatController = GetComponentInChildren<NPCCombatController>();
        InitialiseMe();
    }


}
