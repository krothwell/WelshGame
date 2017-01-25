using UnityEngine;
using System.Collections;

public class SelectionOption : MonoBehaviour {

    public GameObject selectionCirclePrefab;
    public GameObject selectionCircle;
    Color selectedColour; 
    private bool clicked;

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
        selectionCircle.GetComponent<SpriteRenderer>().color = selectedColour;
    }

}
