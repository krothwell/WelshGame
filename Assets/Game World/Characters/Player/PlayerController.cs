using UnityEngine;
using System;
using System.Collections;
using GameUI;
using UnityUtilities;

public class PlayerController : MonoBehaviour {
    public Sprite backUpCharPortrait; //if the character portrait is null then this is used.
    public enum PlayerStatus {
        passive,
        inCombat,
        movingToLocation,
        movingToObject,
        interactingWithObject
    }
    public PlayerStatus playerStatus;
    private Vector2 myPosition;
    public Vector2 MyPosition {
        get { return myPosition; }
        set { myPosition = value; }
    }
    public GameObject selectedEnemy;
    LowerUI lowerUI;
    CombatUI combatui;
    Vector2 camPosition;
    Vector2 destination;
    public GameObject[] characterParts;
    Character character;
    float camDelay;
    float camDefault;
    float walkSpeed;
    float runSpeed;
    float playerSpeed;
    float doubleClickWait;
    float timeSinceClickOne;
    float strikeRangeX;
    float strikeRangeY;
    int destinationClicks;
    public float interactionDistance;
    HumanCharAnimToggles animToggles;
    public GameObject currentInteractiveObject;

    void Awake() {
        character = GetComponent<Character>();
    }

    void Start() {
        playerStatus = PlayerStatus.passive;
        camDefault = 0.3f;
        camDelay = camDefault;
        SetCamPosition();
        myPosition = character.GetMyPosition();
        walkSpeed = 1.5f;
        runSpeed = 3.5f;
        timeSinceClickOne = 0;
        doubleClickWait = 0.2f;
        animToggles = gameObject.GetComponent<HumanCharAnimToggles>();
        character.SetMyOrder(character.bodyParts);
        combatui = FindObjectOfType<CombatUI>();
        strikeRangeX = 1.7f;
        strikeRangeY = 1f;
        lowerUI = FindObjectOfType<LowerUI>();
        interactionDistance = 1f;

    }

    // Update is called once per frame
    void Update() {
        GoToDestinationClicked();
        if (Input.GetMouseButtonUp(0)) {
            MouseSelection.ClickSelect();
            print("mouse clicked");
            DestroySelectionCircleOfInteractiveObject();
            if (playerStatus != PlayerStatus.interactingWithObject) {
                if (MouseSelection.IsClickedGameObjectTag("World")) {
                    print("world clicked");
                    destinationClicks++;
                    playerStatus = PlayerStatus.movingToLocation;
                    SetDestination();
                }
                else if (MouseSelection.IsClickedGameObjectTag("WorldObject")) {
                    if (MouseSelection.selected.GetComponent<EnemyController>() != null && playerStatus == PlayerStatus.inCombat) {
                        if (combatui.currentAbility == CombatUI.CombatAbilities.strike) {
                            AttemptStrikeToSelectedEnemy();
                        }
                    }
                    else {
                        destinationClicks++;
                        playerStatus = PlayerStatus.movingToObject;
                        SetDestination();
                    }
                }

            }
            
        }
        else if (animToggles.IsWalking || animToggles.IsRunning) {

            character.SetFightMode(false);
            if (myPosition != destination) {
                StartMoving();
                if (playerStatus == PlayerStatus.movingToObject) {
                    if (character.GetDistanceFromMyPosition(MouseSelection.selected.GetComponent<Transform>().position) < interactionDistance) {
                        StopMoving();
                        InteractWithObject();
                    }
                }
            }
            else {
                StopMoving();
            }
            camDelay -= Time.deltaTime;
        }

        if (!animToggles.IsWalking && !animToggles.IsRunning) {
            if (playerStatus == PlayerStatus.inCombat) {
                character.SetFightMode(true);
            }
        }

        if (camDelay <= 0) {
            MoveCameraToCoordinates(destination);
        }
    }


    public void SetMyName(string name) {
        GetComponent<Character>().nameID = name;
    }

    public string GetMyName() {
        return character.nameID;
    }

    public void SetMyPortrait(string portraitPath) {
        GetComponent<Character>().SetMyPortrait(portraitPath);
    }
    public Sprite GetMyPortrait() {
        if (GetComponent<Character>().GetMyPortrait() == null) {
            return backUpCharPortrait;
        }
        else {
            return GetComponent<Character>().GetMyPortrait();
        }
    }

    private void StartMoving() {
        character.SetTargetPosition(destination, myPosition);
        character.MoveToCoordinates(playerSpeed);
        myPosition = character.GetMyPosition();
        character.SetMyOrder(character.bodyParts);
        character.SetFightMode(false);
    }

