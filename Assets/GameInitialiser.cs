using System;
using UnityEngine;
using DbUtilities;
using UnityUtilities;

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

    void Awake() {
        DbCommands.SetConnectionString("URI=file:" + Application.dataPath + DB_PATH);
        dbSetup = new DbSetup();
        //Debugging.PrintDbTable("Copied");
        
        //dbSetup.ReplaceTable("QuestsActivated");
        //DbCommands.UpdateTableField("QuestsActivated", "Completed", "0");
        Debugging.PrintDbTable("QuestsActivated");
        DbCommands.InsertTupleToTable("PlayerGames", "0", "Current Game", "Current player", "No path", DateTime.Now.ToString(), "Start", "0", "0");
        DbCommands.InsertTupleToTable("PlayerGames", "-1", "New game", "New player", "No path", DateTime.Now.ToString(), "Start", "0", "0");
        playerSavesController = FindObjectOfType<PlayerSavesController>();
        // when playerSavesController is first initialised it tries to load the
        // game, manually initialising rather than using Start() or Awake()
        // ensures it doesn't try to load it before the connection string is set
        // up.
        playerSavesController.ManuallyInitialise();
    }
	
}
