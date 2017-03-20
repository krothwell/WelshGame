using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities.Display;

public class StaticCharacter : NPCCharacter {

	// Use this for initialization
	void Awake () {
        ImageLayerOrder.SetOrderOnGameObjectArray(CharacterParts, ImageLayerOrder.GetOrderInt(gameObject) - 1);
        ImageLayerOrder.SetZ(gameObject);
        combatController = GetComponentInChildren<CharCombatController>();
        InitialiseMe();
    }
	
}
