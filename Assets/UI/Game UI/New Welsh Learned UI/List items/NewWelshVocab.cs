using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using System.Collections.Generic;
using UIUtilities;
using UnityUtilities;

public class NewWelshVocab : MonoBehaviour {
    public GameObject relatedGrammarPrefab, grammarListPrefab;
    protected GameObject myGrammarList;
    protected string enVocab;
    protected string cyVocab;
    

    public void InitialiseMe(string engVocab, string cymVocab) {
        transform.Find("EnglishVocabulary").GetComponent<Text>().text = engVocab;
        transform.Find("WelshVocabulary").GetComponent<Text>().text = cymVocab;
        enVocab = engVocab;
        cyVocab = cymVocab;
        if (enVocab == cyVocab) {
            Destroy(transform.Find("WelshVocabulary").gameObject);
        }
    }

    public void ToggleRelatedGrammar() {
        if (myGrammarList == null) {
            DisplayRelatedGrammar();
        } else {
            HideRelatedGrammar();
        }
    }

    public void DisplayRelatedGrammar() {
        List<string[]> grammarData = new List<string[]>();
        print(DbQueries.GetGrammarRelatedToVocab(enVocab, cyVocab));
        Debugging.PrintDbQryResults(DbQueries.GetGrammarRelatedToVocab(enVocab, cyVocab), enVocab, cyVocab);
        
        DbCommands.GetDataStringsFromQry(DbQueries.GetGrammarRelatedToVocab(enVocab, cyVocab), out grammarData, enVocab, cyVocab);
        GameObject grammarList = Instantiate(grammarListPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        grammarList.transform.SetParent(gameObject.transform, false);
        foreach (string[] grammarRuleData in grammarData) {
            GameObject grammarRule = Instantiate(relatedGrammarPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            grammarRule.transform.Find("GrammarIntro").GetComponent<Text>().text = grammarRuleData[0];
            grammarRule.transform.Find("GrammarBody").GetComponent<Text>().text = grammarRuleData[1];
            grammarRule.transform.SetParent(grammarList.transform, false);
        }
        GetComponentInChildren<Button>().GetComponentInChildren<Image>().GetComponent<Transform>().Rotate(0, 0, -90);
        myGrammarList = grammarList;
        Canvas.ForceUpdateCanvases();
        Debugging.PrintDbTable("DiscoveredVocabGrammar");
    }

    public void HideRelatedGrammar() {
        GetComponentInChildren<Button>().GetComponentInChildren<Image>().GetComponent<Transform>().Rotate(0, 0, 90);
        Destroy(myGrammarList);
        myGrammarList = null;
    }

}

