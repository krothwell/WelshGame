using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;
public class GameInitialiser : MonoBehaviour {
    DbSetup dbSetup;
    PlayerSavesController playerSavesController;
    // Use this for initialization
    private static string DB_PATH = "/GameDb.s3db";

    void Awake() {
        DbCommands.SetConnectionString("URI=file:" + Application.dataPath + DB_PATH);
        print(DbCommands.GetConn());
        dbSetup = new DbSetup();
        playerSavesController = FindObjectOfType<PlayerSavesController>();
        playerSavesController.ManuallyInitialise();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
