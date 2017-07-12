using UnityEngine;
using UnityEngine.UI;

namespace DataUI.ListItems {
    public class PlayerChoiceTextOnly : MonoBehaviour {
        PlayerChoiceDetailsUI playerChoiceDetailsUI;
        InputField input;


        // Use this for initialization
        void Start() {
            playerChoiceDetailsUI = FindObjectOfType<PlayerChoiceDetailsUI>();
        }

        public void EditChoice() {
            playerChoiceDetailsUI.SetActivePlayerChoiceEdit();
        }

        public void UpdateChoiceDisplay(string newText) {
            input.GetComponent<InputField>().text = newText;
        }

        public void InitialiseDisplay(string choiceTxt, string idTxt) {
            input = transform.GetComponentInChildren<InputField>();
            input.text = choiceTxt;
            transform.Find("ChoiceID").GetComponent<Text>().text = idTxt;
        }
    }
}