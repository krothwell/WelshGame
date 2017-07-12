using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoiceDetailsTextOnlyUI : UIController, ISelectableUI {

    public void SelectSelf() {
        DisplayComponents();
    }

    public void DeselectSelf() {
        HideComponents();
    }
}
