using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use with UI button components to scroll as required when clicking or pressing mouse button down.
/// Use in conjunction with mouse pressed listener
/// </summary>
[RequireComponent(typeof(MousePressedListener))]
public class PressScrollModifier : MonoBehaviour, IScroll, IReactWhenPressed {
    /// <summary>
    /// Drag RectTransform to field in editor you want to scroll.
    /// </summary>
    [SerializeField]
    private RectTransform targetToScroll;
    /// <summary>
    /// In editor, enter positive for X to scroll right, negative to scroll left
    /// Enter positve in Y to scroll up, negative to scroll down. Enter 0 for either XY to remain static.
    /// </summary>
    [SerializeField]
    private Vector2 modifyXYofTarget;
    
    /// <summary>
    /// Replaces vector with new vector modified by vector defined in editor to increment / decrement position of X/Y
    /// </summary>
    public void ModifyScrollRectPosition() {
        try {
            targetToScroll.localPosition = new Vector2(targetToScroll.localPosition.x + modifyXYofTarget.x,
                                                       targetToScroll.localPosition.y + modifyXYofTarget.y);
        } catch (MissingReferenceException e) {
            Debug.LogError(e.Message);
            Debug.LogError(e.Source);
            Debug.LogError(e.TargetSite);
        }
    }

    /// <summary>
    /// Needed to work when mouse button held down in conjunction with MousePressedListener.
    /// </summary>
    public void ReactToPressed() {
        ModifyScrollRectPosition();
    }
}
