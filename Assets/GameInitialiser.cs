using System;
using UnityEngine;
using DbUtilities;
using UnityUtilities;
using DataUI;

/// <summary> 
/// Used in conjunction with objects that should be initialised before other
/// parts of the game run. Ensures that classes dependant on each with methods 
/// which need to be run at start of game are initialised and their functions 
/// run in the correct order.
/// </summary>
public class GameInitialiser : MonoBehaviour {
    DbSetup dbSetup;
    PlayerSavesController playerSavesController;
    // Use this for initialization
    private static string DB_PATH = "/GameDb.s3db";
    DataUIController dataUI;
    void Awake() {
        dataUI = FindObjectOfType<DataUIController>();
        if (dataUI != null) {
            dataUI.HideComponents();
        }
    }
    void Start() {
        DbCommands.SetConnectionString("URI=file:" + Application.dataPath + DB_PATH);
        dbSetup = new DbSetup();
        //dbSetup.DropTable("VocabTagged");
        //DbCommands.UpdateTableField("ActivatedDialogues", "Completed", "0");
        dbSetup.CreateTables();
        //print("tables created");
        //dbSetup.CopyTable("PlayerGames", "Copied");
        //string[,] delFields = { { "EnglishText", "''" }, { "WelshText", "''" } };
        //DbCommands.DeleteTupleInTable("VocabTranslations", delFields);
        //dbSetup.ReplaceTable("PlayerGames");
        
        //Debugging.PrintDbTable("VocabTranslations");
        DbCommands.InsertTupleToTable("PlayerGames", "0", "Current Game", "Current player", "No path", DateTime.Now.ToString(), "Start", "0", "0", "0");
        DbCommands.InsertTupleToTable("PlayerGames", "-1", "New game", "New player", "No path", DateTime.Now.ToString(), "Start", "0", "0", "0");
        playerSavesController = FindObjectOfType<PlayerSavesController>();
        // when playerSavesController is first initialised it tries to load the
        // game, manually initialising rather than using Start() or Awake()
        // ensures it doesn't try to load it before the connection string is set
        // up.
        playerSavesController.ManuallyInitialise();
        Debugging.PrintDbTable("ActivatedDialogues");

    }
	
}
