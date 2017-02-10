using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using UnityUtilities;

namespace StartMenuUI {
    public class SelectPlayerPortraitBtn : MonoBehaviour, ISelectableUI {

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
                if (MouseSelection.IsClickedDifferentGameObjectTo(this.gameObject)) {
                    //if a rule is not being edited then the rule list is refreshed.
                    GameObject clickSelect;
                    MouseSelection.ClickSelect(out clickSelect);
                    SelectPlayerPortraitBtn charPortraitSelector = clickSelect.GetComponent<SelectPlayerPortraitBtn>();
                    if (charPortraitSelector != null) {
                        DeselectSelf();
                    }
                }
            }
        }

        public void SelectSelf() {
            GetComponent<Image>().color = newGame.portraitSelectedColor;
            newGame.SetSelectedPortrait(GetComponent<SelectPlayerPortraitBtn>());
        }

        public void DeselectSelf() {
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
                DeselectSelf();
            }
        }

        void OnMouseUp() {
            SelectSelf();
        }
    }
}