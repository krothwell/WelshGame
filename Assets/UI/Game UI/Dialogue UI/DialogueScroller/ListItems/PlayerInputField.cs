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

            DialogueUI dialogueManager;
            // Use this for initialization

            public void InitialiseMe(string answerTxt) {
                correctAnswer = answerTxt;
            }
        }
    }
}