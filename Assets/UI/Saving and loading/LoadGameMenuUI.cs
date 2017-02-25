using UnityEngine;
using System.Collections;

namespace StartMenuUI {
    public class LoadGameMenuUI : UIController {
        public string saveGames;

        void Awake() {
            saveGames = "saveGames";
            CreateSelectionToggleGroup(saveGames);
        }
    }
}