using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
//using StartMenuUI;
using DbUtilities;
using GameUI;
using UnityUtilities;

public class PlayerSavesController : MonoBehaviour {
    GameObject player;
    public GameObject saveGamePrefab;
    public string playerName, playerPortraitPath;
    public int saveID;
    GameObject saveGame;
    SceneLoader sceneLoader;
    private ExistingSaveGame selectedSave;
    public ExistingSaveGame SelectedSave {
        get { return selectedSave; }
        set { selectedSave = value; }
    }

    public GameObject newSaveInput;

    /// <summary>
    /// Rather than rely on start or awake I want to ensure that attempts to load the game are made
    /// after the database path has been set.
    /// </summary>
    public void ManuallyInitialise() {
        if (FindObjectOfType<PlayerCharacter>() != null) {
            
            player = FindObjectOfType<PlayerCharacter>().gameObject;
            LoadSave();
            



        }
    }

    // Use this for initialization
    void Awake() {
        sceneLoader = new SceneLoader();
    }

    private void SetPlayerName(string name) {
        playerName = name;
        //player.GetComponent<MainCharacter>().SetMyName(name);
    }

    private void SetPlayerPortraitPath(string path) {
        playerPortraitPath = path;
        //player.GetComponent<MainCharacter>().SetMyName(name);
    }

    private void SetSaveID(string sid) {
        saveID = int.Parse(sid);
    }

