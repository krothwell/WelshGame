using UnityEngine;
using UnityEngine.UI;
namespace GameUI {
    namespace ListItems {
        public class PlayerInputField : MonoBehaviour {
            private string correctAnswer;
            public string CorrectAnswer {
                get { return correctAnswer; }
                set { correctAnswer = value; }
            }

            private bool submitted;
            public bool Submitted {
                get { return submitted; }
                set { submitted = value; }
            }

            DialogueUI dialogueManager;
            // Use this for initialization

            public void InitialiseMe(string answerTxt) {
                correctAnswer = answerTxt;
            }

            public void SetSubmitted() {
                GetComponent<InputField>().enabled = false;
                submitted = true;
            }

        }
    }
}