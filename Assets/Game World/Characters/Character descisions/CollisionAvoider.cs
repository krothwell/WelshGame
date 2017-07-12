using UnityEngine;
using GameUtilities;
using System;
using UnityUtilities;

/// <summary>
/// NOTE: This may need to be merged with Character class, while doling out 
/// some generic functions to a utilities namespace.
///
/// A line collider is used which spans the distance between the attached 
/// character and the target location which the character is going towards.
/// This class is responsible for controlling the line collider by 
/// specifying the end and starting points of the line. When it detects an 
/// obstacle (Perimeter object), the class is responsible for redirecting 
/// the character by choosing the corner of the obstacle nearest the 
/// destination to direct the character to. Once the redirection has 
/// occurred, the character will try once again to go to the chosen location
/// unless interrupted somewhere along the way.  
/// </summary>
public class CollisionAvoider : MonoBehaviour {
    float xDirection, yDirection, distanceX, distanceY;
    private CharMovementController characterMovementController;
    public CharMovementController CharacterMovementController {
        get { return characterMovementController; }
        //set { characterMovementController = value; }
    }
    private Character character;
    public Character Character {
        get { return character; }
        //set { character = value; }
    }
    Vector2 charPos;
    Vector2 closestCorner;
    GameObject myPerimeter, selected, objCurrentlyAvoiding;
    EdgeCollider2D collisionDetector;
    Vector2 myPerimeterSize;

    void Update () {
        //Time.timeScale = 0.5f;
        if (Input.GetMouseButtonUp(0)) {
            MouseSelection.ClickSelect(out selected);
        }
        
    }

    public void InitialiseMe(Character character_, CharMovementController movementController_) {
        character = character_;
        characterMovementController = movementController_;
        myPerimeter = character.Perimeter;
        myPerimeterSize = GetPerimeterSize(myPerimeter);
        collisionDetector = GetComponent<EdgeCollider2D>();
}

    void OnTriggerEnter2D(Collider2D trigger) {
        RedirectWhenObstacleDetected(trigger.gameObject);
    }

    public void RedirectWhenObstacleDetected(GameObject obstacle) {
        //print(trigger.gameObject);
        if (obstacle.name == "Perimeter") {
            if (obstacle != myPerimeter) {
                if (character.MyDecision.GetType() == typeof(MoveToCharacterDecision)) {
                    if (obstacle.transform.parent.gameObject != selected) {
                        print(obstacle.transform.parent.gameObject);
                        RedirectCharacter(obstacle);
                    }
                } else {
                    RedirectCharacter(obstacle);
                }
            }
        }
    }

    public Vector2 GetPerimeterSize(GameObject perimObj) {
        Vector2 objScale = new Vector2(Math.Abs(perimObj.GetComponent<Transform>().lossyScale.x),
                                       Math.Abs(perimObj.GetComponent<Transform>().lossyScale.y));
        Vector2 objColliderChildSize = perimObj.GetComponent<BoxCollider2D>().size;
        return new Vector2(objColliderChildSize.x * objScale.x, objColliderChildSize.y * objScale.y);
    }

    public void SetCollisionDetector() {
        /*the collision detector line's beginning is offset to the closest corner of the character's Perimeter, in the direction 
         * of which the character is travelling. This is to avoid situations where the line must detect a collision but doesn't
         * because the angle from the starting point to the destination point doesn't intersect the obstacles perimeter but the 
          character's perimeter does. */
        //ResetCollisionDetector();

        SetAxisDirection();
        float offsetX = (float)(myPerimeter.transform.localPosition.x + (xDirection * (myPerimeter.transform.localScale.x / 2)));
        float offsetY = (float)(myPerimeter.transform.localPosition.y + (yDirection * (myPerimeter.transform.localScale.y / 2)));
        collisionDetector.offset = new Vector2(offsetX, offsetY);

        //the 
        Vector2 bPosition = new Vector2 ((float)(myPerimeter.transform.position.x + (xDirection * (myPerimeter.transform.lossyScale.x / 2))),
                                         (float)(myPerimeter.transform.position.y + (yDirection * (myPerimeter.transform.lossyScale.y / 2))));

        //Vector2 distanceXY = World.GetVector2DistanceFromPositions2D(character.GetMyPosition(), bPosition);
        SetDistanceFromMyPosition(bPosition);
        float multiplier;
        multiplier = 1 / transform.parent.GetComponent<Transform>().localScale.x;
        Vector2[] newPoints = new Vector2[2];
        newPoints[0] = new Vector2(0f, 0f);
        newPoints[1] = new Vector2((distanceX) * multiplier, (distanceY) * multiplier);
        collisionDetector.points = newPoints;
    }

