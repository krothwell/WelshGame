using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public GameObject[] options;
    UI ui;
    SaveGameManager saveGameManager;
    GameObject saveGamesList;

	// Use this for initialization
	void Start () {
        ui = GameObject.FindObjectOfType<UI>();
        saveGameManager = GameObject.FindObjectOfType<SaveGameManager>();
        saveGamesList = transform.FindChild("LoadGame").FindChild("Panel").FindChild("SaveGamesList").gameObject;
        print(saveGamesList);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetOptionsDisplay(GameObject optionSelected) {
        foreach (GameObject option in options) {
            HideOptionMenu(option);
        }
        optionSelected.transform.FindChild("Panel").gameObject.SetActive(true); 
    }

    public void HideOptionMenu(GameObject option) {
        GameObject optionPanel = option.transform.FindChild("Panel").gameObject;
        optionPanel.SetActive(false);
    }

    public void FillSaveGames() {
        ui.FillDisplayFromDb(DbSetup.GetSaveGamesDisplayQry(true), saveGamesList.transform, saveGameManager.BuildSaveGameRow);
    }
}
