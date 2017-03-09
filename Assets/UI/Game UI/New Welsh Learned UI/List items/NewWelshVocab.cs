using UnityEngine;
using UnityEngine.UI;

public class NewWelshVocab : MonoBehaviour {

    private string[] relatedGrammarIDs;
    

    public void InitialiseMe(string vocab) {
        transform.FindChild("Vocabulary").GetComponent<Text>().text = vocab;
    }

    

    public void SetRelatedGrammarIDs() {

    }

}

