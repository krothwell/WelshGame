using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVitalsUI : UIController {
    PlayerCharacter player;
    Slider healthSlider;
    private Image portrait;
    public Image Portrait {
        get { return GetPanel().transform.Find("PlayerBtn").Find("Image").GetComponent<Image>(); }
    }
    // Use this for initialization
    Image portraitImg;
	void Start () {
        player = FindObjectOfType<PlayerCharacter>();
        healthSlider = GetPanel().GetComponentInChildren<Slider>();
        healthSlider.maxValue = player.GetCombatController().BaseHealth;
        healthSlider.value = player.GetCombatController().Health;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public new void DisplayComponents() {
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
