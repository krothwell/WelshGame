using UnityEngine;

/// <summary>
/// The dialogue holder is responsible for manage building, displaying and positioning of dialogue parts.
/// </summary>
[RequireComponent(typeof(pressableYScroller))]
public class DialogueHolder : MonoBehaviour {
    
    float minimumHeightOfDialogue;
    public static DialogueHolder instance;

    private Vector2 startingXYofdialogueHolder;

    private void Awake() {
        instance = this;
    }
    void Start () {
        startingXYofdialogueHolder = new Vector2(transform.localPosition.x, transform.localPosition.y);
    }

    public void Reset() {
        EmptyContents();
        ResetPosition();
    }

    private void EmptyContents() {
        foreach (Transform dialoguePart in transform) {
            Destroy(dialoguePart.gameObject);
        }
    }

    /// <summary>
    /// Moves the dialogue container back to it's local starting position.
    /// </summary>
    private void ResetPosition() {
        transform.localPosition = new Vector2(startingXYofdialogueHolder.x, startingXYofdialogueHolder.y);
    }


    public bool IsValidScrollModifier(float modifierValue) {
        if (modifierValue <= 0) {
            Debug.LogError("You must use a positive value");
            return false;
        } else {
            return true;
        }
    }
}