    public void StartNewGame(string name, string portraitPath) {
        string[,] pgFields = new string[,] {
                                                { "PlayerNames", name },
                                                { "PortraitImages", portraitPath },
                                                { "Dates", DateTime.Now.ToString() },
                                                { "LocationName", "Start" },
                                                { "LocationX", "-2" },
                                                { "LocationY", "0" },
                                                { "SkillPointsSpent", "0" }
                                            };
        DbCommands.UpdateTableTuple("PlayerGames", "SaveIDs = 0", pgFields);

        //Dialogues activated
        string[,] delFields = new string[,] {
                                                {"SaveIDs", "0" }
        };
        DbCommands.DeleteTupleInTable("ActivatedDialogues", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("ActivatedDialogues",
                                                            new string[] { "SaveIDs" },
                                                            new string[] { "0" },
                                                            new string[] { "-1" });
        //Quests activated
        DbCommands.DeleteTupleInTable("QuestsActivated", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("QuestsActivated",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { "0" },
                                                    new string[] { "-1" });

        //Quests tasks completed
        DbCommands.DeleteTupleInTable("QuestTasksActivated", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("QuestTasksActivated",
                                            new string[] { "SaveIDs" },
                                            new string[] { "0" },
                                            new string[] { "-1" });

        //Task parts completed
        DbCommands.DeleteTupleInTable("CompletedQuestTaskParts", delFields);

        //discovered vocabulary
        DbCommands.DeleteTupleInTable("DiscoveredVocab", delFields);

        //discovered vocabulary
        DbCommands.DeleteTupleInTable("DiscoveredVocabGrammar", delFields);

        //acquired vocabulary read proficiencies
        DbCommands.DeleteTupleInTable("AcquiredVocabReadSkills", delFields);

        //acquired vocabulary write proficiencies
        DbCommands.DeleteTupleInTable("AcquiredVocabWriteSkills", delFields);

        //acquired grammar proficiencies
        DbCommands.DeleteTupleInTable("AcquiredGrammarSkills", delFields);

        PlayerPrefsManager.SetSaveGame(0);
        sceneLoader.LoadSceneByName("Demo");
    }

    /*used in game to create a new save with player referenced save slot */

    public void Save() {
        if (selectedSave == null) {
            print("saving new");
            SaveNew();
        }
        else {
            print("saving over selected");
            SaveSelected();
        }
        CopyCurrentGameToPlayerSave(saveID);
        SaveWorldItems();
    }
    public void SaveNew() {
        SkillsMenuUI skillsMenuUI = FindObjectOfType<SkillsMenuUI>();
        SetSaveID(DbCommands.GenerateUniqueID("PlayerGames", "SaveIDs", "SaveID"));
        string saveRef = FindObjectOfType<GameMenuUI>().transform
            .Find("Panel")
            .Find("SaveGameUI")
            .Find("SaveInput").GetComponent<InputField>().text;

        if (saveRef != "") {
            DbCommands.InsertTupleToTable("PlayerGames",
                                        saveID.ToString(),
                                        saveRef,
                                        playerName,
                                        playerPortraitPath,
                                        DateTime.Now.ToString(),
                                        sceneLoader.GetCurrentSceneName(),
                                        player.GetComponent<Transform>().position.x.ToString(),
                                        player.GetComponent<Transform>().position.y.ToString(),
                                        skillsMenuUI.GetSkillPointsSpent().ToString());
        }
    }

    public void CopyCurrentGameToPlayerSave(int saveID) {
        string saveIDstr = saveID.ToString();
        //Dialogues activated
        string[,] delFields = new string[,] {
                                                {"SaveIDs", saveIDstr }
        };
        DbCommands.DeleteTupleInTable("ActivatedDialogues", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("ActivatedDialogues",
                                                            new string[] { "SaveIDs" },
                                                            new string[] { saveIDstr },
                                                            new string[] { "0" });
        //Quests activated
        DbCommands.DeleteTupleInTable("QuestsActivated", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("QuestsActivated",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { saveIDstr },
                                                    new string[] { "0" });

        //Quests tasks completed
        DbCommands.DeleteTupleInTable("QuestTasksActivated", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("QuestTasksActivated",
                                                            new string[] { "SaveIDs" },
                                                            new string[] { saveIDstr },
                                                            new string[] { "0" });

        //Task parts completed
        DbCommands.DeleteTupleInTable("CompletedQuestTaskParts", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("CompletedQuestTaskParts",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { saveIDstr },
                                                    new string[] { "0" });

        //Discovered vocab
        DbCommands.DeleteTupleInTable("DiscoveredVocab", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("DiscoveredVocab",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { saveIDstr },
                                                    new string[] { "0" });

        //Discovered grammar
        DbCommands.DeleteTupleInTable("DiscoveredVocabGrammar", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("DiscoveredVocabGrammar",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { saveIDstr },
                                                    new string[] { "0" });

        //acquired vocabulary read proficiencies
        DbCommands.DeleteTupleInTable("AcquiredVocabReadSkills", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("AcquiredVocabReadSkills",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { saveIDstr },
                                                    new string[] { "0" });

        //acquired vocabulary write proficiencies
        DbCommands.DeleteTupleInTable("AcquiredVocabWriteSkills", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("AcquiredVocabWriteSkills",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { saveIDstr },
                                                    new string[] { "0" });

        //acquired grammar proficiencies
        DbCommands.DeleteTupleInTable("AcquiredGrammarSkills", delFields);
        DbCommands.InsertExistingValuesInSameTableWithNewPK("AcquiredGrammarSkills",
                                                    new string[] { "SaveIDs" },
                                                    new string[] { saveIDstr },
                                                    new string[] { "0" });

    }

    public void SaveSelected() {
        SetSaveID(selectedSave.ID.ToString());
        SkillsMenuUI skillsMenuUI = FindObjectOfType<SkillsMenuUI>();
        string[,] fieldVals = new string[,] {
                                                { "PlayerNames", player.GetComponent<PlayerCharacter>().GetMyName() },
                                                { "Dates", DateTime.Now.ToString() },
                                                { "LocationName", sceneLoader.GetCurrentSceneName() },
                                                { "LocationX", player.GetComponent<Transform>().position.x.ToString() },
                                                { "LocationY", player.GetComponent<Transform>().position.y.ToString() },
                                                { "SkillPointsSpent", skillsMenuUI.GetSkillPointsSpent().ToString() }
                                            };
        DbCommands.UpdateTableTuple("PlayerGames", "SaveIDs = " + saveID, fieldVals);
    }

    public Transform BuildSaveGameRow(string[] strArray) {
        int saveID = int.Parse(strArray[0].ToString());
        string saveRefStr = strArray[1].ToString();
        string playerNameStr = strArray[2].ToString();
        string dateStr = strArray[4].ToString();
        saveGame = Instantiate(saveGamePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
        saveGame.transform.Find("Panel").Find("SaveRef").gameObject.GetComponent<Text>().text = saveRefStr;
        saveGame.transform.Find("Panel").Find("CharName").GetComponent<Text>().text = playerNameStr;
        saveGame.transform.Find("Panel").Find("Date").GetComponent<Text>().text = dateStr;
        saveGame.GetComponent<ExistingSaveGame>().ID = saveID;
        saveGame.GetComponent<ExistingSaveGame>().SaveRef = saveRefStr;
        saveGame.GetComponent<ExistingSaveGame>().CharName = playerNameStr;
        saveGame.GetComponent<ExistingSaveGame>().SaveDate = dateStr;
        //saveGame.transform.SetParent(saveGamesList.transform, false);
        return saveGame.transform;
    }

    /*player prefs is used to store the selected save id and then the database is accessed to load the saved scene from the
     * game, other details are loaded using the loadSave function from the Start method. */
    public void LoadFromGameID() {
        print(selectedSave);
        saveID = selectedSave.ID;
        print("ready to load game from id: " + saveID);
        PlayerPrefsManager.SetSaveGame(saveID);
        string sceneName = DbCommands.GetFieldValueFromTable("PlayerGames", "LocationName", "SaveIDs = " + saveID);
        sceneLoader.LoadSceneByName(sceneName);
    }

    /*loads all the details of the saved game except for the scene, since loading a scene destroys saveGameManager obj*/
    public void LoadSave() {
        SkillsMenuUI skillsMenuUI = FindObjectOfType<SkillsMenuUI>();
        PlayerVitalsUI playerVitalsUI = FindObjectOfType<PlayerVitalsUI>();
        int saveID = PlayerPrefsManager.GetSaveGame();
        //print(DbCommands.GetConn());
        string[] playerSave = DbCommands.GetTupleFromTable("PlayerGames", " SaveIDs = " + saveID);
        print("Loading save reference: " 
            + playerSave[1] + " PlayerName: " 
            + playerSave[2] + " Portrait path: " 
            + playerSave[3] + " Save ID: "
            + saveID);
        SetPlayerName(playerSave[2]);
        SetPlayerPortraitPath(playerSave[3]);
        player.GetComponent<PlayerCharacter>().SetMyName(playerName);
        player.GetComponent<PlayerCharacter>().SetMyPortrait(playerPortraitPath);
        playerVitalsUI.Portrait.sprite = player.GetComponent<PlayerCharacter>().GetMyPortrait();
        float playerXpos = float.Parse(playerSave[6]);
        float playerYpos = float.Parse(playerSave[7]);
        //print(playerXpos + ", " + playerYpos);
        Transform playerTransform = player.GetComponent<Transform>();
        playerTransform.localPosition = new Vector2(playerXpos, playerYpos);
        Camera.main.transform.position =  new Vector3(playerTransform.position.x, playerTransform.position.y, Camera.main.transform.position.z);
        skillsMenuUI.SetSkillPointsSpent(int.Parse(playerSave[8]));
        Time.timeScale = 1;

        //Need to update currentgame with data from player save
        CopyPlayerSaveToCurrentGame(playerSave);
        if (saveID != 0) {
            LoadWorldItems();
            LoadPrefabQuests();
        }

        print("game loaded");
    }

    public void CopyPlayerSaveToCurrentGame(string[] playerGameData) {
        string saveID = playerGameData[0];
        if (saveID != "0") {
            //PlayerGames tbl
            string[,] pgFields = new string[,] {
                                                { "PlayerNames", playerGameData[2] },
                                                { "PortraitImages", playerGameData[3] },
                                                { "LocationName", playerGameData[5] }
                                             };
            DbCommands.UpdateTableTuple("PlayerGames", "SaveIDs = 0", pgFields);

            string[,] delFields = new string[,] {
                                                {"SaveIDs", "0" }
            };
            //Dialogues activated
            DbCommands.DeleteTupleInTable("ActivatedDialogues", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("ActivatedDialogues",
                                                                new string[] { "SaveIDs" },
                                                                new string[] { "0" },
                                                                new string[] { playerGameData[0] });
            //Quests activated
            DbCommands.DeleteTupleInTable("QuestsActivated", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("QuestsActivated",
                                                        new string[] { "SaveIDs" },
                                                        new string[] { "0" },
                                                        new string[] { playerGameData[0] });

            //Quests tasks completed
            DbCommands.DeleteTupleInTable("QuestTasksActivated", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("QuestTasksActivated",
                                                                new string[] { "SaveIDs" },
                                                                new string[] { "0" },
                                                                new string[] { playerGameData[0] });
            //Task parts completed
            DbCommands.DeleteTupleInTable("CompletedQuestTaskParts", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("CompletedQuestTaskParts",
                                                        new string[] { "SaveIDs" },
                                                        new string[] { "0" },
                                                        new string[] { playerGameData[0] });

            //discovered vocab
            DbCommands.DeleteTupleInTable("DiscoveredVocab", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("DiscoveredVocab",
                                                        new string[] { "SaveIDs" },
                                                        new string[] { "0" },
                                                        new string[] { playerGameData[0] });

            //discovered grammar
            DbCommands.DeleteTupleInTable("DiscoveredVocabGrammar", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("DiscoveredVocabGrammar",
                                                        new string[] { "SaveIDs" },
                                                        new string[] { "0" },
                                                        new string[] { playerGameData[0] });

            //acquired vocabulary read proficiencies
            DbCommands.DeleteTupleInTable("AcquiredVocabReadSkills", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("AcquiredVocabReadSkills",
                                                        new string[] { "SaveIDs" },
                                                        new string[] { "0" },
                                                        new string[] { playerGameData[0] });

            //acquired vocabulary write proficiencies
            DbCommands.DeleteTupleInTable("AcquiredVocabWriteSkills", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("AcquiredVocabWriteSkills",
                                                        new string[] { "SaveIDs" },
                                                        new string[] { "0" },
                                                        new string[] { playerGameData[0] });

            //acquired grammar proficiencies
            DbCommands.DeleteTupleInTable("AcquiredGrammarSkills", delFields);
            DbCommands.InsertExistingValuesInSameTableWithNewPK("AcquiredGrammarSkills",
                                                        new string[] { "SaveIDs" },
                                                        new string[] { "0" },
                                                        new string[] { playerGameData[0] });
        }
    }

    private void SaveWorldItems() {
        string saveIDstr = saveID.ToString();
        string[,] delFields = new string[,] {
            {"SaveIDs", saveIDstr}
        };
        DbCommands.DeleteTupleInTable("SavedWorldItems", delFields);
        WorldItems worldItems = FindObjectOfType<WorldItems>();
        worldItems.SetWorldItemsList();
        List<string[]> worldItemsList = worldItems.GetWorldItemsList();
        foreach (string[] worldItemData in worldItemsList) {
            DbCommands.InsertTupleToTable("SavedWorldItems",
                saveIDstr,
                worldItemData[0],
                worldItemData[1],
                worldItemData[2],
                worldItemData[3],
                sceneLoader.GetCurrentSceneName(),
                worldItemData[4],
                worldItemData[5]);
        }
    }

    private void LoadWorldItems() {
        string saveIDstr = PlayerPrefsManager.GetSaveGame().ToString();
        WorldItems worldItems = FindObjectOfType<WorldItems>();
        worldItems.DestroyWorldItems();
        List<string[]> newWorldItemsList = new List<string[]>();
        //Debugging.PrintDbQryResults(DbQueries.GetSavedWorldItemsQry(saveIDstr, sceneLoader.GetCurrentSceneName()), saveIDstr, sceneLoader.GetCurrentSceneName());
        DbCommands.GetDataStringsFromQry(DbQueries.GetSavedWorldItemsQry(saveIDstr, sceneLoader.GetCurrentSceneName()), out newWorldItemsList, saveIDstr, sceneLoader.GetCurrentSceneName());
        foreach (string[] worldItemData in newWorldItemsList) {
            string prefabPath = worldItemData[5];
            UnityEngine.Object worldItemPrefab = Resources.Load(prefabPath);
            GameObject worldItem = Instantiate(worldItemPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            string[] parentPath = worldItemData[3].Split('/');
            Transform parentTransform = GameObject.Find(parentPath[1]).transform;
            for (int i = 2; i < parentPath.Length -1; i++) {
                parentTransform = parentTransform.Find(parentPath[i]);
            }
            worldItem.transform.SetParent(parentTransform, false);
            worldItem.transform.position = new Vector3(float.Parse(worldItemData[0]), float.Parse(worldItemData[1]), float.Parse(worldItemData[2]));
            worldItem.name = worldItemData[4];
        }

    }

    private void LoadPrefabQuests() {
        List<string[]> prefabQuestsPathList = new List<string[]>();
        string saveIDstr = PlayerPrefsManager.GetSaveGame().ToString();
        DbCommands.GetDataStringsFromQry(DbQueries.GetPathsForActivePrefabQuestParts(saveIDstr), out prefabQuestsPathList);
        foreach (string[] questPrefabArray in prefabQuestsPathList) {
            string prefabPath = questPrefabArray[0];
            UnityEngine.Object prefabObj = Resources.Load(prefabPath);
            GameObject questPrefab = Instantiate(prefabObj, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            QuestsController questsController = FindObjectOfType<QuestsController>();
            questPrefab.GetComponent<QuestTaskPart>().InitialiseMe(questPrefabArray[1], questPrefabArray[2], questPrefabArray[3]);
            questPrefab.transform.SetParent(questsController.transform, false);
        }
    }

    public void DeleteSave() {
        int id = selectedSave.ID;
        Destroy(selectedSave.gameObject);
        string[,] ruleFields = new string[,] { { "SaveIDs", id.ToString() } };
        DbCommands.DeleteTupleInTable("PlayerGames", ruleFields);
    }

    public void ShowSelectedSaveInInput() {
        newSaveInput.transform.Find("Placeholder").GetComponent<Text>().text = SelectedSave.SaveRef;
    }

    public void ShowDefaultTextInInput() {
        newSaveInput.transform.Find("Placeholder").GetComponent<Text>().text = "... or enter a new save";
    }
}
