using UnityEngine;
using UnityEditor;
using System;
using GameUI;

/// <summary>
/// Abstract class which provides default controls of all derived character
/// types such as the player and NPCs. 
/// </summary>
public abstract class Character : MonoBehaviour {

    //named in editor, this easily allows setting a new portrait and name to new npcs. It also allows
    //an easy way of checking the characters name and portrait have been loaded from db correctly.
    public Sprite CharacterPortrait;
    public string CharacterName;
    protected Character currentlySpeakingTo;
    protected CharCombatController combatController;
    protected CharMovementController movementController;
    protected CharacterMovement movementType;
    protected Animator myAnimator;
    protected float interactionDistance;
    public GameObject[] CharacterParts;
    protected DialogueUI dialogueUI;

    //image root refers to first transform in editor heirarchy where the parts of the character are stored
    private Transform imageRoot;
	
    public void InitialiseMe() {
        dialogueUI = FindObjectOfType<DialogueUI>();
        movementController = GetComponent<CharMovementController>();
        imageRoot = transform.FindChild("CentreOfGravity").GetComponent<Transform>();
        print(imageRoot);
        print(CharacterParts);
        myAnimator = gameObject.GetComponent<Animator>();
        combatController.SetAnimator(myAnimator);
        combatController.SetCharacter(this);
        SetMovementController();
        interactionDistance = 1f;
    }

    public void SetHovered() {
        foreach(GameObject bodyPart in CharacterParts) {
            bodyPart.GetComponent<SpriteRenderer>().color = new  Color(0.9f, 0.8f, 0.7f);
        }

    }
    public void SetUnhovered() {
        foreach (GameObject bodyPart in CharacterParts) {
            bodyPart.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }
    }

    //public string GetMyPortraitPath() {
    //    return AssetDatabase.GetAssetPath(CharacterPortrait);
    //}

    public void SetMyPortrait(string path) {
        CharacterPortrait = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
    }

    public Sprite GetMyPortrait() {
        return CharacterPortrait;
    }

    public Vector2 GetMyPosition() {
        return new Vector2(
                    (float)Math.Round(gameObject.GetComponent<Transform>().position.x, 1),
                    (float)Math.Round(gameObject.GetComponent<Transform>().position.y, 1));
    }

    public Animator GetMyAnimator() {
        return myAnimator;
    }

    private void SetMovementController() {
        if (movementController != null) {
            movementController.InitialiseMe(CharacterParts, imageRoot);
        }
    }

    public CharMovementController GetMovementController() {
        return movementController;
    }

    public CharCombatController GetCombatController() {
        return combatController;
    }

    public float GetInteractionDistance() {
        return interactionDistance;
    }

    public abstract void EndSelection();

    public void SpeakToCharacter(Character character) {
        currentlySpeakingTo = character;
        dialogueUI.StartNewDialogue(currentlySpeakingTo);
    }

    public void PickUpItem(WorldItem item) {
        item.GetPickedUp();
    }
}
