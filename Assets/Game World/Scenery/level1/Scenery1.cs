using UnityEngine;
using System.Collections;
using GameUI;
using GameUtilities.Display;
public class Scenery1 : MonoBehaviour {

    PlayerInventoryUI inventory;
	// Use this for initialization
	void Start () {
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = ImageLayerOrder.GetOrderInt(gameObject);
	}
}