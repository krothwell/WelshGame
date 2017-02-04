using UnityEngine;
using System.Collections;
using GameUI;
using GameUtilities.Display;
public class WorldItem : MonoBehaviour {

    public WorldItems.itemTypes itemType;

    PlayerInventoryUI playerInventory;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = ImageLayerOrder.GetOrderInt(gameObject);
        playerInventory = FindObjectOfType<PlayerInventoryUI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GetPickedUp () {
        playerInventory.RecieveItem(gameObject);
        Destroy(gameObject);

    }

    void OnMouseUp() {
        print("working");
        GetPickedUp();
    }
}