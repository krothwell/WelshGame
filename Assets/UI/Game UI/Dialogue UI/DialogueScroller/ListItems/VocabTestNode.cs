using UnityEngine;
using UnityEngine.UI;
namespace GameUI {
    namespace ListItems {
        public class VocabTestNode : MonoBehaviour {
            private string textToTranslate;
            public string TextToTranslate {
                get { return textToTranslate; }
                set { textToTranslate = value; }
            }
            private string correctAnswer;
            public string CorrectAnswer {
                get { return correctAnswer; }
                set { correctAnswer = value; }
            }

            Text displayText;

            DialogueUI dialogueManager;
            // Use this for initialization

            public void InitialiseMe(string translateTxt, string answerTxt) {
                displayText = GetComponent<Text>();
                textToTranslate = translateTxt;
                correctAnswer = answerTxt;
                displayText.text = textToTranslate;
            }
        }
    }
}