using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;

namespace DataUI {
    namespace ListItems {
        public class ExistingEndCombatWithCharTaskResult : ExistingTaskResult {
            protected Text characterNameText, sceneNameText;

            public void InitialiseMe(string resID, string charName, string sceneName) {
                base.InitialiseMe(resID);
                characterNameText = transform.FindChild("CharName").GetComponent<Text>();
                sceneNameText = transform.FindChild("SceneName").GetComponent<Text>();
                characterNameText.text = charName;
                sceneNameText.text = sceneName;
            }
        }
    }
}