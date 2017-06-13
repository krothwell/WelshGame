using UnityEngine;
using UnityEditor;
using System;
using GameUI;
using GameUtilities;
using GameUtilities.Display;

/// <summary>
/// Abstract class which provides default controls of all derived character
/// types such as the player and NPCs. 
/// </summary>
public abstract class Character : MonoBehaviour {

    //named in editor, this easily allows setting a new portrait and name to new npcs. It also allows
    //an easy way of checking the characters name and portrait have been loaded from db correctly.
    public float maxSpeed;
    public Sprite CharacterPortrait;
    public string CharacterName;
    protected Character currentlySpeakingTo;
    protected CharCombatController combatController;
    protected CharMovementController movementController;
    public CharMovementController MovementController {
        get { return movementController; }
        set { movementController = value; }
    }
    protected CharacterMovement movementType;
    protected Animator myAnimator;
    protected float interactionDistance;
    public GameObject[] CharacterParts;
    protected DialogueUI dialogueUI;
    protected CharacterDecision myDecision;
    public CharAction DefaultAnimationActionPrefab;
    private CharAction defaultAnimationAction;
    public CharacterDecision MyDecision {
        get { return myDecision; }
        set { myDecision = value; }
    }

    //image root refers to first transform in editor heirarchy where the parts of the character are stored
    private Transform imageRoot;
    public Transform ImageRoot {
        get { return imageRoot; }
    }

    protected GameObject perimeter;
    public GameObject Perimeter {
        get { return transform.Find("Perimeter").gameObject; }
    }
	
    public void InitialiseMe() {
        dialogueUI = FindObjectOfType<DialogueUI>();
        imageRoot = transform.Find("CentreOfGravity").GetComponent<Transform>();
        myAnimator = gameObject.GetComponent<Animator>();
        SetCombatController();
        interactionDistance = 1f;
        SetDefaultAnimation();
    }

    protected void Start() {
        ImageLayerOrder.SetOrderOnGameObjectArray(CharacterParts, ImageLayerOrder.GetOrderInt(gameObject) - 1);
        ImageLayerOrder.SetZ(gameObject);
    }

    public void SetDefaultAnimation() {
        if (myAnimator != null) {
            if (DefaultAnimationActionPrefab != null) {
                CharAction defaultAnimationAction = Instantiate(DefaultAnimationActionPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as CharAction;
                defaultAnimationAction.transform.SetParent(transform, false);
                defaultAnimationAction.MyAnimator = myAnimator;
                defaultAnimationAction.MakeAction();
            }
            else {
                AnimationUtilities.SetTriggerIfExists("Idle", myAnimator);
            }
        }
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
                    (float)Math.Round(gameObject.GetComponent<Transform>().localPosition.x, 1),
                    (float)Math.Round(gameObject.GetComponent<Transform>().localPosition.y, 1));
    }

    public Animator GetMyAnimator() {
        return myAnimator;
    }

    private void SetCombatController() {
        if (combatController != null) {
            combatController.SetAnimator(myAnimator);
            combatController.SetCharacter(this);
        }
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
