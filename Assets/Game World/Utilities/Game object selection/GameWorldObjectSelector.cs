using UnityEngine;
using System.Collections;

public class GameWorldObjectSelector : MonoBehaviour {
    /// <summary>
    /// Attaches to interactive game world objects – e.g. NPCs, collectable
    /// items) as a separate script and indicates when the player is hovering
    /// over an object / has selected an object by instantiating a selection 
    /// circle prefab in the object hierarchy.
    /// </summary>
    public GameObject selectionCirclePrefab;
    private GameObject selectionCircle;
    Color selectedColour; 
    private bool clicked;
    public float Scale, xOffset, yOffset;

    void Start () {
        selectedColour = new Color(0.27f, 0.53f, 0.94f);
    }

    public void DestroyMe() {
        if (selectionCircle != null) {
            Destroy(selectionCircle);
            selectionCircle = null;
            clicked = false;
        }
    }

    void OnMouseOver() {
        if (!clicked) {
            if (selectionCircle == null) {
                selectionCircle = Instantiate(selectionCirclePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                selectionCircle.GetComponent<Transform>().localScale = new Vector2(Scale,Scale);
                selectionCircle.GetComponent<Transform>().localPosition = new Vector2(xOffset, yOffset);
                selectionCircle.transform.SetParent(transform, false);
            }
        }

    }

    void OnMouseExit() {
        if (!clicked) {
            DestroyMe();
        }
    }

    void OnMouseUp() {
        clicked = true;
        ChangeColourToSelected();
    }


    void ChangeColourToSelected () {
        if (selectionCircle != null) {
            selectionCircle.GetComponent<SpriteRenderer>().color = selectedColour;
        }
    }

}
