using UnityEngine;
using System.Collections;
using GameQuestUtilities;

namespace Quests {
    public class QuestTheBeginning : MonoBehaviour {
        LowerUI lowerUI;
        public GameObject faerie;
        // Use this for initialization
        void Start() {
            //quests.ActivateDialogue("1");
            lowerUI = FindObjectOfType<LowerUI>();
            lowerUI.SetInUse();
            lowerUI.ProcessCharacterDialogue(faerie.GetComponent<Character>());
        }

        // Update is called once per frame
        void Update() {

        }
    }
}