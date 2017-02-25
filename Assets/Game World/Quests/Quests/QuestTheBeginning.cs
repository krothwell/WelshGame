using UnityEngine;
using System.Collections;
using GameQuestUtilities;
using GameUI;

namespace Quests {
    public class QuestTheBeginning : MonoBehaviour {
        PlayerController player;
        DialogueUI dialogueUI;
        public GameObject faerie;
        // Use this for initialization
        void Start() {
            //quests.ActivateDialogue("1");
            player = FindObjectOfType<PlayerController>();
            player.SetStatusToInteractingWithObject();
            dialogueUI = FindObjectOfType<DialogueUI>();
            //dialogueUI.SetInUse();
            dialogueUI.StartNewDialogue(faerie.GetComponent<EnemyController>());
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update() {

        }
    }
}