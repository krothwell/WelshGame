using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityUtilities;
namespace GameUI {
    public class ExistingSaveGameBtn : ExistingSaveGame, ISelectableUI {

        EscMenuUI escMenuUI;

        // Use this for initialization
        void Start() {
            escMenuUI = FindObjectOfType<EscMenuUI>();
        }

        public override void SelectSelf() {
            GetPanel().GetComponent<Image>().color = new Color(0.9f, 0.99f, 0.95f);
            playerSavesController.SelectedSave = gameObject.GetComponent<ExistingSaveGameBtn>();
            if (playerSavesController.newSaveInput != null) {
                playerSavesController.ShowSelectedSaveInInput();
            }
        }

        void OnMouseUpAsButton() {
            //loadGameMenuUI.ToggleSelectionTo(this, loadGameMenuUI.saveGames);
        }
    }
}
