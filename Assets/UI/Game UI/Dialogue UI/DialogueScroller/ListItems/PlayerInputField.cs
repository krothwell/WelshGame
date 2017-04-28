using UnityEngine;
using UnityEngine.UI;
namespace GameUI {
    namespace ListItems {
        public class PlayerInputField : MonoBehaviour {

            private bool submitted;
            public bool Submitted {
                get { return submitted; }
                set { submitted = value; }
            }

            public void SetSubmitted() {
                GetComponent<InputField>().enabled = false;
                submitted = true;
            }

            public string GetPlayerInputString() {
                return GetComponent<InputField>().text;
            }

        }
    }
}