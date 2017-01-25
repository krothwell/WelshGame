using UnityEngine;
using System.Collections;

public class Quests : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void ActivateDialogue(string id) {
        DbSetup.UpdateTableField("Dialogues", "Active", "1", "DialogueIDs = " + id);
    }
}
