using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectorBtn : MonoBehaviour {
    Image portraitImg;
    GameWorldSelector gameWorldSelector;
	// Use this for initialization
	void Awake () {
        portraitImg = transform.Find("Image").GetComponent<Image>();
	}
	
    public void InitialiseMe(Sprite _portrait, GameWorldSelector _selector) {
        print(portraitImg);
        print(_portrait);
        print(_selector);
        portraitImg.sprite = _portrait;
        gameWorldSelector = _selector;
    }

    void OnMouseOver() {
        gameWorldSelector.OnMouseOver();
    }

    void OnMouseUpAsButton() {
        gameWorldSelector.OnMouseUpAsButton();
    }

    void OnMouseExit() {
        gameWorldSelector.OnMouseExit();
    }
}
