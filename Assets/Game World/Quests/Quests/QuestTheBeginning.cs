using UnityEngine;
using System.Collections;
using GameQuestUtilities;
using GameUI;

namespace Quests {
    public class QuestTheBeginning : MonoBehaviour {
        PlayerCharacter player;
        DialogueUI dialogueUI;
        public GameObject faerie;
        // Use this for initialization
        void Start() {
            //quests.ActivateDialogue("1");
            
            player = FindObjectOfType<PlayerCharacter>();
            dialogueUI = FindObjectOfType<DialogueUI>();
            //dialogueUI.SetInUse();
            
            dialogueUI.StartNewDialogue(faerie.GetComponent<NPCHumanCharacter>());
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update() {

        }
    }
}