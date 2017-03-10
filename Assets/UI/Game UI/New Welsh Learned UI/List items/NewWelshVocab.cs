using UnityEngine;
using UnityEngine.UI;

public class NewWelshVocab : MonoBehaviour {

    string enVocab;
    string cyVocab;
    

    public void InitialiseMe(string engVocab, string cymVocab) {
        transform.FindChild("EnglishVocabulary").GetComponent<Text>().text = engVocab;
        transform.FindChild("WelshVocabulary").GetComponent<Text>().text = cymVocab;
        enVocab = engVocab;
        cyVocab = cymVocab;
        if (enVocab == cyVocab) {
            Destroy(transform.FindChild("WelshVocabulary").gameObject);
        }
    }

    public void DisplayRelatedGrammar() {

    }

}

