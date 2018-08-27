using UnityEngine;

using GameUI;

/// <summary>
/// Implements some methods derived from WorldItem to deal with items that deal
/// with multiple sprites (e.g. body armour can be made of upper torso, upper 
/// arms, and lower arms parts). 
/// </summary>

public abstract class EquipableWorldItem : WorldItem {
    public GameObject[] equipToPlayerParts;

    protected override void Start() {
        base.Start();
        SetPlayerParts();
    }

    protected abstract void SetPlayerParts();

    
}