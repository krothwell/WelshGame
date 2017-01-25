using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using UnityEditor.SceneManagement;

public class SaveGameManager : MonoBehaviour {
    GameObject player;
    GameObject saveGameManager;
    public GameObject saveGamePrefab;
    public string playerName, playerPortraitPath;
    public int saveID;
    GameObject saveGame;
    GameObject saveGamesList;
    SceneLoader sceneLoader;

    private SaveGame selectedSave;
    public SaveGame SelectedSave {
        get { return selectedSave; }
        set { selectedSave = value; }
    }

    public GameObject newSaveInput;

    void Awake() {
        if (FindObjectOfType<MainCharacter>() != null) {
            player = FindObjectOfType<MainCharacter>().gameObject;
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
        NewGame newGame = FindObjectOfType<NewGame>();
        SetPlayerName(newGame.GetNameInput());
        SetPlayerPortraitPath(newGame.GetSelectedPortraitPath());
        print(playerPortraitPath);
        int saves = DbSetup.GetCountFromTable("PlayerGames");
        if (saves > 0) {
            string[,] ruleFields = new string[,] { { "SaveRefs", "CurrentGame"}};
            DbSetup.DeleteTupleInTable("PlayerGames", ruleFields);
        }
        SetSaveID(DbSetup.GenerateUniqueID("PlayerGames", "SaveIDs", "SaveID"));
        DbSetup.InsertTupleToTable("PlayerGames",
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
        SetSaveID(DbSetup.GenerateUniqueID("PlayerGames", "SaveIDs", "SaveID"));
        string saveRef = FindObjectOfType<UI>().transform.FindChild("GameUI").transform
            .FindChild("GameOptions")
            .FindChild("Panel")
            .FindChild("SaveOptions")
            .FindChild("SaveInput").GetComponent<InputField>().text;

        if (saveRef != "") {
            DbSetup.InsertTupleToTable("PlayerGames",
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
                                                { "PlayerNames", player.GetComponent<MainCharacter>().GetMyName() },
                                                { "Dates", DateTime.Now.ToString() },
                                                { "LocationName", sceneLoader.GetCurrentSceneName() },
                                                { "LocationX", player.GetComponent<Transform>().position.x.ToString() },
                                                { "LocationY", player.GetComponent<Transform>().position.y.ToString() }
                                            };
        DbSetup.UpdateTableTuple("PlayerGames", "SaveIDs = " + saveID, fieldVals);
    }

    public Transform BuildSaveGameRow(IDataReader _dbr) {
        int saveID = int.Parse(_dbr["SaveIDs"].ToString());
        string saveRefStr = (_dbr["SaveRefs"].ToString());
        string playerNameStr = (_dbr["PlayerNames"].ToString());
        string dateStr = (_dbr["Dates"].ToString());
        saveGame = Instantiate(saveGamePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        saveGame.transform.FindChild("Panel").FindChild("SaveRef").gameObject.GetComponent<Text>().text = saveRefStr;
        saveGame.transform.FindChild("Panel").FindChild("CharName").GetComponent<Text>().text = playerNameStr;
        saveGame.transform.FindChild("Panel").FindChild("Date").GetComponent<Text>().text = dateStr;
        saveGame.GetComponent<SaveGame>().ID = saveID;
        saveGame.GetComponent<SaveGame>().SaveRef = saveRefStr;
        saveGame.GetComponent<SaveGame>().CharName = playerNameStr;
        saveGame.GetComponent<SaveGame>().SaveDate = dateStr;
        //saveGame.transform.SetParent(saveGamesList.transform, false);
        return saveGame.transform;
    }

    /*player prefs is used to store the selected save id and then the database is accessed to load the saved scene from the
     * game, other details are loaded using the loadSave function from the Start method. */
    public void LoadFromGameID() {
        int id = selectedSave.ID;
        print("ready to load game from id: " + id);
        PlayerPrefsManager.SetSaveGame(id);
        string sceneName = DbSetup.GetFieldValueFromTable("PlayerGames", "LocationName", "SaveIDs = " + id);
        sceneLoader.LoadSceneByName(sceneName);
    }

    /*loads all the details of the saved game except for the scene, since loading a scene destroys saveGameManager obj*/
    public void LoadSave() {
        int id = PlayerPrefsManager.GetSaveGame();
        print(id);
        string[] playerSave = DbSetup.GetTupleFromTable("PlayerGames", " SaveIDs = " + id);
        print("Loading safe reference: " 
            + playerSave[1] + " PlayerName: " 
            + playerSave[2] + " Portrait path: " 
            + playerSave[3]);
        SetPlayerName(playerSave[2]);
        SetPlayerPortraitPath(playerSave[3]);
        player.GetComponent<MainCharacter>().SetMyName(playerName);
        player.GetComponent<MainCharacter>().SetMyPortrait(playerPortraitPath);
        
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
        DbSetup.DeleteTupleInTable("PlayerGames", ruleFields);
    }

    public void ShowSelectedSaveInInput() {
        newSaveInput.transform.FindChild("Placeholder").GetComponent<Text>().text = SelectedSave.SaveRef;
    }

    public void ShowDefaultTextInInput() {
        newSaveInput.transform.FindChild("Placeholder").GetComponent<Text>().text = "... or enter a new save";
    }
}
