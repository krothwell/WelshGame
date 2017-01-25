using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

    GameObject panel;
    // Use this for initialization
    void Start() {
        panel = gameObject.transform.FindChild("Panel").gameObject;
    }

    // Update is called once per frame
    void Update() {

    }

    public void DisplayPanel() {
        panel.SetActive(true);
    }

    public
        void HidePanel() {
        panel.SetActive(false);
    }
}
