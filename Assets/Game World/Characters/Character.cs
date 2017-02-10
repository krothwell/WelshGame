﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using GameUtilities.Display;
/// <summary>
/// Abstract class which provides default controls of all derived character
/// types such as the player and NPCs. 
/// </summary>
public class Character : MonoBehaviour {
    public Sprite charPortrait;
    public string nameID;
    private Vector2 targetPosition;
    public Vector2 TargetPosition {
        get { return targetPosition; }
        set { targetPosition = value; }
    }
    private Vector2 interimPosition;
    public Vector2 InterimPosition {
        get { return interimPosition; }
        set { interimPosition = value; }
    }

    private Vector2 myPosition;
    public bool redirecting = false;
    public Vector2 newPosition;
    private bool isFighting;
    public bool IsFighting {
        get { return isFighting; }
        set { isFighting = value; }
    }
    public float distanceX, distanceY, rawDistanceX, rawDistanceY;
    public int rerouteCount, rerouteLimit;
    public int RerouteCount {
        get { return rerouteCount;  }
        set { rerouteCount = value; }
    }

    public int RerouteLimit {
        get { return rerouteLimit; }
        set { rerouteLimit = value; }
    }
    GameObject charSprite;
    public GameObject[] bodyParts;
    public CollisionAvoider collisionAvoider;
    // Use this for initialization
    public GameObject GetCharacterSprite() {
        return gameObject.transform.FindChild("CentreOfGravity").gameObject; ;
    }
	
    public void SetCharDefaults() {
        SetMyOrder(bodyParts);
        collisionAvoider = transform.GetComponentInChildren<CollisionAvoider>();
        RerouteLimit = 50;
    }

    public void SetTargetPosition(Vector2 targetPos, Vector2 myPos) {
        myPosition = myPos;
        if (targetPosition != targetPos) {
            targetPosition = targetPos;
        }
        if (redirecting) {
            if (myPosition == interimPosition) {
                SetInterimPosition(Vector2.zero, false);
                newPosition = targetPosition;
                SetMyDirection(targetPosition, myPosition);
            }
            else {
                newPosition = interimPosition;
            }
        }
        else { newPosition = targetPosition; }
    }

    public void MoveToCoordinates(float speed) {
        SetDistanceFromNewPosition(newPosition, myPosition);
        collisionAvoider.SetCollisionDetector();
        //to walk as the crow flies diagonally, a percentage is captured for each axis
        float percentageOfTravelX = (100f / distanceX) * (1 * speed * Time.deltaTime);
        float percentageOfTravelY = (100f / distanceY) * (1 * speed * Time.deltaTime);
        //the greatest distance of an axis will go at the normal speed, while the shorter distance will move at the percentage of the distance travelled by the other axis
        float newX = distanceX > distanceY ? (speed * Time.deltaTime) : distanceX / 100 * percentageOfTravelY;
        float newY = distanceY > distanceX ? (speed * Time.deltaTime) : distanceY / 100 * percentageOfTravelX;
        int xModifier = myPosition.x >= newPosition.x ? -1 : 1;
        int yModifier = myPosition.y >= newPosition.y ? -1 : 1;
        transform.Translate(new Vector3(xModifier * newX, yModifier * newY, 0));
    }

    public void SetInterimPosition(Vector2 position, bool redirect) {
        interimPosition = position;
        redirecting = redirect;
        rerouteCount++;
    }

    public Vector2 GetMyPosition() {
        return new Vector2(
                    (float)Math.Round(gameObject.GetComponent<Transform>().position.x, 1),
                    (float)Math.Round(gameObject.GetComponent<Transform>().position.y, 1));
        
    }

    public void SetMyOrder(GameObject[] characterParts) {
        ImageLayerOrder.SetOrderOnGameObjectArray(characterParts, ImageLayerOrder.GetOrderInt(gameObject) - 1);
        ImageLayerOrder.SetZ(gameObject);
    }

    public void SetMyDirection(Vector2 targettedPosition, Vector2 myPosition) {
        //depending on which direction the player is going, changes the modifier to decrease or increase on that axis
        int xModifier = myPosition.x >= targettedPosition.x ? -1 : 1;
        Vector3 spriteDirection = new Vector3(xModifier,
                                      GetCharacterSprite().GetComponent<Transform>().localScale.y,
                                      GetCharacterSprite().GetComponent<Transform>().localScale.z);
        if (GetCharacterSprite().GetComponent<Transform>().localScale.x != spriteDirection.x) {
            GetCharacterSprite().GetComponent<Transform>().localScale = spriteDirection;
        }
    }

    public void SetDistanceFromNewPosition(Vector2 newPosition, Vector2 myPosition) {
        distanceX = Math.Abs(newPosition.x - myPosition.x);
        distanceY = Math.Abs(newPosition.y - myPosition.y);
    }

    public void SetFightMode(bool active) {
        isFighting = active;

    }

    public void SetHovered() {
        foreach(GameObject bodyPart in bodyParts) {
            bodyPart.GetComponent<SpriteRenderer>().color = new  Color(0.9f, 0.8f, 0.7f);
        }

    }
    public void SetUnhovered() {
        foreach (GameObject bodyPart in bodyParts) {
            bodyPart.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }

    }

    public float GetDistanceFromMyPosition(Vector2 newPosition) {
        return (float)Math.Sqrt((Math.Pow((double)(newPosition.x - myPosition.x), 2)
                              + (Math.Pow((double)(newPosition.y - myPosition.y), 2))));
    }

    public string GetMyPortraitPath() {
        return AssetDatabase.GetAssetPath(charPortrait);
    }

    public void SetMyPortrait(string path) {
        charPortrait = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
    }

    public Sprite GetMyPortrait() {
        return charPortrait;
    }
}
