using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using System.Collections.Generic;
using UIUtilities;

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
            print("glist null");
            DisplayRelatedGrammar();
        } else {
            HideRelatedGrammar();
            print(myGrammarList + "glist not null");
        }
    }

    public void DisplayRelatedGrammar() {
        List<string[]> grammarData = new List<string[]>();
        print(DbQueries.GetGrammarRelatedToVocab(enVocab, cyVocab));
        DbCommands.GetDataStringsFromQry(DbQueries.GetGrammarRelatedToVocab(enVocab, cyVocab), out grammarData, enVocab, cyVocab);
        GameObject grammarList = Instantiate(grammarListPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        grammarList.transform.SetParent(gameObject.transform, false);
        foreach (string[] grammarRuleData in grammarData) {
            GameObject grammarRule = Instantiate(relatedGrammarPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            grammarRule.transform.Find("GrammarIntro").GetComponent<Text>().text = grammarRuleData[0];
            grammarRule.transform.Find("GrammarBody").GetComponent<Text>().text = grammarRuleData[1];
            grammarRule.transform.SetParent(grammarList.transform, false);
            GetComponentInChildren<Button>().GetComponentInChildren<Image>().GetComponent<Transform>().Rotate(0, 0, -90);
        }
        myGrammarList = grammarList;
        Canvas.ForceUpdateCanvases();
    }

    public void HideRelatedGrammar() {
        GetComponentInChildren<Button>().GetComponentInChildren<Image>().GetComponent<Transform>().Rotate(0, 0, 90);
        Destroy(myGrammarList);
        myGrammarList = null;
    }

}

