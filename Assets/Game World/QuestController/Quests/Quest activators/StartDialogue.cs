using UnityEngine;
using System.Collections;
using GameQuestUtilities;
using GameUI;

namespace Quests {
    public class StartDialogue : MonoBehaviour {
        public string dialogueID;
        DialogueUI dialogueUI;
        // Use this for initialization
        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
            dialogueUI.StartNewDialogue(dialogueID);
            Destroy(this);
            Destroy(gameObject);
        }
    }
}