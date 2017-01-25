using UnityEngine;
using System.Collections;
public class WorldItem : MonoBehaviour {

    public WorldItems.itemTypes itemType;

    Inventory inventory;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = OrderInLayer.GetOrderInt(gameObject);
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GetPickedUp () {
        inventory.RecieveItem(gameObject);
        Destroy(gameObject);

    }

    void OnMouseUp() {
        print("working");
        GetPickedUp();
    }
}