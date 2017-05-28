using UnityEngine;
using System.Collections;

public class EnemyToPlayerCloseCombatPlacements : MonoBehaviour {
    public GameObject[] leftPlacements;
    public GameObject[] rightPlacements;
    public GameObject left;
    public GameObject right;
    
    // Use this for initialization
    void Start () {
        left = transform.Find("Left").gameObject;
        right = transform.Find("Right").gameObject;
        int leftCount = transform.Find("Left").childCount;
        int rightCount = transform.Find("Right").childCount;
        leftPlacements = new GameObject[leftCount];
        rightPlacements = new GameObject[rightCount];
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetFreePositionLeft() {
        return GetFreePosition(leftPlacements);
    }

    public int GetFreePositionRight() {
        return GetFreePosition(rightPlacements);
    }

    int GetFreePosition(GameObject [] placements) {
        for (int i = 0; i < placements.Length; i++) {
            if (placements[i] == null) {
                return i;
            }
        }
        return -1;
    }

    public void SetEnemyPlacement(GameObject side, int index, GameObject enemy) {
        GameObject[] placements = (side == left) ? leftPlacements : rightPlacements;
        placements[index] = enemy;
    }

}
