using UnityEngine;
using System.Collections;
using UnityUtilities;
using GameUI;
using GameUtilities;

public abstract class GameWorldSelector : MonoBehaviour {
    /// <summary>
    /// Attaches to interactive game world objects – e.g. NPCs, collectable
    /// items) as a separate script and indicates when the player is hovering
    /// over an object / has selected an object by instantiating a selection 
    /// circle prefab in the object hierarchy.
    /// </summary>
    public GameObject selectionCirclePrefab, DefaultSelectionDecisionPrefab;
    protected CharacterDecision myDecision;
    public CharacterDecision MyDecision {
        get { return myDecision; }
    }
    protected CharAbility abilitySelected;
    protected GameObject selectionCircle;
    Color selectedColour; 
    protected bool clicked, countingDown;
    public float Scale, xOffset, yOffset;
    protected PlayerCharacter playerCharacter;
    protected Animator myAnimator;
    protected CombatUI combatUI;
    protected float countdown;
    protected CharacterMovement movementType;
    protected bool doubleClicks;
    public bool DoubleClicks {
        get { return doubleClicks; }
    }

    protected MouseSelection mouseSelection;

    void Start () {
        countdown = 0.2f;
        playerCharacter = FindObjectOfType<PlayerCharacter>();
        selectedColour = new Color(0.27f, 0.53f, 0.94f);
        mouseSelection = FindObjectOfType<MouseSelection>();
    }

    void Update() {
        if (countingDown) {
            countdown -= Time.deltaTime;
            if (countdown <= 0) {
                SetSelected();
                ResetCountdown();
            }
        }
    }

    public void ResetCountdown() {
        countingDown = false;
        countdown = 0.2f;
        doubleClicks = false;
    }

    public void EndCurrentSelection() {
        DestroySelectionCircle();
        if (myDecision != null) {
            if (!myDecision.IsEnding) {
                myDecision.EndDecision();
                myDecision = null;
            }
        }
        clicked = false;
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
        EndCurrentSelection();
        selectionCircle = Instantiate(selectionCirclePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        selectionCircle.GetComponent<Transform>().localScale = new Vector2(Scale, Scale);
        selectionCircle.GetComponent<Transform>().localPosition = new Vector3(xOffset, yOffset, 0.1f);
        selectionCircle.transform.SetParent(transform, false);
        myAnimator = selectionCircle.GetComponent<Animator>();
    }

    public void BuildCircle(Vector2 atCoordinates) {
        EndCurrentSelection();
        print("creating selection circle");
        selectionCircle = Instantiate(selectionCirclePrefab, new Vector3(atCoordinates.x, atCoordinates.y, -0.0001f), Quaternion.identity) as GameObject;
        selectionCircle.GetComponent<Transform>().localScale = new Vector2(Scale, Scale);
        selectionCircle.transform.SetParent(transform, false);
        myAnimator = selectionCircle.GetComponent<Animator>();

    }

    public void DestroySelectionCircle() {
        if (selectionCircle != null) {
            print("destroying seleciton circle");
            Destroy(selectionCircle);
            selectionCircle = null;
            //doubleClicks = false;
        }
    }

    void OnMouseExit() {
        if (!clicked) {
            clicked = false;
            DestroySelectionCircle();
        }
    }

    void OnMouseUpAsButton() {
        if (World.IsGamePaused()) {
            SetSelected();
            ResetCountdown();
        }
        else if (!countingDown) {
            CountDownToDecision();
        }
        Select();
        SetMouseClicks();
    }

    public void CountDownToDecision() {
        countingDown = true;
    }

    public void SetMouseClicks() {
        if (Input.GetMouseButtonUp(0)) {
            mouseSelection.GetIsDoubleClick(out doubleClicks);
        }
    }

    protected void Select() {
        playerCharacter.EndSelection();
        clicked = true;
        ChangeColourToSelected();
        playerCharacter.SetCurrentSelection(this);
    }

    public abstract void SetSelected();

    protected void ChangeColourToSelected () {
        if (selectionCircle != null) {
            selectionCircle.GetComponent<SpriteRenderer>().color = selectedColour;
        }
    }

    protected void BuildSelectionPlayerDecision(GameObject decisionPrefab) {
        if (myDecision == null) {
            GameObject decision = Instantiate(decisionPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            decision.GetComponent<CharacterDecision>().InitialiseMe(playerCharacter);
            decision.transform.SetParent(playerCharacter.transform, false);
            myDecision = decision.GetComponent<CharacterDecision>();
            playerCharacter.MyDecision = myDecision;
        }
    }

    protected void QueueDecisionToRun() {
        if (combatUI.IsCombatUIActive()) {
            combatUI.QueueDecision(myDecision);
        }
        else {
            if (myDecision != null) {
                myDecision.ProcessDecision();
            }
        }
    }
}
