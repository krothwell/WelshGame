using UnityEngine;
using System;
using System.Collections;
using GameUI;

/// <summary>
/// This class is responsible for controlling the actions and reactions of an 
/// enemy character when they attack the player. 
/// </summary>
public class EnemyController : Character {
    CombatUI combatui;
    GameObject positionSet;
    Vector2 newTargetPosition;
    EnemyToPlayerCloseCombatPlacements closeRangeEnemyPlacements;
    Vector2 myCurrentPosition;
    float walkSpeed;
    float runSpeed;
    float mySpeed;
    HumanCharAnimToggles animToggles;
    public bool beViolent;
    PlayerController player;
    private bool nextToPlayer = false;
    public bool NextToPlayer {
        get { return nextToPlayer; }
        set { nextToPlayer = value; }
    }

    void Start() {
        combatui = FindObjectOfType<CombatUI>();
        myCurrentPosition = GetMyPosition();
        walkSpeed = 1.5f;
        runSpeed = 3.5f;
        animToggles = gameObject.GetComponent<HumanCharAnimToggles>();
        player = FindObjectOfType<PlayerController>();
        closeRangeEnemyPlacements = FindObjectOfType<EnemyToPlayerCloseCombatPlacements>();
        SetCharDefaults();
    }

    void Update() {
        if (beViolent) {
            if (positionSet == null) {
                SetNewPosition();
                player.SetStatusToUnderAttack();
            }
            else {
                SetPositionToPlayer();
            }
        }
    }

    /*nextToPlayer bool is set by object holding collider */
    private void SetPositionToPlayer() {
        if (myCurrentPosition == newTargetPosition) {
            if(redirecting) {
                SetInterimPosition(Vector2.zero, false);
            } else {
                animToggles.IsRunning = false;
                animToggles.IsWalking = false;
                mySpeed = 0;
                myCurrentPosition = GetMyPosition();
                SetPositionCoords();
                SetMyDirection(player.MyCurrentPosition, myCurrentPosition);
                SetFightMode(true);
            }


        } else {
            SetFightMode(false);
            print("moving to new position");
            if (distanceX < 2f && distanceY < 1f) {
                animToggles.IsRunning = false;
                animToggles.IsWalking = true;
                mySpeed = walkSpeed;
            } else {
                animToggles.IsRunning = true;
                animToggles.IsWalking = false;
                mySpeed = runSpeed;
            }
            SetTargetPosition(newTargetPosition,myCurrentPosition);
            MoveToCoordinates(mySpeed);
            SetMyOrder(bodyParts);
            myCurrentPosition = GetMyPosition();
            if (distanceX > 2 && distanceY > 2) {
                SetMyDirection(newTargetPosition, myCurrentPosition);
            } else {
                SetMyDirection(player.MyCurrentPosition, myCurrentPosition);
            }
        }
    }

    private void SetNewPosition() {
        GameObject side;
        int sideRaw = player.MyCurrentPosition.x >= myCurrentPosition.x ? -1 : 1;
        int positionIndex;
        if (sideRaw == -1) {
            positionIndex = closeRangeEnemyPlacements.GetFreePositionLeft();
            side = closeRangeEnemyPlacements.left;
            if (positionIndex == -1) {
                positionIndex = closeRangeEnemyPlacements.GetFreePositionRight();
                side = closeRangeEnemyPlacements.right;
                print("player is to my right, but no positions to his left so will move to his right");
            }
        }
        else {
            positionIndex = closeRangeEnemyPlacements.GetFreePositionRight();
            side = closeRangeEnemyPlacements.right;
            if (positionIndex == -1) {
                positionIndex = closeRangeEnemyPlacements.GetFreePositionLeft();
                side = closeRangeEnemyPlacements.left;
                print("player is to my left, but no positions to his right so will move to his left");
            }
        }

        if (positionIndex > -1) {
            positionSet = side.transform.GetChild(positionIndex).gameObject;
            closeRangeEnemyPlacements.SetEnemyPlacement(side, positionIndex, gameObject);
            SetPositionCoords();
        }
    }

    public void SetPositionCoords() {

        float x = (float)Math.Round(positionSet.GetComponent<Transform>().position.x, 1);
        float y = (float)Math.Round(positionSet.GetComponent<Transform>().position.y, 1);
        newTargetPosition = new Vector2(x, y);
    }

    void OnMouseEnter() {
        if (combatui.currentAbility == CombatUI.CombatAbilities.strike) {
            SetHovered();
        }
    }

    void OnMouseExit() {
        if(player.selectedEnemy != gameObject) {
            SetUnhovered();
        }
        
    }

    void OnMouseUp() {
        if (combatui.currentAbility == CombatUI.CombatAbilities.strike) {
            player.setSelectedEnemy(gameObject);
        }
    }

}