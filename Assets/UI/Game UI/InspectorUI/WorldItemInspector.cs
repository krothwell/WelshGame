using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemInspector : Inspector {

    public override void SetInspectorText() {
        inspectorUI.SetInspectorText(GetComponent<WorldItem>().name);
    }
}
