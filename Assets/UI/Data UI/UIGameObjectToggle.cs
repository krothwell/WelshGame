using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameObjectToggle : MonoBehaviour {
    public GameObject GameObjectToToggle;
    // Use this for initialization

    public void ActivateGameObject() {
        GameObjectToToggle.SetActive(true);
    }

    public void DeactivateGameObject() {
        GameObjectToToggle.SetActive(false);
    }
}
