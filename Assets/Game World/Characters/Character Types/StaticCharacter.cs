using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities.Display;

public class StaticCharacter : NPCCharacter {

	// Use this for initialization
	void Awake () {
        print(gameObject);
        int gameObjectLayerOrder = ImageLayerOrder.GetOrderInt(gameObject) - 1;
        print(gameObjectLayerOrder);
        ImageLayerOrder.SetOrderOnGameObjectArray(CharacterParts, gameObjectLayerOrder);
        ImageLayerOrder.SetZ(gameObject);
        combatController = GetComponentInChildren<CharCombatController>();
        InitialiseMe();
    }
	
}
