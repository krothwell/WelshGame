using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InspectorUI : UIController {
    private GameObject healthSlider;

    void Start() {
        healthSlider = GetPanel().GetComponentInChildren<Slider>().gameObject;
    }

    public void SetInspectorText(string txt) {
        GetPanel().GetComponentInChildren<Text>().text = txt;
    }

    public void SetInspectorPosition(Transform tf, float offset) {
        transform.position = new Vector2(tf.position.x, tf.position.y + offset);
    }

    public void SetInspectorHealth(Transform tf) {
        CharCombatController combatController = tf.GetComponentInChildren<CharCombatController>();
        if(combatController == null) {
            healthSlider.SetActive(false);
        }
        else {
            healthSlider.SetActive(true);
            Slider mySlider = healthSlider.GetComponent<Slider>();
            mySlider.maxValue = combatController.BaseHealth;
            mySlider.value = combatController.Health;
        }
    }

}
