using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inspector : MonoBehaviour {

	protected InspectorUI inspectorUI;
	void Start () {
        inspectorUI = FindObjectOfType<InspectorUI>();
        print(inspectorUI);
	}

    void OnMouseEnter() {
        SetInspectorText();
        inspectorUI.SetInspecting(true);
    }

    public abstract void SetInspectorText();

    void OnMouseExit() {
        inspectorUI.SetInspecting(false);
    }
}
