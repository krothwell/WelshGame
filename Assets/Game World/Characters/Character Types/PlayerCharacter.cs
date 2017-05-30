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
        ((PlayerCombatController)combatController).SetCombatStateAction();
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



    public void DestroySelectionCircleOfInteractiveObject() {
        if (currentObjectInteractingWith != null) {
            if (currentObjectInteractingWith.GetComponent<GameWorldSelector>() != null) {
                currentObjectInteractingWith.GetComponent<GameWorldSelector>().EndCurrentSelection();
            }
            currentObjectInteractingWith = null;
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
        }
        currentObjectInteractingWith = null;
        GetCombatController().SetCurrentEnemyTarget(null);
    }


}


