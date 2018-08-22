using UnityEngine;
using System;
using System.Collections;
using GameUI;
using UnityUtilities;
/// <summary>
/// This class is responsible for controlling the player’s character by reacting
/// to input from the player and game environment.  
/// </summary>
public class PlayerCharacter : Character {
    public MusicZone CurrentMusicZone;
    public GameObject SelectionCirclePrefab;
    public Sprite backUpPlayerPortrait; //if the character portrait is null then this is used.
    public GameObject currentObjectInteractingWith;
    private GameWorldSelector currentSelection;

    void Awake() {
        combatController = GetComponent<CharCombatController>();
        InitialiseMe();
        dialogueUI = FindObjectOfType<DialogueUI>();
    }

    public void SetMyName(string name) {
        CharacterName = name;
    }

    public string GetMyName() {
        return CharacterName;
    }

    public Sprite GetPlayerPortrait() {
        if (GetMyPortrait() == null) {
            return backUpPlayerPortrait;
        }
        else {
            return GetMyPortrait();
        }
    }

    public void SetCurrentSelection(GameWorldSelector selection) {
        currentSelection = selection;
    }

    public GameWorldSelector GetCurrentSelection() {
        return currentSelection;
    }

    public override void EndSelection() {
        if (currentSelection != null) {
            currentSelection.EndCurrentSelection();
            currentSelection = null;
        }
        currentObjectInteractingWith = null;
        GetCombatController().SetCurrentEnemyTarget(null);
    }


}


