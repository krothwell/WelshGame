using UnityEngine.UI;
using UnityEngine;
using System;

public class UITextPanelListItem : UIDisplayController {
    // Use this for initialization
    protected GameObject GetPanel() {
        return transform.Find("Panel").gameObject;
    }

    public override void SetColour(Color newColor) {
        GetPanel().GetComponent<Image>().color = newColor;
    }
}
