using UnityEngine;
using System.Collections;
using System;

public class CollisionDetector : MonoBehaviour {
    float xDirection, yDirection, distanceX, distanceY;
    Character character;
    Vector2 charPos, closestCorner;
    GameObject myPerimeter;
    EdgeCollider2D collisionDetector;
    Transform perimeterTransform;
    MainCharacter mainChar;
    void Start() {
        character = transform.parent.GetComponent<Character>();
        myPerimeter = transform.parent.FindChild("Perimeter").gameObject;
        collisionDetector = GetComponent<EdgeCollider2D>();
        perimeterTransform = myPerimeter.GetComponent<Transform>();
        mainChar = FindObjectOfType<MainCharacter>();
      

    }

    void Update () {
        //Time.timeScale = 0.5f;
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();

        }
    }



    void OnTriggerEnter2D(Collider2D trigger) {
        RedirectWhenObstacleDetected(trigger.gameObject);
    }

    public void RedirectWhenObstacleDetected(GameObject obstacle) {
        //print(trigger.gameObject);
        if (obstacle.name == "Perimeter") {
            if (obstacle != myPerimeter) {
                if (mainChar.playerStatus == MainCharacter.PlayerStatus.movingToObject 
                || mainChar.playerStatus == MainCharacter.PlayerStatus.interactingWithObject) {
                    if (obstacle.transform.parent.gameObject != SelectController.selected) {
                        print(SelectController.selected);
                        print(obstacle.transform.parent.gameObject);
                        RedirectCharacter(obstacle);
                    }
                } else {
                    RedirectCharacter(obstacle);
                }
            }

        }
    }

    public void SetCollisionDetector() {
        /*the collision detector line's beginning is offset to the closest corner of the character's Perimeter, in the direction 
         * of which the character is travelling. This is to avoid situations where the line must detect a collision but doesn't
         * because the angle from the starting point to the destination point doesn't intersect the obstacles perimeter but the 
          character's perimeter does. */
        SetAxisDirection();
        float offsetX = (float)(perimeterTransform.localPosition.x + (xDirection * (perimeterTransform.localScale.x / 2)));
        float offsetY = (float)(perimeterTransform.localPosition.y + (yDirection * (perimeterTransform.localScale.y / 2)));
        collisionDetector.offset = new Vector2(offsetX, offsetY);

        //the 
        Vector2 bPosition = new Vector2 ((float)(perimeterTransform.position.x + (xDirection * (perimeterTransform.lossyScale.x / 2))),
                                         (float)(perimeterTransform.position.y + (yDirection * (perimeterTransform.lossyScale.y / 2))));
        SetDistanceFromMyPosition(bPosition);

        float multiplier;
        multiplier = 1 / transform.parent.GetComponent<Transform>().localScale.x;
        Vector2[] newPoints = new Vector2[2];
        newPoints[0] = new Vector2(0f, 0f);
        newPoints[1] = new Vector2((distanceX) * multiplier, (distanceY) * multiplier);
        collisionDetector.points = newPoints;
    }

    void SetDistanceFromMyPosition(Vector2 myPosition) {
        distanceX = character.newPosition.x - myPosition.x;
        distanceY = character.newPosition.y - myPosition.y;
    }

    public void RedirectCharacter(GameObject objectToAvoid) {
        SetAxisDirection();
        closestCorner = ChooseClosestCorner(objectToAvoid);
        character.SetInterimPosition(closestCorner, true);
        character.SetMyDirection(closestCorner, character.GetMyPosition());
    }

    private void SetAxisDirection () {
        xDirection = GetAxisDirection(character.TargetPosition.x, character.GetMyPosition().x);
        yDirection = GetAxisDirection(character.TargetPosition.y, character.GetMyPosition().y);
    }

    private float GetAxisDirection (float target, float current) {
        if (target > current) {
            return 1f;
        }
        else if (target < current) {
            return -1f;
        }
        else {
            return 0f;
        }
    }

    private Vector2 ChooseClosestCorner(GameObject obj) {
        Vector2 objPos = obj.GetComponent<Transform>().position;
        Vector2 objSize = obj.GetComponent<Transform>().lossyScale;

        //TODO: move corner relevant to characters perimeter
        Vector2 corner1 = new Vector2((float)(Math.Round((objPos.x + (-(xDirection) * (objSize.x / 2))
                                     + (-(xDirection * perimeterTransform.lossyScale.x) / 1.25)),1)),
                                      (float)(Math.Round(objPos.y + (yDirection * (objSize.y / 2))
                                     + ((yDirection * perimeterTransform.lossyScale.y)), 1)));

        Vector2 corner2 = new Vector2((float)(Math.Round(objPos.x + (xDirection * (objSize.x / 2))
                                     + ((xDirection * perimeterTransform.lossyScale.x) / 1.25), 1)),
                                      (float)(Math.Round(objPos.y + (-(yDirection) * (objSize.y / 2))
                                     + (-(yDirection * perimeterTransform.lossyScale.y)), 1)));

        float distanceFromC1 = GetDistanceFromCharPosition(corner1);
        float distanceFromC2 = GetDistanceFromCharPosition(corner1);
        return (distanceFromC1 < distanceFromC2) ? corner1:corner2;
    } 

    public float GetDistanceFromCharPosition(Vector2 newPosition) {
        return (float)Math.Sqrt((Math.Pow((double)(newPosition.x - charPos.x), 2)
                              + (Math.Pow((double)(newPosition.y - charPos.y), 2))));
    }


}
