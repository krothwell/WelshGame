using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI {
    public class QuestNotifier : MonoBehaviour {
        private string questName;
        public string QuestName {
            get { return questName; }
            set { questName = value; }
        }
        QuestsUI questsUI;
        // Use this for initialization
        void Start() {
            questsUI = FindObjectOfType<QuestsUI>();
        }

        public void SelectQuestInQuestsUI() {
            questsUI.SelectQuest(questName);
        }

        public void DestroySelf() {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}