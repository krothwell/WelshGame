using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DbUtilities;
namespace GameUI {
    public class GameMenuUI : UIController {
        private bool isOn;
        public bool IsOn {
            get { return isOn; }
        }
        GameObject myPanel;
        GameObject saveGameList, loadGameList;
        GameObject submitSaveBtn;
        public GameObject[] options;
        PlayerSavesController playerSavesController;
        GameObject saveInput;
        SceneLoader sceneLoader;
        // Use this for initialization
        void Start() {
            myPanel = transform.Find("Panel").gameObject;
            playerSavesController = FindObjectOfType<PlayerSavesController>();
            saveGameList = options[0].transform.Find("SaveGamesList").gameObject;
            loadGameList = options[1].transform.Find("LoadGamesList").gameObject;
            saveInput = options[0].transform.Find("SaveInput").gameObject;
            submitSaveBtn = options[0].transform.Find("SubmitSaveBtn").gameObject;
            sceneLoader = new SceneLoader();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                SetPanel();
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }
        }

        private void SetPanel() {
            if (myPanel.activeSelf) {
                myPanel.SetActive(false);
                isOn = false;
            }
            else {
                myPanel.SetActive(true);
                isOn = true;
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
            FillDisplayFromDb(DbQueries.GetSaveGamesDisplayQry(false), saveGameList.transform, playerSavesController.BuildSaveGameRow);
        }

        public void FillLoadGames() {
            FillDisplayFromDb(DbQueries.GetSaveGamesDisplayQry(true), loadGameList.transform, playerSavesController.BuildSaveGameRow);
        }

        public void ExitToMain() {
            sceneLoader.LoadSceneByIndex(0);
        }



    }
}