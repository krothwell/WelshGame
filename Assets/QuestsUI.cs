using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI {
    public class QuestsUI : UIController {
        public string selectedQuest;
        // Use this for initialization
        void Start() {
            selectedQuest = "selectedQuest";
            CreateSelectionToggleGroup(selectedQuest);
        }

        // Update is called once per frame
        void Update() {

        }
    }

}