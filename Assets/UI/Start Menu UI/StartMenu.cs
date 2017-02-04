using UnityEngine;
using System.Collections;
using DbUtilities;

namespace StartMenuUI {
    public class StartMenu : MonoBehaviour {
        public GameObject[] options;
        UIController ui;
        PlayerSavesController saveGameManager;
        GameObject saveGamesList;

        // Use this for initialization
        void Start() {
            ui = GameObject.FindObjectOfType<UIController>();
            saveGameManager = GameObject.FindObjectOfType<PlayerSavesController>();
            saveGamesList = transform.FindChild("LoadGame").FindChild("Panel").FindChild("SaveGamesList").gameObject;
            print(saveGamesList);
        }

        // Update is called once per frame
        void Update() {

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
            ui.FillDisplayFromDb(DbCommands.GetSaveGamesDisplayQry(true), saveGamesList.transform, saveGameManager.BuildSaveGameRow);
        }
    }
}