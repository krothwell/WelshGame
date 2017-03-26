using UnityEngine;
using System;
using System.Collections;
using GameUI;

/// <summary>
/// This class is responsible for controlling the actions and reactions of an 
/// enemy character when they attack the player. 
/// </summary>
public class NPCCloseCombatController : CharCombatController {
    CombatUI combatui;
    GameObject combatSlot;
    Vector2 combatSlotPosition;
    EnemyToPlayerCloseCombatPlacements closeRangeEnemyPlacements;
    PlayerCharacter player;
    NPCMovementController npcMovementController;
    Character character;
    private bool isInWeaponRange;



    void Awake() {
        isInWeaponRange = false;
        combatui = FindObjectOfType<CombatUI>();
        player = FindObjectOfType<PlayerCharacter>();
        closeRangeEnemyPlacements = FindObjectOfType<EnemyToPlayerCloseCombatPlacements>();
    }
    void Start() {
    }

    void Update() {
        CheckToAttackPlayer();
    }
    
    public void SetMyMovementController(NPCMovementController movementController) {
        npcMovementController = movementController;
    }
    
    public void SetMyCharacter(Character characterObj) {
        character = characterObj;
    }

    public void CheckToAttackPlayer() {
        if (IsInCombat()) {
            if (isInWeaponRange) {
                AttackPlayer();
            } else {
                MoveToCombatPosition();
            }
        }
    }
    
    private void AttackPlayer() {
        //do attack here
        print(character.name + " attacks the player");
    }
    
    private void MoveToCombatPosition() {
        if (combatSlot == null) {
            SetCombatSlot();
        }
        if (character.GetMyPosition() != combatSlotPosition) {
            npcMovementController.ProcessMovement();
        }
    }

    public override void ProcessAction() {
        throw new NotImplementedException();
    }

    public override void TriggerCombat(Character charIn) {
       
    }

    public override void GetHit() {

    }

    public override void EndCombat(Character charIn) {

    }

    public override void TriggerStrategyMode() {
        throw new NotImplementedException();
    }

    private void SetCombatSlot() {
        GameObject side;
        int sideRaw = player.GetMyPosition().x >= character.GetMyPosition().x ? -1 : 1;
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
            combatSlot = side.transform.GetChild(positionIndex).gameObject;
            closeRangeEnemyPlacements.SetEnemyPlacement(side, positionIndex, gameObject);
            SetCombatSlotPosition();
        }
    }

    private void SetCombatSlotPosition() {
        float x = (float)Math.Round(combatSlot.GetComponent<Transform>().position.x, 1);
        float y = (float)Math.Round(combatSlot.GetComponent<Transform>().position.y, 1);
        combatSlotPosition = new Vector2(x, y);
    }

    public Vector2 GetCombatSlotPosition() {
        return combatSlotPosition;
    }

    //may need to derive this, depends if character combat and npc combat use weapons in the same way, e.g. get their range from the equipped weapon
    public override Vector2 GetWeaponReachXY() {
        throw new NotImplementedException();
    }

    //void OnMouseEnter() {
    //    if (combatui.currentAbility == CombatUI.CombatAbilities.strike) {
    //        SetHovered();
    //    }
    //}

    //void OnMouseExit() {
    //    if(player.selectedEnemy != gameObject) {
    //        SetUnhovered();
    //    }
    //}

    //void OnMouseUpAsButton() {
    //    if (combatui.currentAbility == CombatUI.CombatAbilities.strike) {
    //        player.setSelectedEnemy(gameObject);
    //    }
    //}

}