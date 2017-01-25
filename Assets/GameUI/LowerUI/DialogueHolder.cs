using UnityEngine;
using System.Collections;

public class DialogueHolder : MonoBehaviour {
    GameObject dialogueScroller;
    float minimumHeightOfDialogue;
    private float componentsHeightCombined = 0f;
    //if a new dialgue node is shown, the node and it's choices will be displayed. The scroll window will 
    //be scrolled to the bottom automatically, if there is any room left causing the previous node/choices to display above
    //the active node/choices, then the height of the dialogue holder must be artificially increased to hide the previous node.
    //The size must also be decreased following the addition of another node, to ensure when scrolling back the user will easily
    //be able to read the dialogue transcript.
    // Use this for initialization
    void Start () {
        dialogueScroller = transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //public void SetInitialSizeAndPosition(RectTransform scrollerRectTransform) {
    //    GetComponent<RectTransform>().sizeDelta = scrollerRectTransform.sizeDelta;
    //    GetComponent<RectTransform>().position = scrollerRectTransform.position;
    //}

    //public void IncreaseHeightByComponent(GameObject component) {
    //    float myHeight = GetComponent<RectTransform>().sizeDelta.y;
    //    componentsHeightCombined += component.GetComponent<RectTransform>().sizeDelta.y;
    //}

    //public Vector3 GetComponentPosition(GameObject component) {
    //    RectTransform componentTransform = component.GetComponent<RectTransform>();
    //    float componentHeight = componentTransform.sizeDelta.y;
    //    float componentXpos = componentTransform.position.x;
    //    float componentYpos = componentTransform.position.y;
    //    float newComponentYpos = 0 - ((componentHeight / 2) + componentsHeightCombined);
    //    print(newComponentYpos);
    //    Vector3 newComponentPosition = new Vector3(componentXpos, newComponentYpos, 0f);
    //    return newComponentPosition;
    //}
}
