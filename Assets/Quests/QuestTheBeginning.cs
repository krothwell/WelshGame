using UnityEngine;
using System.Collections;

public class QuestTheBeginning : MonoBehaviour {
    Quests quests;
    LowerUI lowerUI;
    public GameObject faerie;
	// Use this for initialization
	void Start () {
        quests = FindObjectOfType<Quests>();
        //quests.ActivateDialogue("1");
        lowerUI = FindObjectOfType<LowerUI>();
        lowerUI.SetInUse();
        lowerUI.ProcessCharacterDialogue(faerie.GetComponent<Character>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
