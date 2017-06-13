using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inspector : MonoBehaviour {

	protected InspectorUI inspectorUI;
    public float yOffset = -0.5f;
	void Awake () {
        inspectorUI = FindObjectOfType<InspectorUI>();
	}

    public void Inspect() {
        SetInspectorText();
        inspectorUI.SetInspectorPosition(transform, yOffset);
        inspectorUI.SetInspectorHealth(transform);
        inspectorUI.DisplayComponents();
    }

    public abstract void SetInspectorText();

    public void EndInspection() {
        inspectorUI.HideComponents();
    }
}
