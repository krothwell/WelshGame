using UnityEngine;
using System.Collections;
using DbUtilities;

namespace StartMenuUI {
    public class StartMenu : UIController {
        public GameObject[] options;
        public string mainMenus;
        UIController ui;
        PlayerSavesController saveGameManager;
        LoadGameMenuUI loadGameMenuUI;
        NewGameMenuUI newGameMenuUI;
        GameObject saveGamesList;

        // Use this for initialization
        void Start() {
            ui = FindObjectOfType<UIController>();
            saveGameManager = FindObjectOfType<PlayerSavesController>();
            loadGameMenuUI = FindObjectOfType<LoadGameMenuUI>();
            newGameMenuUI = FindObjectOfType<NewGameMenuUI>();
            saveGamesList = loadGameMenuUI.transform.Find("Panel").Find("SaveGamesList").gameObject;
            print(saveGamesList);
            mainMenus = "mainMenus";
            CreateNewMenuToggleGroup(mainMenus);
        }

        public void FillSaveGames() {
            ui.FillDisplayFromDb(DbQueries.GetSaveGamesDisplayQry(true), saveGamesList.transform, saveGameManager.BuildSaveGameRow);
        }

        public void ActivateLoadGameMenu() {
            ToggleMenuTo(loadGameMenuUI,mainMenus);
        }

        public void ActivateNewGameMenu() {
            ToggleMenuTo(newGameMenuUI, mainMenus);
        }
    }
}