    private void StopMoving() {
        camDelay = 0;
        animToggles.IsWalking = false;
        animToggles.IsRunning = false;
        myPosition = character.GetMyPosition();
        if (playerStatus == PlayerStatus.inCombat) {
            character.SetFightMode(true);
        }
        character.RerouteCount = 0;
    }

    private void InteractWithObject() {
        playerStatus = PlayerStatus.interactingWithObject;
        GameObject objOfInterest = MouseSelection.selected;
        if (objOfInterest.GetComponent<Character>() != null) {
            lowerUI.SetInUse();
            lowerUI.ProcessCharacterDialogue(objOfInterest.GetComponent<Character>());
        }
        currentInteractiveObject = objOfInterest;

    }

    public void DestroySelectionCircleOfInteractiveObject() {
        if (currentInteractiveObject != null) {
            currentInteractiveObject.GetComponent<GameWorldObjectSelector>().DestroyMe();
            currentInteractiveObject = null;
        }
    }

    private void GoToDestinationClicked() {
        if (destinationClicks > 0) {

            WalkOrRun();
            character.SetMyDirection(destination, myPosition);
        }
    }

    private void WalkOrRun() {
        if (timeSinceClickOne < doubleClickWait) {
            timeSinceClickOne += Time.deltaTime;
        }
        else if (destinationClicks > 1) {
            animToggles.IsRunning = true;
            animToggles.IsWalking = false;
            playerSpeed = runSpeed;
            destinationClicks = 0;
            timeSinceClickOne = 0;
        }
        else {
            animToggles.IsWalking = true;
            animToggles.IsRunning = false;
            playerSpeed = walkSpeed;
            destinationClicks = 0;
            timeSinceClickOne = 0;
        }


    }

    private void SetDestination() {
        if (character.RerouteCount < character.RerouteLimit) {
            if (playerStatus == PlayerStatus.movingToObject) {

                GameObject objectOfInterest = MouseSelection.selected;
                Vector2 objPosition = objectOfInterest.GetComponent<Transform>().position;
                destination = new Vector2((float)Math.Round(objPosition.x,1), (float)Math.Round(objPosition.y,1));

            } else { 
                destination = GetMouseCoords();
            }
        } else {
            destination = character.GetMyPosition();
        }
    }

    private void MoveCameraToCoordinates(Vector2 newPosition) {
        if (camPosition != newPosition) {
            float distanceX = Math.Abs(newPosition.x - camPosition.x);
            float distanceY = Math.Abs(newPosition.y - camPosition.y);
            float percentageOfTravelX = (100f / distanceX) * (1 * playerSpeed * Time.deltaTime);
            float percentageOfTravelY = (100f / distanceY) * (1 * playerSpeed * Time.deltaTime);
            int xModifier = camPosition.x >= newPosition.x ? -1 : 1;
            int yModifier = camPosition.y >= newPosition.y ? -1 : 1;
            float newX = distanceX > distanceY ? (playerSpeed * Time.deltaTime) : distanceX / 100 * percentageOfTravelY;
            float newY = distanceY > distanceX ? (playerSpeed * Time.deltaTime) : distanceY / 100 * percentageOfTravelX;
            //the players horizontal direction is changed by the modifier if it does not match the modifier
            Camera.main.transform.Translate(new Vector3(xModifier * newX, yModifier * newY, 0));
            SetCamPosition();
        }
        else {
            camDelay = camDefault;
            print("camDelay reset");
        }
    }

    private Vector2 GetMouseCoords() {
        Vector2 rayPos = new Vector2(
                    (float)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 1),
                    (float)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 1));
        return rayPos;
    }

    private void SetCamPosition() {
        camPosition = new Vector2(
                    (float)Math.Round(Camera.main.GetComponent<Transform>().position.x, 1),
                    (float)Math.Round(Camera.main.GetComponent<Transform>().position.y, 1));
    }

    public void setSelectedEnemy(GameObject enemy) {
        selectedEnemy = enemy;
    }

    void AttemptStrikeToSelectedEnemy() {
        if (character.distanceX > strikeRangeX || character.distanceY > strikeRangeY) {
            print("Enemy too far away");
        }
        else {
            print("attempting strike");
            Vector2 enemyPosition = selectedEnemy.GetComponent<Transform>().position;
            character.SetMyDirection(enemyPosition, myPosition);
            character.SetDistanceFromNewPosition(enemyPosition, myPosition);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            lowerUI.SetInUse();
            lowerUI.SetRandomVocab();
        }

    }

    public void SetUnderAttack() {
        playerStatus = PlayerStatus.inCombat;
    }

    public void StrikeSelectedEnemy() {
        print("striking enemy");
        animToggles.Strike();
        selectedEnemy.GetComponent<Character>().SetUnhovered();
        selectedEnemy = null;
    }
}


