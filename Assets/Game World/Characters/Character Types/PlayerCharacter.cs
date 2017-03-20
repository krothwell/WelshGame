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
    private GameObject objectClickedByPlayer;
    private DialogueUI dialogueUI;

    void Awake() {
        combatController = GetComponent<CharCombatController>();
        InitialiseMe();
        playerStatus = PlayerStatus.passive;
        dialogueUI = FindObjectOfType<DialogueUI>();
        ((PlayerMovementController)movementController).SetCharacter(this);
        ((PlayerCombatController)combatController).SetCombatStateAction();
    }

    // Update is called once per frame
    void Update() {
        MakeDecisionOnPlayerInput();    
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

    public void PickUpObject() {
        currentObjectInteractingWith = objectClickedByPlayer;
        objectClickedByPlayer.GetComponent<WorldItem>().GetPickedUp();
    }

    public void SpeakToCharacter() {
        currentObjectInteractingWith = objectClickedByPlayer;
        dialogueUI.StartNewDialogue(objectClickedByPlayer.GetComponent<Character>());
    }

    public void DestroySelectionCircleOfInteractiveObject() {
        if (currentObjectInteractingWith != null) {
            if (currentObjectInteractingWith.GetComponent<GameWorldObjectSelector>() != null) {
                currentObjectInteractingWith.GetComponent<GameWorldObjectSelector>().DestroyMe();
            }
            currentObjectInteractingWith = null;
        }
    }

    public GameObject GetObjSelectedByPlayer() {
        return objectClickedByPlayer;
    }

    protected void MakeDecisionOnPlayerInput() {
        if (Input.GetMouseButtonUp(0)) {
            MakeDecisionOnPlayerMouseLeftClick();
        }
    }

    protected void MakeDecisionOnPlayerMouseLeftClick() {
        MouseSelection.ClickSelect(out objectClickedByPlayer);
        print(objectClickedByPlayer);
        if (objectClickedByPlayer != null) {
            if (combatController.IsInCombat()) {
                combatController.ProcessAction();
            }
            else {
                MakeMovementDecision();
                print(playerStatus);
            }
        }
    }

    public void MakeMovementDecision() {
        if (objectClickedByPlayer.GetComponent<WorldItem>() != null) {
            playerStatus = PlayerStatus.movingToObject;
        }
        else if (objectClickedByPlayer.GetComponent<Character>() != null) {
            playerStatus = PlayerStatus.movingToCharacter;
        }
        else if (objectClickedByPlayer.GetComponent<CharCombatController>()
          || objectClickedByPlayer.tag == "World") {
            playerStatus = PlayerStatus.movingToLocation;
        }
        if (movementController != null) {
            movementController.ProcessMovement();
        }
    }

}


