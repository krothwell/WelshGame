using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class DefaultAnimation : Editor {
    [CustomEditor(typeof(NPCCharacter))]
    // Use this for initialization
    string[] animations = new[] { "idle1", "idle2", "sitting" };
    int _choiceIndex = 0;
    public override void OnInspectorGUI() {
        // Draw the default inspector
        DrawDefaultInspector();
        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, animations);
        var charClass = target as NPCCharacter;
        // Update the selected choice in the underlying object
        charClass.DefaultAnimationOverride = GetAnimationOverride();
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }

    public string GetAnimationOverride() {
        string animationParam = "";
        switch (_choiceIndex) {
            case 1:
                animationParam = "idle1";
                break;
            case 2:
                animationParam = "isSitting";
                break;
        }
        return animationParam;
    }
}
