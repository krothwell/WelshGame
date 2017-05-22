using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtilities.Display;

public class StaticCharacter : NPCCharacter {

	// Use this for initialization
	void Awake () {
        int gameObjectLayerOrder = ImageLayerOrder.GetOrderInt(gameObject) - 1;
        ImageLayerOrder.SetOrderOnGameObjectArray(CharacterParts, gameObjectLayerOrder);
        ImageLayerOrder.SetZ(gameObject);
        combatController = GetComponentInChildren<CharCombatController>();
        InitialiseMe();
    }

    void Update() {
        //print(GetMyPosition());
    }
	
}
