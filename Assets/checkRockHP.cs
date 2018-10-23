using UnityEngine;

public class checkRockHP : MonoBehaviour {
    public FloatReference RHP;
	
	// Update is called once per frame
	void Update () {
        Debug.Log(RHP.Value);
	}
}
