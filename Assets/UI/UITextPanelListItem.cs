using UnityEngine.UI;
using UnityEngine;

public abstract class UITextPanelListItem : MonoBehaviour {
    // Use this for initialization
    protected GameObject GetPanel() {
        return transform.FindChild("Panel").gameObject;
    }

    public void SetPanelColour(Color newColor) {
        GetPanel().GetComponent<Image>().color = newColor;
    }
}
