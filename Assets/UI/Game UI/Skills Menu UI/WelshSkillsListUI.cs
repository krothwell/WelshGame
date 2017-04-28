using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;
using UnityEngine.UI;

public class WelshSkillsListUI : UIController {
    enum ListDisplay {
        vocabulary,
        grammar
    }
    ListDisplay listDisplay;
    
    public GameObject WelshVocabPrefab, WelshGrammarPrefab;
    Transform welshSkillsList;
    GameObject toggleListBtn, listToggledToLbl;
	// Use this for initialization
	void Awake () {
        listDisplay = ListDisplay.vocabulary;
        toggleListBtn = GetPanel().transform.FindChild("ToggleListBtn").gameObject;
        listToggledToLbl = GetPanel().transform.FindChild("ListToggledToLbl").gameObject;
        welshSkillsList = GetPanel().transform.FindChild("ScrollWindow").FindChild("WelshSkillsList");
        
    }
	
	private Transform BuildVocab(string[] vocabData) {
        string eng = vocabData[0];
        string cym = vocabData[1];
        string readCorrectTally = vocabData[2];
        string writeCorrectTally = vocabData[3];
        WelshVocab welshVocab = (
            Instantiate(WelshVocabPrefab, new Vector3(0f, 0f), Quaternion.identity)
            ).GetComponent<WelshVocab>();
        welshVocab.InitialiseMe(eng, cym,readCorrectTally,writeCorrectTally);
        return welshVocab.transform;
    }

    private Transform BuildGrammar(string[] grammarData) {
        string id = grammarData[0];
        string shortDescription = grammarData[1];
        string longDescription = grammarData[2];
        string correctTally = grammarData[3];
        WelshGrammar welshGrammar = (
            Instantiate(WelshGrammarPrefab, new Vector3(0f, 0f), Quaternion.identity)
            ).GetComponent<WelshGrammar>();
        welshGrammar.InitialiseMe(id, shortDescription, longDescription, correctTally);
        return welshGrammar.transform;
    }

    public void ToggleList() {
        if(listDisplay == ListDisplay.vocabulary) {
            listDisplay = ListDisplay.grammar;
            toggleListBtn.GetComponent<Text>().text = "Vocabulary";
            listToggledToLbl.GetComponent<Text>().text = "Grammar";
            DisplayWelshGrammarList();

        } else {
            listDisplay = ListDisplay.vocabulary;
            toggleListBtn.GetComponent<Text>().text = "Grammar";
            listToggledToLbl.GetComponent<Text>().text = "Vocabulary";
            DisplayWelshVocabList();
        }
    }

    public void DisplayWelshSkills() {
        DisplayWelshVocabList();
    }

    public void DisplayWelshVocabList() {
        FillDisplayFromDb(DbQueries.GetPlayerVocabSkillsQry(), welshSkillsList, BuildVocab);
    }

    public void DisplayWelshGrammarList() {
        FillDisplayFromDb(DbQueries.GetPlayerGrammarSkillsQry(), welshSkillsList, BuildGrammar);
    }
}
