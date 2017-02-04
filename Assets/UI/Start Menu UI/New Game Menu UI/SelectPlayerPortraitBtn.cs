using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using UnityUtilities;

namespace StartMenuUI {
    public class SelectPlayerPortraitBtn : MonoBehaviour {

        NewGameMenuUI newGame;

        // Use this for initialization
        void Start() {
            newGame = FindObjectOfType<NewGameMenuUI>();
        }

        // Update is called once per frame
        void Update() {
            DeselectIfTypeIsSame();
        }

        private void DeselectIfTypeIsSame() {
            if (Input.GetMouseButtonUp(0)) {
                MouseSelection.ClickSelect();
                if (MouseSelection.ClickedDifferentGameObjectTo(this.gameObject)) {
                    //if a rule is not being edited then the rule list is refreshed.
                    SelectPlayerPortraitBtn charPortraitSelector = MouseSelection.selected.GetComponent<SelectPlayerPortraitBtn>();
                    if (charPortraitSelector != null) {
                        DeselectMe();
                    }
                }
            }
        }

        public void SelectMe() {
            GetComponent<Image>().color = newGame.portraitSelectedColor;
            newGame.SetSelectedPortrait(GetComponent<SelectPlayerPortraitBtn>());
        }

        private void DeselectMe() {
            GetComponent<Image>().color = newGame.portraitDeselectedColor;
        }

        public string GetMyImagePath() {
            return AssetDatabase.GetAssetPath(transform.GetChild(0).GetComponentInChildren<Image>().sprite);
        }

        void OnMouseEnter() {
            if (newGame.GetSelectedPortrait().gameObject != gameObject) {
                GetComponent<Image>().color = newGame.portraitHoverColor;
            }
        }

        void OnMouseExit() {
            if (newGame.GetSelectedPortrait().gameObject != gameObject) {
                DeselectMe();
            }
        }

        void OnMouseUp() {
            SelectMe();
        }
    }
}