using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class CharacterPortraitSelector : MonoBehaviour {

    NewGame newGame;

	// Use this for initialization
	void Start () {
        newGame = FindObjectOfType<NewGame>();
	}
	
	// Update is called once per frame
	void Update () {
        DeselectIfTypeIsSame();
	}

    private void DeselectIfTypeIsSame() {
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();
            if (SelectController.ClickedDifferentGameObjectTo(this.gameObject)) {
                //if a rule is not being edited then the rule list is refreshed.
                CharacterPortraitSelector charPortraitSelector = SelectController.selected.GetComponent<CharacterPortraitSelector>();
                if (charPortraitSelector != null) {
                    DeselectMe();
                }
            }
        }
    }

    public void SelectMe() {
        GetComponent<Image>().color = newGame.portraitSelectedColor;
        newGame.SetSelectedPortrait(GetComponent<CharacterPortraitSelector>());
    }

    private void DeselectMe() {
        GetComponent<Image>().color = newGame.portraitDeselectedColor;
    }

    public string GetMyImagePath() {
        return AssetDatabase.GetAssetPath(transform.GetChild(0).GetComponentInChildren<Image>().sprite);
    }

    void OnMouseEnter() {
        if (newGame.GetSelectedPortrait().gameObject != gameObject) {
            GetComponent<Image>().color = newGame.portraitHoverColor;
        }
    }

    void OnMouseExit() {
        if (newGame.GetSelectedPortrait().gameObject != gameObject) {
            DeselectMe();
        }
    }

    void OnMouseUp() {
        SelectMe();
    }
}
