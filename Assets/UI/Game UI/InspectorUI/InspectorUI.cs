using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InspectorUI : UIController {
    //private bool isInspecting = false;
    //// Use this for initialization
    //bool isOnDisplay = true;
	
	// Update is called once per frame
	//void Update () {
	//	if (IsInspecting()) {
 //           if (!isOnDisplay) {
 //               DisplayComponents();
 //               isOnDisplay = true;
 //           }
 //       } else {
 //           if (isOnDisplay) {
 //               HideComponents();
 //               isOnDisplay = false;
 //           }
 //       }
	//}

 //   private bool IsInspecting() {
 //       return isInspecting;
 //   }

 //   public void SetInspecting(bool inspecting) {
 //       isInspecting = inspecting;
 //   }

    public void SetInspectorText(string txt) {
        GetPanel().GetComponentInChildren<Text>().text = txt;
    }

    public void SetInspectorPosition(Transform tf, float offset) {
        transform.position = new Vector2(tf.position.x, tf.position.y + offset);
    }

}
