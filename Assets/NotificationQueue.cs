using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI {
    public class NotificationQueue : MonoBehaviour {
        public GameObject newQuestNotifierPrefab;
        List<NotificationData> notificationDataList;
        private Transform notificationList;
        GameObject notificationLbl;
        // Use this for initialization
        void Start() {
            notificationDataList = new List<NotificationData>();
            notificationList = GetComponentInChildren<HorizontalLayoutGroup>().transform;
            notificationLbl = transform.FindChild("NotificationsLbl").gameObject;
            ClearNotifications();
        }

        private void ClearNotifications() {
            foreach (Transform notifier in notificationList) {
                Destroy(notifier.gameObject);
            }
        }

        public void QueueNewQuestNotifier(string questName) {
            NotificationData newQuestNotifier = new NotificationData("New quest", questName, NotificationData.NotificationType.newQuest);
            if (!notificationDataList.Contains(newQuestNotifier)) { notificationDataList.Add(newQuestNotifier); }
            
        }

        public void QueueUpdatedQuestNotifier(string questName) {
            NotificationData newQuestNotifier = new NotificationData("Updated quest", questName, NotificationData.NotificationType.newQuest);
            if (!notificationDataList.Contains(newQuestNotifier)) { notificationDataList.Add(newQuestNotifier); }
        }

        public void DisplayQueuedNotifications() {
            foreach (NotificationData notification in notificationDataList) {
                switch (notification.MyNotificationType) {
                    case NotificationData.NotificationType.newQuest:
                        GameObject newQuestNotification = Instantiate(newQuestNotifierPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                        newQuestNotification.transform.SetParent(notificationList, false);
                        newQuestNotification.GetComponent<NewQuestNotifier>().InitialiseSelf(notification);
                        break;
                }
                //notificationDataList.Remove(notification);
            }
            ToggleLbl();
            notificationDataList = new List<NotificationData>();
        }

        public void ToggleLbl() {
            if (notificationList.childCount > 0) {
                ShowMyLbl();
            }
            else {
                HideMyLbl(); }
        }

        public void HideMyLbl() {
            notificationLbl.SetActive(false);
        }

        public void ShowMyLbl() {
            notificationLbl.SetActive(true);
        }
    }
}
