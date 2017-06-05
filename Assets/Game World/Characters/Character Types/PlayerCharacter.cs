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
    public GameObject SelectionCirclePrefab;
    public Sprite backUpPlayerPortrait; //if the character portrait is null then this is used.
    public enum PlayerStatus {
        passive,
        movingToLocation,
        movingToObject,
        movingToCharacter,
        movingToWeaponRange,
        speakingToCharacter
    }
    public PlayerStatus playerStatus;
    public GameObject currentObjectInteractingWith;
    private GameWorldSelector currentSelection;

    void Awake() {
        combatController = GetComponent<CharCombatController>();
        InitialiseMe();
        playerStatus = PlayerStatus.passive;
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
        print("ending selection");
        if (currentSelection != null) {
            currentSelection.EndCurrentSelection();
            currentSelection = null;
        }
        currentObjectInteractingWith = null;
        GetCombatController().SetCurrentEnemyTarget(null);
    }


}


