using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace StartMenuUI {
    public class NewGameMenuUI : UIController {
        public Color portraitSelectedColor, portraitHoverColor, portraitDeselectedColor;
        public string selectedPortrait;
        private SelectPlayerPortraitBtn currentPortraitSelected;
        private PlayerSavesController playerSavesController;
        GameObject portraitGrid;
        // Use this for initialization
        void Start() {
            portraitSelectedColor = new Color(0f, 0.35f, 66.6f, 0.6f);
            portraitHoverColor = new Color(0f, 0.15f, 46.6f, 0.6f);
            portraitDeselectedColor = new Color(0f, 0f, 0f, 0.50196f);
            portraitGrid = GetPanel().transform.FindChild("PortraitGrid").gameObject;
            selectedPortrait = "selectedPortrait";
            CreateSelectionToggleGroup(selectedPortrait);
            playerSavesController = FindObjectOfType<PlayerSavesController>();
        }

        public void SetFirstPortraitSelected() {
            if (currentPortraitSelected == null) {
                GameObject firstPortrait = portraitGrid.transform.GetChild(0).gameObject;
                ToggleSelectionTo(firstPortrait.GetComponent<SelectPlayerPortraitBtn>(), selectedPortrait);
                SetSelectedPortrait(firstPortrait.GetComponent<SelectPlayerPortraitBtn>());
            }
        }

        public void SetSelectedPortrait(SelectPlayerPortraitBtn portrait) {
            currentPortraitSelected = portrait;
        }

        public string GetSelectedPortraitPath() {
            print(currentPortraitSelected);
            return currentPortraitSelected.GetMyImagePath();
        }

        public SelectPlayerPortraitBtn GetSelectedPortrait() {
            return currentPortraitSelected;
        }

        public string GetNameInput() {
            return GetComponentInChildren<InputField>().text == "" ?
                GetComponentInChildren<InputField>().gameObject.transform.FindChild("Placeholder").GetComponent<Text>().text :
                GetComponentInChildren<InputField>().text;
        }

        public void StartNewGame() {
            playerSavesController.StartNewGame(GetNameInput(), GetSelectedPortraitPath());
        }
    }
}