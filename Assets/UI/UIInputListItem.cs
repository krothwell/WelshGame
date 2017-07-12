using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputListItem : UIDisplayController {
    private InputField inputField;

    public InputField GetInputField() {
        inputField = GetComponentInChildren<InputField>();
        return inputField;
    }

    public override void SetColour(Color newColor) {
        ColorBlock cb = GetInputField().colors;
        cb.normalColor = newColor;
        inputField.colors = cb;
    }

    public void SetInputText(string newText) {
        inputField.text = newText;
    }


}
