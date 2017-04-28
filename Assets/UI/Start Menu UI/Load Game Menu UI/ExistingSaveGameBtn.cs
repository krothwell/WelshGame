using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityUtilities;
namespace StartMenuUI {
    public class ExistingSaveGameBtn : ExistingSaveGame, ISelectableUI {
        public LoadGameMenuUI loadGameMenuUI;

        // Use this for initialization
        void Start() {
            loadGameMenuUI = GetComponentInParent<LoadGameMenuUI>();
        }

        // Update is called once per frame

        public override void SelectSelf() {
            GetPanel().GetComponent<Image>().color = new Color(0.9f, 0.99f, 0.95f);
            GetPlayerSavesController().SelectedSave = gameObject.GetComponent<ExistingSaveGameBtn>();
        }

        void OnMouseUpAsButton() {
            print(loadGameMenuUI);
            loadGameMenuUI.ToggleSelectionTo(this, loadGameMenuUI.saveGames);
        }
    }
}
