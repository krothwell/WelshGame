using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
using System;
using GameUtilities;

public class PlayerCombatController : CharCombatController {
    public AudioClip combatTheme;
    PlayerInventoryUI inventory;
    public GameObject selectedEnemy;
    CombatUI combatui;
    PlayerCharacter character;
    public CombatStateAction CombatStateActionPrefab;
    private CombatStateAction combatStateAction;
    protected WeaponItem myWeapon;

    new void Awake() {
        combatui = FindObjectOfType<CombatUI>();
        character = FindObjectOfType<PlayerCharacter>();
        inventory = FindObjectOfType<PlayerInventoryUI>();
        base.Awake();
    }
    public override WorldDamage GetWeaponDamage() {
        WeaponItem weapon = (inventory.GetItemFromEquippedDict(WorldItems.WorldItemTypes.WeaponWearable) as WeaponItem);
        WorldDamage wd = new WorldDamage();
        wd.BaseWeaponDamage = weapon.BaseDamage;
        return wd;
    }

    public void setSelectedEnemy(GameObject enemy) {
        selectedEnemy = enemy;
    }

    public override void TriggerCombat(Character charIn) {
        MusicPlayer mp = FindObjectOfType<MusicPlayer>();
        mp.TransitionMusic(combatTheme, 0.005f, 0.005f);
        print("play combat theme");
        mp.MusicLocked = true;
        combatStateAction = Instantiate(CombatStateActionPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as CombatStateAction;
        combatStateAction.transform.SetParent(myCharacter.transform, false);
        combatStateAction.MyAnimator = GetComponent<Animator>();
        AddToEnemyList(charIn);
        combatui.AddToUnderAttack(charIn);
        ToggleInCombat(true);
        combatStateAction.MakeAction();
        TriggerStrategyMode();
    }

    public void TriggerStrategyMode() {
        combatui.ToggleCombatMode();
    }

    public override void GetHit(WorldDamage damage) {
        DeductHealth(damage);
        print(health);
        combatui.SetPlayerHealthDisplay(health);
    }

    public override void EndCombat(Character charIn) {
        print("ending combat...");
        if (CharacterEnemyList.Contains(charIn)) {
            RemoveFromEnemyList(charIn);
            combatui.RemoveFromUnderAttack(charIn);
            if (CharacterEnemyList.Count < 1) {
                ToggleInCombat(false);
                combatStateAction.StopAction();
                combatui.HideUnderAttack();
                combatui.HidePlayerVitals();
                MusicPlayer mp = FindObjectOfType<MusicPlayer>();
                print(mp);
                mp.MusicLocked = false;
                character.CurrentMusicZone.TransitionToZoneMusic();
            }
        }
    }

    public override Vector2 GetWeaponReachXY() {
        myWeapon = (inventory.GetItemFromEquippedDict(WorldItems.WorldItemTypes.WeaponWearable) as WeaponItem);
        if (myWeapon == null) {
            return new Vector2(0f, 0f);
        } else { 
            return myWeapon.GetWeaponRange();
        }
    }

    public bool IsCharacterEnemy(Character characterIn) {
        return characterIn.GetCombatController().IsAttacking(character);
    }
}
