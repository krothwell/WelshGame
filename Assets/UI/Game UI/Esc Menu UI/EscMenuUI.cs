using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DbUtilities;
namespace GameUI {
    public class EscMenuUI : MonoBehaviour {
        GameObject myPanel;
        GameObject saveGameList, loadGameList;
        GameObject submitSaveBtn;
        public GameObject[] options;
        UIController ui;
        PlayerSavesController playerSavesController;
        GameObject saveInput;
        // Use this for initialization
        void Start() {
            myPanel = transform.FindChild("Panel").gameObject;
            playerSavesController = FindObjectOfType<PlayerSavesController>();
            ui = transform.parent.parent.gameObject.GetComponent<UIController>();
            saveGameList = options[0].transform.FindChild("SaveGamesList").gameObject;
            loadGameList = options[1].transform.FindChild("LoadGamesList").gameObject;
            saveInput = options[0].transform.FindChild("SaveInput").gameObject;
            submitSaveBtn = options[0].transform.FindChild("SubmitSaveBtn").gameObject;
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
            }
            else {
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
            ui.FillDisplayFromDb(DbQueries.GetSaveGamesDisplayQry(false), saveGameList.transform, playerSavesController.BuildSaveGameRow);
        }

        public void FillLoadGames() {
            ui.FillDisplayFromDb(DbQueries.GetSaveGamesDisplayQry(true), loadGameList.transform, playerSavesController.BuildSaveGameRow);
        }



    }
}