    private void ResetCollisionDetector() {
        Vector2[] newPoints = new Vector2[2];
        newPoints[0] = new Vector2(0f, 0f);
        newPoints[1] = new Vector2(0.01f, 0.01f);
        collisionDetector.points = newPoints;
    }

    private void SetDistanceFromMyPosition(Vector2 myPosition) {
        distanceX = characterMovementController.GetNextPosition().x - myPosition.x;
        distanceY = characterMovementController.GetNextPosition().y - myPosition.y;
    }

    public void RedirectCharacter(GameObject objectToAvoid) {
        if (characterMovementController.GetRerouteCount() == 0) {
            objCurrentlyAvoiding = null;
        }
        SetAxisDirection();
        closestCorner = ChooseCorner(objectToAvoid);
        characterMovementController.SetInterimPosition(closestCorner, true);
        characterMovementController.SetMyDirection(closestCorner, character.GetMyPosition());
    }

    private void SetAxisDirection () {
        xDirection = GetAxisDirection(characterMovementController.GetTargetPosition().x, character.GetMyPosition().x);
        yDirection = GetAxisDirection(characterMovementController.GetTargetPosition().y, character.GetMyPosition().y);
    }

    private float GetAxisDirection (float target, float current) {
        if (target > current) {
            return 1f;
        }
        else if (target < current) {
            return -1f;
        }
        else {
            return 1f;
        }
    }

    private Vector2 ChooseCorner(GameObject objToAvoid) {
        //perimeters scale should be 1,1,1.

        //print(objToAvoid + " of " + objToAvoid.transform.parent);
        Vector2 objPos = objToAvoid.GetComponent<Transform>().position;
        Vector2 objColliderActualSize = GetPerimeterSize(objToAvoid);
        float corner1x, corner1y, corner2x, corner2y, pOffSetX, pOffSetY;
        pOffSetX = myPerimeter.transform.localPosition.x;
        pOffSetY = myPerimeter.transform.localPosition.y;

        corner1x = objPos.x + (-xDirection * (objColliderActualSize.x / 2)) //center char on corner
                 + (-xDirection * (pOffSetX * myPerimeter.transform.lossyScale.x)) //center char perimeter on corner
                 - (float)(xDirection * (0.1 + (myPerimeterSize.x / 2))); //offset perimeter x to 0.1 away from corner
        corner2x = objPos.x + (xDirection * (objColliderActualSize.x / 2))
                 + (-xDirection * (pOffSetX * myPerimeter.transform.lossyScale.x))
                 + (float)(xDirection * (0.1 + (myPerimeterSize.x / 2)));

        corner1y = objPos.y + (yDirection * (objColliderActualSize.y / 2))
                 + (-yDirection * (pOffSetY * myPerimeter.transform.lossyScale.y))
                 + (float)(yDirection * (0.1 + (myPerimeterSize.y / 2)));
        corner2y = objPos.y + (-yDirection * (objColliderActualSize.y / 2)) //center char on corner
                 + (-yDirection * (pOffSetY * myPerimeter.transform.lossyScale.y)) //center char perimeter on corner
                 - (float)(yDirection * (0.1 + (myPerimeterSize.y / 2))); //offset perimeter x to 0.1 away from corner

        Vector2 corner1 = new Vector2((float)(Math.Round(corner1x, 1)), (float)Math.Round(corner1y, 1));
        Vector2 corner2 = new Vector2((float)(Math.Round(corner2x, 1)), (float)Math.Round(corner2y, 1));


        float distanceFromC1 = GetDistanceFromCharPosition(corner1);
        float distanceFromC2 = GetDistanceFromCharPosition(corner2);
        Vector2 chosenCorner = new Vector2();

        if (objCurrentlyAvoiding == null || objCurrentlyAvoiding != objToAvoid) {
            chosenCorner = (distanceFromC1 < distanceFromC2) ? corner1 : corner2;
        }
        else {
            chosenCorner = (distanceFromC1 > distanceFromC2) ? corner1 : corner2;
        }
        objCurrentlyAvoiding = objToAvoid; 
        return chosenCorner; 
    } 

    public float GetDistanceFromCharPosition(Vector2 newPosition) {
        charPos = character.GetMyPosition();
        return (float)Math.Sqrt((Math.Pow((double)(newPosition.x - charPos.x), 2)
                              + (Math.Pow((double)(newPosition.y - charPos.y), 2))));
    }


}
