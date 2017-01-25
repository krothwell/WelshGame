using UnityEngine;
using System.Collections;

public class CharacterPerimeter : MonoBehaviour {
    CollisionDetector collisionDetector;
	// Use this for initialization
	void Start () {
        collisionDetector = transform.parent.GetComponentInChildren<CollisionDetector>();
        print(collisionDetector);
	}

    void OnTriggerEnter2D(Collider2D trigger) {
        collisionDetector.RedirectWhenObstacleDetected(trigger.gameObject);
    }
}
