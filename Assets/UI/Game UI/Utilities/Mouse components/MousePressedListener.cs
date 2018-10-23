using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Use on any UI component to trigger a reaction when pressed. Requires a corresponding component
/// which implements IReactWhenPressed to use the polymorphic method ReactToPressed.
/// </summary>
[RequireComponent(typeof(IReactWhenPressed))]
public class MousePressedListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler {
    private IReactWhenPressed pressedUIObject;
    private bool isPressed = false;

    void Start() {
        pressedUIObject = GetComponent<IReactWhenPressed>();
    }
    private void Update() {
        if (!isPressed) return;
        pressedUIObject.ReactToPressed();
    }

    public void OnDrag(PointerEventData eventData) {
        //
    }
    public void OnPointerDown(PointerEventData eventData) {
        isPressed = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        isPressed = false;
    }
    public void OnPointerUp(PointerEventData eventData) {
        isPressed = false;
    }
}

