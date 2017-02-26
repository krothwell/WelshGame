using UnityEngine;
using System;
using System.Collections;
using GameUI;
using UnityUtilities;
/// <summary>
/// This class is responsible for controlling the player’s character by reacting
/// to input from the player and game environment.  
/// </summary>
public class PlayerController : Character {
    public Sprite backUpCharPortrait; //if the character portrait is null then this is used.
    public enum PlayerStatus {
        passive,
        inCombat,
        movingToLocation,
        movingToObject,
        interactingWithObject
    }
    public PlayerStatus playerStatus;
    private Vector2 myCurrentPosition;
    public Vector2 MyCurrentPosition {
        get { return myCurrentPosition; }
        set { myCurrentPosition = value; }
    }
    public GameObject selectedEnemy;
    CombatUI combatui;
    Vector2 camPosition;
    Vector2 destination;
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
    private GameObject clickSelected;
    DialogueUI dialogueUI;

    void Awake() {
        playerStatus = PlayerStatus.passive;
    }

    void Start() {
        camDefault = 0.3f;
        camDelay = camDefault;
        SetCamPosition();
        myCurrentPosition = GetMyPosition();
        walkSpeed = 1.5f;
        runSpeed = 3.5f;
        timeSinceClickOne = 0;
        doubleClickWait = 0.2f;
        animToggles = gameObject.GetComponent<HumanCharAnimToggles>();
        SetMyOrder(bodyParts);
        combatui = FindObjectOfType<CombatUI>();
        strikeRangeX = 1.7f;
        strikeRangeY = 1f;
        interactionDistance = 1f;
        dialogueUI = FindObjectOfType<DialogueUI>();
        SetCharDefaults();
    }

    // Update is called once per frame
    void Update() {
        PrepareToGoToDestinationClicked();
        if (Input.GetMouseButtonUp(0)) {
            MouseSelection.ClickSelect(out clickSelected);
            DestroySelectionCircleOfInteractiveObject();
            if (playerStatus != PlayerStatus.interactingWithObject) {
                if (MouseSelection.IsClickedGameObjectTag("World")) {
                    print("world clicked");
                    destinationClicks++;
                    playerStatus = PlayerStatus.movingToLocation;
                    SetDestination();
                }
                else if (MouseSelection.IsClickedGameObjectTag("WorldObject")) {
                    if (clickSelected.GetComponent<EnemyController>() != null && playerStatus == PlayerStatus.inCombat) {
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
            SetFightMode(false);
            if (myCurrentPosition != destination) {
                StartMoving();
                if (playerStatus == PlayerStatus.movingToObject) {
                    if (GetDistanceFromMyPosition(clickSelected.GetComponent<Transform>().position) < interactionDistance) {
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
                SetFightMode(true);
            }
        }

        if (camDelay <= 0) {
            MoveCameraToCoordinates(destination);
        }
    }


    public void SetMyName(string name) {
        nameID = name;
    }

    public string GetMyName() {
        return nameID;
    }

    public Sprite GetPlayerPortrait() {
        if (GetMyPortrait() == null) {
            return backUpCharPortrait;
        }
        else {
            return GetMyPortrait();
        }
    }

    private void StartMoving() {
        SetTargetPosition(destination, myCurrentPosition);
        MoveToCoordinates(playerSpeed);
        myCurrentPosition = GetMyPosition();
        SetMyOrder(bodyParts);
        SetFightMode(false);
    }

    private void StopMoving() {
        camDelay = 0;
        animToggles.IsWalking = false;
        animToggles.IsRunning = false;
        myCurrentPosition = GetMyPosition();
        if (playerStatus == PlayerStatus.inCombat) {
            SetFightMode(true);
        }
        RerouteCount = 0;
    }

    private void InteractWithObject() {
        playerStatus = PlayerStatus.interactingWithObject;;
        if (clickSelected.GetComponent<Character>() != null) {
            //dialogueUI.SetInUse();
            dialogueUI.StartNewDialogue(clickSelected.GetComponent<Character>());
        } else if (clickSelected.GetComponent<WorldItem>()) {
            clickSelected.GetComponent<WorldItem>().GetPickedUp();
            playerStatus = PlayerStatus.passive;
        }
        currentInteractiveObject = clickSelected;

    }

    public void DestroySelectionCircleOfInteractiveObject() {
        if (currentInteractiveObject != null) {
            currentInteractiveObject.GetComponent<GameWorldObjectSelector>().DestroyMe();
            currentInteractiveObject = null;
        }
    }

    private void PrepareToGoToDestinationClicked() {
        if (destinationClicks > 0) {
            WalkOrRun();
            SetMyDirection(destination, myCurrentPosition);
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
        if (RerouteCount < RerouteLimit) {
            if (playerStatus == PlayerStatus.movingToObject) {

                GameObject objectOfInterest = clickSelected;
                Vector2 objPosition = objectOfInterest.GetComponent<Transform>().position;
                destination = new Vector2((float)Math.Round(objPosition.x,1), (float)Math.Round(objPosition.y,1));

            } else { 
                destination = GetMouseCoords();
            }
        } else {
            destination = GetMyPosition();
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
            //print("camDelay reset");
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
        if (distanceX > strikeRangeX || distanceY > strikeRangeY) {
            print("Enemy too far away");
        }
        else {
            print("attempting strike");
            Vector2 enemyPosition = selectedEnemy.GetComponent<Transform>().position;
            SetMyDirection(enemyPosition, myCurrentPosition);
            SetDistanceFromNewPosition(enemyPosition, myCurrentPosition);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            //dialogueUI.SetInUse();
            //dialogueUI.SetRandomVocab();
        }

    }

    public void SetStatusToUnderAttack() {
        playerStatus = PlayerStatus.inCombat;
    }

    public void SetStatusToInteractingWithObject() {
        playerStatus = PlayerStatus.interactingWithObject;
    }

    public void StrikeSelectedEnemy() {
        print("striking enemy");
        animToggles.Strike();
        selectedEnemy.GetComponent<Character>().SetUnhovered();
        selectedEnemy = null;
    }
}


