using UnityEngine;
using System.Collections;

public class OrderInLayer : MonoBehaviour {

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void SetLayerOrder(GameObject go) {
        int layer = GetOrderInt(go);
        go.GetComponent<SpriteRenderer>().sortingOrder = layer;
    }

    public static void SetOrderOnArray(GameObject [] arrayObjects, int order) {
        foreach(GameObject go in arrayObjects) {
            go.GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }

    public static int GetOrderInt(GameObject go) {
        return (int)(-System.Math.Round(go.GetComponent<Transform>().position.y, 1) * 10);
    }

    public static void SetZ(GameObject go) {
        Vector3 goPos = go.GetComponent<Transform>().position;
        //float newZ = (int)System.Math.Round(goPos.y, 1) * 10;
        go.GetComponent<Transform>().position = new Vector3(goPos.x, goPos.y, goPos.y);
    }
}
