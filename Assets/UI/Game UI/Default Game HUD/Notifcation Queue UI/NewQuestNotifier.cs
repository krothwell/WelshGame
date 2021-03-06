﻿using UnityEngine;

namespace GameUI {
    
    public class NewQuestNotifier : GameNotifier {
        Transform triskelion;
        QuestsUI questsUI;
        // Use this for initialization
        void Awake() {
            questsUI = FindObjectOfType<QuestsUI>();
            triskelion = transform.Find("Triskelion");
        }

        void Update() {
            triskelion.Rotate(0, 0, -160 * Time.deltaTime); 
        }

        public override void ActivateNotification() {
            questsUI.ToggleToQuestNotified(myData.MyNotificationDetail);
            DestroySelf();
        }


    }
}