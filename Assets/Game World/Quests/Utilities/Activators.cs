using UnityEngine;
using System.Collections;
using DbUtilities;
namespace GameQuestUtilities {
    public class Activators : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        public void ActivateDialogue(string id) {
            DbCommands.UpdateTableField("Dialogues", "Active", "1", "DialogueIDs = " + id);
        }
    }
}