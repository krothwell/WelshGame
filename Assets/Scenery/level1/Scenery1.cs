using UnityEngine;
using System.Collections;
public class Scenery1 : MonoBehaviour {

    Inventory inventory;
	// Use this for initialization
	void Start () {
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = OrderInLayer.GetOrderInt(gameObject);
	}
}