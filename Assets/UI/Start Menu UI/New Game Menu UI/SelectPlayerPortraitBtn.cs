using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using UnityUtilities;

namespace StartMenuUI {
    public class SelectPlayerPortraitBtn : MonoBehaviour, ISelectableUI {

        NewGameMenuUI newGame;

        // Use this for initialization
        void Awake() {
            newGame = FindObjectOfType<NewGameMenuUI>();
        }

        public void SelectSelf() {
            print(GetComponent<Image>());
            print(newGame);
            print(newGame.portraitSelectedColor);
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
            newGame.ToggleSelectionTo(this, newGame.selectedPortrait);
        }
    }
}