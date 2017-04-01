using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;
using UnityEngine.UI;

public class WelshSkillsListUI : UIController {
    List<string[]> proficiencyList;
    public GameObject WelshVocabPrefab;
    Transform welshSkillsList;
	// Use this for initialization
	void Start () {
        welshSkillsList = GetComponentInChildren<VerticalLayoutGroup>().transform;
        proficiencyList = new List<string[]>();
        DbCommands.GetDataStringsFromQry(DbQueries.GetWelshThresholdsQry(), out proficiencyList);
    }
	
	private Transform BuildVocab(string[] vocabData) {
        string eng = vocabData[0];
        string cym = vocabData[1];
        string correctTally = vocabData[2];
        NewWelshVocab WelshVocab = (
            Instantiate(WelshVocabPrefab, new Vector3(0f, 0f), Quaternion.identity)
            ).GetComponent<NewWelshVocab>();
        WelshVocab.InitialiseMe(eng, cym);
        return WelshVocab.transform;
    }

    public string GetProficiencyString(int tallyCorrect) {
        string profRet = "Novice";
        foreach(string[] proficiencyArray in proficiencyList) {
            int threshold = int.Parse(proficiencyArray[1]);
            string proficiencyName = proficiencyArray[0];
            if (tallyCorrect >= threshold) {
                profRet = proficiencyName;
                break;
            }
        }
        return profRet;
    }

    public void DisplayWelshSkills() {
        DisplayWelshVocabList();
    }

    public void DisplayWelshVocabList() {
        FillDisplayFromDb(DbQueries.GetPlayerVocabSkillsQry(), welshSkillsList, BuildVocab);
    }

    public void DisplayWelshGrammarList() {
        FillDisplayFromDb(DbQueries.GetPlayerVocabSkillsQry(), welshSkillsList, BuildVocab);
    }
}
