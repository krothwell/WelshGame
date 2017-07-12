using UnityEngine;
using UnityEngine.UI;

namespace DataUI.ListItems {
    public class DialogueNodeTextOnly : MonoBehaviour {
        DialogueNodeDetailsUI dialogueNodeDetailsUI;
        InputField input;


        // Use this for initialization
        void Start() {
            dialogueNodeDetailsUI = FindObjectOfType<DialogueNodeDetailsUI>();
        }

        public void Edit() {
            dialogueNodeDetailsUI.SetActiveNodeEdit();
        }

        public void UpdateNodeDisplay(string newText) {
            input.GetComponent<InputField>().text = newText;
        }

        public void InitialiseDisplay(string nodeTxt, string idTxt) {
            input = transform.GetComponentInChildren<InputField>();
            input.text = nodeTxt;
            transform.Find("IDText").GetComponent<Text>().text = idTxt;
        }
    }
}