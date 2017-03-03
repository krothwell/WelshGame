using UnityEngine;
using UnityEngine.UI;
using System;
//using StartMenuUI;
using DbUtilities;
using GameUI;
using UnityUtilities;

public class PlayerSavesController : MonoBehaviour {
    GameObject player;
    GameObject saveGameManager;
    public GameObject saveGamePrefab;
    public string playerName, playerPortraitPath;
    public int saveID;
    GameObject saveGame;
    GameObject saveGamesList;
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
        if (FindObjectOfType<PlayerController>() != null) {
            player = FindObjectOfType<PlayerController>().gameObject;
            LoadSave();
        }
    }

    // Use this for initialization
    void Start() {
        sceneLoader = FindObjectOfType<SceneLoader>();
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
                                                { "LocationX", "0.0" },
                                                { "LocationY", "0.0" }
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

        PlayerPrefsManager.SetSaveGame(0);

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
    }
    public void SaveNew() {
        SetSaveID(DbCommands.GenerateUniqueID("PlayerGames", "SaveIDs", "SaveID"));
        string saveRef = FindObjectOfType<EscMenuUI>().transform
            .FindChild("Panel")
            .FindChild("SaveOptions")
            .FindChild("SaveInput").GetComponent<InputField>().text;

        if (saveRef != "") {
            DbCommands.InsertTupleToTable("PlayerGames",
                                        saveID.ToString(),
                                        saveRef,
                                        playerName,
                                        playerPortraitPath,
                                        DateTime.Now.ToString(),
                                        sceneLoader.GetCurrentSceneName(),
                                        player.GetComponent<Transform>().position.x.ToString(),
                                        player.GetComponent<Transform>().position.y.ToString());
        }

        CopyCurrentGameToPlayerSave(saveID);
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
    }

    public void SaveSelected() {
        SetSaveID(selectedSave.ID.ToString());
        string[,] fieldVals = new string[,] {
                                                { "PlayerNames", player.GetComponent<PlayerController>().GetMyName() },
                                                { "Dates", DateTime.Now.ToString() },
                                                { "LocationName", sceneLoader.GetCurrentSceneName() },
                                                { "LocationX", player.GetComponent<Transform>().position.x.ToString() },
                                                { "LocationY", player.GetComponent<Transform>().position.y.ToString() }
                                            };
        DbCommands.UpdateTableTuple("PlayerGames", "SaveIDs = " + saveID, fieldVals);
        CopyCurrentGameToPlayerSave(saveID);
    }

    public Transform BuildSaveGameRow(string[] strArray) {
        int saveID = int.Parse(strArray[0].ToString());
        string saveRefStr = strArray[1].ToString();
        string playerNameStr = strArray[2].ToString();
        string dateStr = strArray[4].ToString();
        saveGame = Instantiate(saveGamePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
        saveGame.transform.FindChild("Panel").FindChild("SaveRef").gameObject.GetComponent<Text>().text = saveRefStr;
        saveGame.transform.FindChild("Panel").FindChild("CharName").GetComponent<Text>().text = playerNameStr;
        saveGame.transform.FindChild("Panel").FindChild("Date").GetComponent<Text>().text = dateStr;
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
        int id = selectedSave.ID;
        print("ready to load game from id: " + id);
        PlayerPrefsManager.SetSaveGame(id);
        string sceneName = DbCommands.GetFieldValueFromTable("PlayerGames", "LocationName", "SaveIDs = " + id);
        sceneLoader.LoadSceneByName(sceneName);
    }

    /*loads all the details of the saved game except for the scene, since loading a scene destroys saveGameManager obj*/
    public void LoadSave() {
        int id = PlayerPrefsManager.GetSaveGame();
        print(DbCommands.GetConn());
        string[] playerSave = DbCommands.GetTupleFromTable("PlayerGames", " SaveIDs = " + id);
        print("Loading save reference: " 
            + playerSave[1] + " PlayerName: " 
            + playerSave[2] + " Portrait path: " 
            + playerSave[3]);
        SetPlayerName(playerSave[2]);
        SetPlayerPortraitPath(playerSave[3]);
        player.GetComponent<PlayerController>().SetMyName(playerName);
        player.GetComponent<PlayerController>().SetMyPortrait(playerPortraitPath);
        float playerXpos = float.Parse(playerSave[6]);
        float playerYpos = float.Parse(playerSave[7]);
        player.GetComponent<Transform>().position = new Vector2(playerXpos, playerYpos);
        Camera.main.transform.position =  new Vector3(playerXpos, playerYpos, Camera.main.transform.position.z);
        Time.timeScale = 1;

        //Need to update currentgame with data from player save
        
        CopyPlayerSaveToCurrentGame(playerSave);
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
            Debugging.PrintDbTable("CompletedQuestTaskParts");
        }
    }

    public void DeleteSave() {
        int id = selectedSave.ID;
        Destroy(selectedSave.gameObject);
        string[,] ruleFields = new string[,] { { "SaveIDs", id.ToString() } };
        DbCommands.DeleteTupleInTable("PlayerGames", ruleFields);
    }

    public void ShowSelectedSaveInInput() {
        newSaveInput.transform.FindChild("Placeholder").GetComponent<Text>().text = SelectedSave.SaveRef;
    }

    public void ShowDefaultTextInInput() {
        newSaveInput.transform.FindChild("Placeholder").GetComponent<Text>().text = "... or enter a new save";
    }
}
