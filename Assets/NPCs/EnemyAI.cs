using UnityEngine;
using System;
using System.Collections;

public class EnemyAI : MonoBehaviour {
    CombatUI combatui;
    GameObject positionSet;
    Vector2 newPosition;
    CloseRangeEnemyPlacements closeRangeEnemyPlacements;
    Vector2 myPosition;
    public GameObject[] characterParts;
    Character character;
    float walkSpeed;
    float runSpeed;
    float mySpeed;
    AnimToggles animToggles;
    public bool beViolent;
    MainCharacter mainCharacter;
    private bool nextToPlayer = false;
    public bool NextToPlayer {
        get { return nextToPlayer; }
        set { nextToPlayer = value; }
    }

    void Start() {
        combatui = FindObjectOfType<CombatUI>();
        character = gameObject.GetComponent<Character>();
        myPosition = character.GetMyPosition();
        walkSpeed = 1.5f;
        runSpeed = 3.5f;
        animToggles = gameObject.GetComponent<AnimToggles>();
        mainCharacter = FindObjectOfType<MainCharacter>();
        closeRangeEnemyPlacements = FindObjectOfType<CloseRangeEnemyPlacements>();
    }

    void Update() {
        if (beViolent) {
            if (positionSet == null) {
                SetNewPosition();
                mainCharacter.SetUnderAttack();
            }
            else {
                SetPositionToPlayer();
            }
        }
    }

    /*nextToPlayer bool is set by object holding collider */
    private void SetPositionToPlayer() {
        if (myPosition == newPosition) {
            if(character.redirecting) {
                character.SetInterimPosition(Vector2.zero, false);
            } else {
                animToggles.IsRunning = false;
                animToggles.IsWalking = false;
                mySpeed = 0;
                myPosition = character.GetMyPosition();
                SetPositionCoords();
                character.SetMyDirection(mainCharacter.MyPosition, myPosition);
                character.SetFightMode(true);
            }


        } else {
            character.SetFightMode(false);
            print("moving to new position");
            if (character.distanceX < 2f && character.distanceY < 1f) {
                animToggles.IsRunning = false;
                animToggles.IsWalking = true;
                mySpeed = walkSpeed;
            } else {
                animToggles.IsRunning = true;
                animToggles.IsWalking = false;
                mySpeed = runSpeed;
            }
            character.SetTargetPosition(newPosition,myPosition);
            character.MoveToCoordinates(mySpeed);
            character.SetMyOrder(character.bodyParts);
            myPosition = character.GetMyPosition();
            if (character.distanceX > 2 && character.distanceY > 2) {
                character.SetMyDirection(newPosition, myPosition);
            } else {
                character.SetMyDirection(mainCharacter.MyPosition, myPosition);
            }
        }
    }

    private void SetNewPosition() {
        GameObject side;
        int sideRaw = mainCharacter.MyPosition.x >= myPosition.x ? -1 : 1;
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
        newPosition = new Vector2(x, y);
    }

    void OnMouseEnter() {
        if (combatui.currentAbility == CombatUI.CombatAbilities.strike) {
            character.SetHovered();
        }
    }

    void OnMouseExit() {
        if(mainCharacter.selectedEnemy != gameObject) {
            character.SetUnhovered();
        }
        
    }

    void OnMouseUp() {
        if (combatui.currentAbility == CombatUI.CombatAbilities.strike) {
            mainCharacter.setSelectedEnemy(gameObject);
        }
    }

}