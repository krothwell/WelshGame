using UnityEngine;
using System.Collections;
using UnityUtilities;
using GameUI;

public abstract class GameWorldSelector : MonoBehaviour {
    /// <summary>
    /// Attaches to interactive game world objects – e.g. NPCs, collectable
    /// items) as a separate script and indicates when the player is hovering
    /// over an object / has selected an object by instantiating a selection 
    /// circle prefab in the object hierarchy.
    /// </summary>
    public GameObject selectionCirclePrefab, selectionDecisionPrefab;
    protected CharacterDecision myDecision;
    protected CharAbility abilitySelected;
    protected GameObject selectionCircle;
    Color selectedColour; 
    protected bool clicked;
    public float Scale, xOffset, yOffset;
    protected PlayerCharacter playerCharacter;
    protected Animator myAnimator;
    protected CombatUI combatUI;

    void Start () {
        playerCharacter = FindObjectOfType<PlayerCharacter>();
        selectedColour = new Color(0.27f, 0.53f, 0.94f);
    }

    public void DestroyMe() {
        if (selectionCircle != null) {
            Destroy(selectionCircle);
            selectionCircle = null;
            clicked = false;
        }
    }

    void OnMouseOver() {
        if (!clicked) {
            if (selectionCircle == null) {
                combatUI = FindObjectOfType<CombatUI>();
                abilitySelected = combatUI.GetCurrentAbility();
                DisplayCircle();
            }
        }
    }

    public abstract void DisplayCircle();

    public void BuildCircle() {
        DestroyMe();
        selectionCircle = Instantiate(selectionCirclePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        selectionCircle.GetComponent<Transform>().localScale = new Vector2(Scale, Scale);
        selectionCircle.GetComponent<Transform>().localPosition = new Vector2(xOffset, yOffset);
        selectionCircle.transform.SetParent(transform, false);
        myAnimator = selectionCircle.GetComponent<Animator>();
    }

    public void BuildCircle(Vector2 atCoordinates) {
        DestroyMe();
        selectionCircle = Instantiate(selectionCirclePrefab, new Vector3(atCoordinates.x, atCoordinates.y, 0f), Quaternion.identity) as GameObject;
        selectionCircle.GetComponent<Transform>().localScale = new Vector2(Scale, Scale);
        selectionCircle.transform.SetParent(transform, false);
        myAnimator = selectionCircle.GetComponent<Animator>();
    }

    void OnMouseExit() {
        if (!clicked) {
            DestroyMe();
        }
    }

    void OnMouseUpAsButton() {
        SetSelected();
    }

    protected void Select() {
        clicked = true;
        ChangeColourToSelected();
        if (playerCharacter.GetCurrentSelectionCircle() != this) {
            playerCharacter.EndSelection();
            playerCharacter.SetCurrentSelection(this);
        }
    }

    public abstract void SetSelected();

    protected void ChangeColourToSelected () {
        if (selectionCircle != null) {
            selectionCircle.GetComponent<SpriteRenderer>().color = selectedColour;
        }
    }

    protected void BuildSelectionPlayerDecision(GameObject decisionPrefab) {
        GameObject decision = Instantiate(decisionPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        decision.GetComponent<CharacterDecision>().InitialiseMe(playerCharacter);
        decision.transform.SetParent(playerCharacter.transform, false);
        myDecision = decision.GetComponent<CharacterDecision>();
    }

    protected void QueueDecisionToRun() {
        if (combatUI.IsCombatUIActive()) {
            combatUI.QueueDecision(myDecision);
        }
        else {
            myDecision.ProcessDecision();
        }
    }
}
