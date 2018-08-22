using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCombatWhenHit : MonoBehaviour {
    CharCombatController charCombatController;
    PlayerCharacter playerCharacter;
    // Use this for initialization
    void Start () {
        charCombatController = GetComponent<CharCombatController>();
        playerCharacter = FindObjectOfType<PlayerCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
		if (charCombatController.IsAttacking(playerCharacter) ){
            if(charCombatController.Health < charCombatController.BaseHealth) {
                charCombatController.EndCombat(playerCharacter);
                Destroy(charCombatController);
                Destroy(gameObject);
                Destroy(this);
            }
        }
	}
}
