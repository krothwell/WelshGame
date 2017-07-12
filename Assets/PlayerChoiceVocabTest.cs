using UnityEngine;
using UnityEngine.UI;



namespace DataUI.ListItems {
    public class PlayerChoiceVocabTest : MonoBehaviour {
        Text englishText, welshText, idText;
        private string english;
        public string English {
            get { return english; }
            set { english = value; }
        }

        private string welsh;
        public string Welsh {
            get { return welsh; }
            set { welsh = value; }
        }

        public void InitialiseDisplay(string enTxt, string cyTxt, string idTxt) {
            englishText = transform.Find("EnglishText").GetComponent<Text>();
            welshText = transform.Find("WelshText").GetComponent<Text>();
            idText = transform.Find("ChoiceID").GetComponent<Text>();
            idText.text = idTxt;
            englishText.text = enTxt;
            welshText.text = cyTxt;

        }
    }
}