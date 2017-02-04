using UnityEngine;
using System.Collections;

public class CharacterCollisionPerimeter : MonoBehaviour {
    CollisionAvoider collisionAvoider;
	// Use this for initialization
	void Start () {
        collisionAvoider = transform.parent.GetComponentInChildren<CollisionAvoider>();
	}

    void OnTriggerEnter2D(Collider2D trigger) {
        collisionAvoider.RedirectWhenObstacleDetected(trigger.gameObject);
    }
}
