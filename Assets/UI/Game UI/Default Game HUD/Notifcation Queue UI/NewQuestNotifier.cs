using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI {
    
    public class NewQuestNotifier : GameNotifier {
        Transform triskelion;
        QuestsUI questsUI;
        // Use this for initialization
        void Awake() {
            questsUI = FindObjectOfType<QuestsUI>();
            triskelion = transform.FindChild("Triskelion");
        }

        void Update() {
            triskelion.Rotate(0, 0, -160 * Time.deltaTime); 
        }

        public override void ActivateNotification() {
            questsUI.SelectQuest(myData.MyNotificationDetail);
            DestroySelf();
        }


    }
}