﻿using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private float camMovementDelay, camMovementDelayDefault;
    PlayerCharacter playerCharacter;
    CharMovementController playerMovementController;
    bool followingPlayer;
    Vector2 camPosition;
    float speed;
    // Use this for initialization
    void Awake () {
        followingPlayer = false;
        camMovementDelayDefault = 0.3f;
        camMovementDelay = camMovementDelayDefault;
        playerCharacter = FindObjectOfType<PlayerCharacter>();
        playerMovementController = playerCharacter.MovementController;
        SetCamPosition();
    }
	
	// Update is called once per frame
	void Update () {
        MonitorPlayerMovement();
    }

    private void SetCamPosition() {
        camPosition = new Vector2(
                    (float)Math.Round(Camera.main.GetComponent<Transform>().position.x, 1),
                    (float)Math.Round(Camera.main.GetComponent<Transform>().position.y, 1));
    }

    private void MoveCameraToCoordinates(Vector2 newPosition) {
        SetMySpeed();
        float distanceX = Math.Abs(newPosition.x - camPosition.x);
        float distanceY = Math.Abs(newPosition.y - camPosition.y);
        float percentageOfTravelX = (100f / distanceX) * (1 * speed * Time.deltaTime);
        float percentageOfTravelY = (100f / distanceY) * (1 * speed * Time.deltaTime);
        int xModifier = camPosition.x >= newPosition.x ? -1 : 1;
        int yModifier = camPosition.y >= newPosition.y ? -1 : 1;
        float newX = distanceX > distanceY ? (speed * Time.deltaTime) : distanceX / 100 * percentageOfTravelY;
        float newY = distanceY > distanceX ? (speed * Time.deltaTime) : distanceY / 100 * percentageOfTravelX;
        //print(distanceX + " " + distanceY + " " + playerSpeed + " " + percentageOfTravelX + " " + percentageOfTravelY);
        //print(newX + " " + newY + " " + xModifier + " " + yModifier);
        Camera.main.transform.Translate(new Vector2(xModifier * newX, yModifier * newY));
        SetCamPosition();
    }

    private void MonitorPlayerMovement() {
        if ((CharacterMovementDecision) playerCharacter.MyDecision) {
            playerMovementController = playerCharacter.MovementController;
            if (!followingPlayer) {
                StartFollowingPlayerCountDown();
                if (camMovementDelay < 0f) {
                    
                    followingPlayer = true;
                    camMovementDelay = camMovementDelayDefault;
                }
            }
        } else if (camPosition != playerCharacter.GetMyPosition()) {
            CentreOnPlayer();
        }
        FollowPlayer();
    }

    private void FollowPlayer() {
        if (followingPlayer) {
            if (camPosition != playerMovementController.GetTargetPosition()) {
                MoveCameraToCoordinates(playerMovementController.GetTargetPosition());
            }
            else {
                followingPlayer = false;
            }
        }
    }

    private void CentreOnPlayer() {
        followingPlayer = false;
        MoveCameraToCoordinates(playerCharacter.GetMyPosition());
    }

    private void StartFollowingPlayerCountDown() {
        camMovementDelay -= Time.deltaTime;
    }

    public void SetMySpeed() {
        if(playerMovementController!= null) {
            CharacterMovement movement = playerMovementController.GetMyMovement();
            if (movement != null) {
                speed = movement.GetMovementSpeed();
            }
        }

    }

}
