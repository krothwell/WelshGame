using UnityEngine;
using UnityEngine.UI;
using System;

namespace UIUtilities {
    /// <summary>
    /// Ad-hoc UI general commands relating to Unity UI objects that are not provided by default.
    /// </summary>
    public class UICommands {
        public static void SnapScrollToContentChild(GameObject target, ScrollRect scrollRect) {
            float scrollerHeight = scrollRect.GetComponent<RectTransform>().sizeDelta.y;
            Canvas.ForceUpdateCanvases();
            float targetPosY = Math.Abs(target.GetComponent<RectTransform>().localPosition.y);
            RectTransform contentHolder = scrollRect.content;
            float contentHeight = contentHolder.GetComponent<RectTransform>().sizeDelta.y;
            if (contentHeight > scrollerHeight) {
                float scrollableDistance = contentHeight - scrollerHeight;
                scrollRect.verticalNormalizedPosition = 1 - targetPosY / scrollableDistance;
            }
        }
    }
}