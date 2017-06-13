using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVitalsUI : UIController {
    PlayerCharacter player;
    Slider healthSlider;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerCharacter>();
        healthSlider = GetPanel().GetComponentInChildren<Slider>();
        healthSlider.maxValue = player.GetCombatController().BaseHealth;
        healthSlider.value = player.GetCombatController().Health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    new void DisplayComponents() {
        healthSlider.maxValue = player.GetCombatController().BaseHealth;
        healthSlider.value = player.GetCombatController().Health;
        base.DisplayComponents();
    }

    public void SetHealth(float value) {
        print(healthSlider);
        print(value);
        print(healthSlider.transform);
        healthSlider.value = value;
    }
}
