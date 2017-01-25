using UnityEngine;
using System.Collections;

public class DataUI : MonoBehaviour {
    public Color colorDataUIbtn;
    public Color colorDataUItxt;
    public Color colorDataUIinactive;
    public Color colorDataUIInputSelected;
    public GameObject[] options;

    void Start() {
        colorDataUIbtn = new Color(0.27f, 0.53f, 0.94f);
        colorDataUItxt = new Color(0f, 0f, 0f);
        colorDataUIinactive = new Color(0.13f, 0.13f, 0.13f);
        colorDataUIInputSelected = new Color(0.8f, 0.85f, 1f);
    }
    public void SetOptionsDisplay(GameObject optionSelected) {
        foreach (GameObject option in options) {
            option.SetActive(false);
        }
        optionSelected.SetActive(true);
    }

    public void ActivateSelf() {
        gameObject.SetActive(true);
    }

    public void DeactivateSelf() {
        gameObject.SetActive(false);
        SelectController.DelayNextSelection();
    }
}
