using UnityEngine;
using UnityEngine.UI;
using UnityUtilities;

namespace GameUI {
    public abstract class GameNotifier : MonoBehaviour {
        protected Text lblText;
        protected Text detailText;
        protected NotificationData myData;
        protected NotificationQueue notificationQueue;
    

        public void SetDetailText(string detail) {
            detailText = transform.FindChild("QuestNameLbl").GetComponent<Text>();
            detailText.text = detail;
        }

        public void SetNotificationMsg(string msg) {
            lblText = transform.FindChild("QuestNotifierLbl").GetComponent<Text>();
            lblText.text = msg;
        }

        public void InitialiseSelf(NotificationData data) {
            myData = data;
            SetDetailText(myData.MyNotificationDetail);
            SetNotificationMsg(myData.MyNotificationLbl);
            SetWidthToTextLength();
            notificationQueue = FindObjectOfType<NotificationQueue>();
        }

        public void SetWidthToTextLength() {
            LayoutElement layoutElement = GetComponent<LayoutElement>();
            BoxCollider2D myBoxCollider = GetComponent<BoxCollider2D>();
            Canvas.ForceUpdateCanvases();

            float marginLeft = lblText.transform.localPosition.x;
            //print("Triskelion: " + triskelionWidth);
            float closeBtnWidth = GetComponentInChildren<Button>().GetComponent<RectTransform>().sizeDelta.x;
            //print("CloseBtn: " + closeBtnWidth);
            float textWidth = lblText.preferredWidth > detailText.preferredWidth ? lblText.preferredWidth : detailText.preferredWidth;
            //print("TextWidth: " + textWidth);
            float widthTotal = marginLeft + closeBtnWidth + textWidth;

            layoutElement.minWidth = widthTotal;
            myBoxCollider.size = new Vector2(widthTotal, myBoxCollider.size.y);
        }

        public abstract void ActivateNotification();

        void OnMouseUpAsButton() {
            ActivateNotification();
            MouseSelection.DelayNextClickSelect();
        }

        public void DestroySelf() {
            notificationQueue = FindObjectOfType<NotificationQueue>();
            transform.SetParent(null);//destroy doesn't destroy obj until end of frame, so child count won't be updated unless this is detatched from the parent
            Destroy(gameObject);
            Destroy(this);
            MouseSelection.DelayNextClickSelect();
            notificationQueue.ToggleLbl();
        }

    }
}