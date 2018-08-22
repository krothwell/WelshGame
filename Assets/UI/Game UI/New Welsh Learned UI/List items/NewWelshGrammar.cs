using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DbUtilities;

public class NewWelshGrammar : MonoBehaviour {

    public GameObject grammarBodyPrefab, grammarListPrefab;
    private GameObject myGrammarList;
    //private string grammarID;
    private string grammarDetails;


    public void InitialiseMe(string grammarIDstr, string grammarSummaryStr, string grammarDetailsStr) {
        transform.Find("GrammarSummary").GetComponent<Text>().text = grammarSummaryStr;
        //grammarID = grammarIDstr;
        grammarDetails = grammarDetailsStr;
    }

    public void ToggleGrammarDescription() {
        if (myGrammarList == null) {
            DisplayGrammarDescription();
        }
        else {
            HideRelatedGrammar();
        }
    }

    public void DisplayGrammarDescription() {
        GameObject grammarList = Instantiate(grammarListPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        grammarList.transform.SetParent(gameObject.transform, false);
        GameObject grammarBody = Instantiate(grammarBodyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        grammarBody.GetComponent<Text>().text = grammarDetails;
        grammarBody.transform.SetParent(grammarList.transform, false);
        myGrammarList = grammarList;
        GetComponentInChildren<Button>().GetComponentInChildren<Image>().GetComponent<Transform>().Rotate(0, 0, -90);
        Canvas.ForceUpdateCanvases();
    }

    public void HideRelatedGrammar() {
        GetComponentInChildren<Button>().GetComponentInChildren<Image>().GetComponent<Transform>().Rotate(0, 0, 90);
        Destroy(myGrammarList);
        myGrammarList = null;
    }
}
