using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInspector : Inspector {
	
    public override void SetInspectorText() {
        inspectorUI.SetInspectorText(GetComponent<Character>().CharacterName);
    }
}
