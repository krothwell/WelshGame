using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationData {
    public enum NotificationType {
        newQuest,
        taskCompleted,
        newTask,
        questCompleted
    }
    private string myNotificationLbl;
    public string MyNotificationLbl {
        get { return myNotificationLbl; }
        set { myNotificationLbl = value; }
    }
    private string myNotificationDetail;
    public string MyNotificationDetail {
        get { return myNotificationDetail; }
        set { myNotificationDetail = value; }
    }

    private NotificationType myNotificationType;
    public NotificationType MyNotificationType {
        get { return myNotificationType; }
        set { myNotificationType = value; }
    }

    public NotificationData(string lbl, string detail, NotificationType notificaitonType) {
        myNotificationLbl = lbl;
        myNotificationDetail = detail;
        myNotificationType = notificaitonType;
    }
}
