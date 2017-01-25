using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class EscMenu : MonoBehaviour {
    GameObject myPanel;
    GameObject saveGameList, loadGameList;
    GameObject submitSaveBtn;
    public GameObject[] options;
    UI ui;
    SaveGameManager saveGameManager;
    GameObject saveInput;
	// Use this for initialization
	void Start () {
        myPanel = transform.FindChild("Panel").gameObject;
        saveGameManager = GameObject.Find("SaveGameManager").GetComponent<SaveGameManager>();
        ui = transform.parent.parent.gameObject.GetComponent<UI>();
        saveGameList = options[0].transform.FindChild("SaveGamesList").gameObject;
        loadGameList = options[1].transform.FindChild("LoadGamesList").gameObject;
        saveInput = options[0].transform.FindChild("SaveInput").gameObject;
        submitSaveBtn = options[0].transform.FindChild("SubmitSaveBtn").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            SetPanel();
            Time.timeScale = Time.timeScale == 0 ? 1:0;
        }
    }

    private void SetPanel() {
        if (myPanel.activeSelf) {
            myPanel.SetActive(false);
        } else { 
            myPanel.SetActive(true);
        }
    }

    public void DisplaySaveComponents() {
        saveGameList.SetActive(true);
        saveInput.SetActive(true);
        submitSaveBtn.SetActive(true);
    }

    public void HideSaveComponents() {
        saveGameList.SetActive(false);
        saveInput.SetActive(false);
        submitSaveBtn.SetActive(false);
    }

    public void SetOptionsDisplay(GameObject optionSelected) {
        foreach (GameObject option in options) {
            option.SetActive(false);
        }
        optionSelected.SetActive(true);
    }

    public void HideOption(GameObject option) {
        option.SetActive(false);
    }

    public void FillSaveGames() {
        ui.FillDisplayFromDb(DbSetup.GetSaveGamesDisplayQry(false), saveGameList.transform, saveGameManager.BuildSaveGameRow);
    }

    public void FillLoadGames() {
        ui.FillDisplayFromDb(DbSetup.GetSaveGamesDisplayQry(true), loadGameList.transform, saveGameManager.BuildSaveGameRow);
    }



}