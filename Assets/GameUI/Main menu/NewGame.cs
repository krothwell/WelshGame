using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewGame : MonoBehaviour {
    public Color portraitSelectedColor, portraitHoverColor, portraitDeselectedColor;
    private CharacterPortraitSelector currentPortraitSelected;
    GameObject panel,portraitGrid;
	// Use this for initialization
	void Start () {
        panel = gameObject.transform.FindChild("Panel").gameObject;
        portraitSelectedColor = new Color(0f,0.35f,66.6f, 0.6f);
        portraitHoverColor = new Color(0f, 0.15f, 46.6f, 0.6f);
        portraitDeselectedColor = new Color(0f,0f,0f,0.50196f);
        portraitGrid = panel.transform.FindChild("PortraitGrid").gameObject;
        SetFirstPortraitSelected();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SetFirstPortraitSelected() {
        GameObject firstPortrait = portraitGrid.transform.GetChild(0).gameObject;
        firstPortrait.GetComponent<CharacterPortraitSelector>().SelectMe();
    }

    public void DisplayPanel() {
        panel.SetActive(true);
    }

    public void HidePanel() {
        panel.SetActive(false);
    }

    public void SetSelectedPortrait(CharacterPortraitSelector portrait) {
        currentPortraitSelected = portrait;
    }

    public string GetSelectedPortraitPath() {
        print(currentPortraitSelected);
        return currentPortraitSelected.GetMyImagePath();
    }

    public CharacterPortraitSelector GetSelectedPortrait() {
        return currentPortraitSelected;
    }

    public string GetNameInput() {
        return GetComponentInChildren<InputField>().text == "" ? 
            GetComponentInChildren<InputField>().gameObject.transform.FindChild("Placeholder").GetComponent<Text>().text : 
            GetComponentInChildren<InputField>().text;
    }
}
