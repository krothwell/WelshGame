using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNodeDetailsTextOnlyUI : UIController, ISelectableUI {

    public void SelectSelf() {
        DisplayComponents();
    }

    public void DeselectSelf() {
        HideComponents();
    }
}
