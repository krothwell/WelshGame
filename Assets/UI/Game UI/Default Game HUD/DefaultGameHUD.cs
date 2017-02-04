using UnityEngine;
using System.Collections;

namespace GameUI {
    public class DefaultGameHUD : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void HideMe() {
            gameObject.SetActive(false);
        }

        public void ShowMe() {
            gameObject.SetActive(true);
        }
    }
}