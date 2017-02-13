using UnityEngine;
using UnityEngine.UI;
using System;
using StartMenuUI;
using DbUtilities;

public class PlayerSavesController : MonoBehaviour {
    GameObject player;
    GameObject saveGameManager;
    public GameObject saveGamePrefab;
    public string playerName, playerPortraitPath;
    public int saveID;
    GameObject saveGame;
    GameObject saveGamesList;
    SceneLoader sceneLoader;

    private ExistingSaveGameBtn selectedSave;
    public ExistingSaveGameBtn SelectedSave {
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

    // Update is called once per frame
    void Update() {

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

    public void StartNewGame() {
        /*only used in main menu, the new game inputfield is found and used to set the character name
         * a save ID is generated and a save reference called autosave is overwritten or created if it doesn't exist
         * with the new character name and save id. This will be used for reference when changing levels. 
         */
        NewGameMenuUI newGame = FindObjectOfType<NewGameMenuUI>();
        SetPlayerName(newGame.GetNameInput());
        SetPlayerPortraitPath(newGame.GetSelectedPortraitPath());
        print(playerPortraitPath);
        int saves = DbCommands.GetCountFromTable("PlayerGames");
        if (saves > 0) {
            string[,] ruleFields = new string[,] { { "SaveRefs", "CurrentGame"}};
            DbCommands.DeleteTupleInTable("PlayerGames", ruleFields);
        }
        SetSaveID(DbCommands.GenerateUniqueID("PlayerGames", "SaveIDs", "SaveID"));
        DbCommands.InsertTupleToTable("PlayerGames",
                                    saveID.ToString(),
                                    "CurrentGame",
                                    playerName,
                                    playerPortraitPath,
                                    DateTime.Now.ToString(),
                                    "Start",
                                    "0.0", 
                                    "0.0");
        PlayerPrefsManager.SetSaveGame(saveID);
    }

    /*used in game to create a new save with player referenced save slot */
    public void SaveNew() {
        SetSaveID(DbCommands.GenerateUniqueID("PlayerGames", "SaveIDs", "SaveID"));
        string saveRef = FindObjectOfType<UIController>().transform.FindChild("GameUI").transform
            .FindChild("GameOptions")
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
                                        "Start",
                                        player.GetComponent<Transform>().position.x.ToString(),
                                        player.GetComponent<Transform>().position.y.ToString());
        }
    }

    public void Save() {
        if (selectedSave == null) {
            SaveNew();
        } else {
            SaveSelected();
        }
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
        saveGame.GetComponent<ExistingSaveGameBtn>().ID = saveID;
        saveGame.GetComponent<ExistingSaveGameBtn>().SaveRef = saveRefStr;
        saveGame.GetComponent<ExistingSaveGameBtn>().CharName = playerNameStr;
        saveGame.GetComponent<ExistingSaveGameBtn>().SaveDate = dateStr;
        //saveGame.transform.SetParent(saveGamesList.transform, false);
        return saveGame.transform;
    }

    /*player prefs is used to store the selected save id and then the database is accessed to load the saved scene from the
     * game, other details are loaded using the loadSave function from the Start method. */
    public void LoadFromGameID() {
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
