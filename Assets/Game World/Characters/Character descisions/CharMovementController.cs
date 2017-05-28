
using UnityEngine;
using GameUtilities.Display;
using GameUtilities;

public class CharMovementController : MonoBehaviour {

    protected CharacterMovementDecision currentMovementDecision;

    protected Vector2 targetPosition, //the overall target unless changed for some reason
                    interimPosition, //the position to go to if there is something in the way
                    myPosition, //where I am if I'm keeping track properly
                    nextPosition; // the position I'm actually going towards, could be target position, could be interim position.

    //public float rawDistanceX, rawDistanceY;
    protected int rerouteCount, rerouteLimit;

    protected bool redirecting = false, isMoving = false;

    private CollisionAvoider collisionAvoider;

    private GameObject[] characterParts;

    private Transform imageRoot;

    protected CharacterMovement movement;
    public CharacterMovement Movement {
        get { return movement; }
        set { movement = value; }
    }
    protected Character character;
    public Character Character {
        get { return character; }
        set { character = value; }
    }


    public void InitialiseMe () {
        collisionAvoider = GetComponent<CollisionAvoider>();
        collisionAvoider.InitialiseMe(character, this);
        imageRoot = character.ImageRoot;
        rerouteLimit = 50;
        characterParts = character.CharacterParts;
        SetMyOrder(characterParts);
        character.MovementController = this;
    }

    public void SetTargetPosition (Vector2 targetPos) {
        targetPosition = targetPos;
    }

    public Vector2 GetTargetPosition() {
        return targetPosition;
    }

    protected void SetInterimPostion(Vector2 interimPos) {
        interimPosition = interimPos;
        redirecting = true;
    }

    public void SetNextPosition() {
        if (redirecting) {
            nextPosition = interimPosition;
        }
        else {
            nextPosition = targetPosition;
            SetMyDirection(targetPosition, character.GetMyPosition());
        }
    }

    public Vector2 GetNextPosition() {
        return nextPosition;
    }

    public void CheckToMakeMovement() {
        if (isMoving) {
            MoveTowardsPosition();
        }
    }

    public void MoveTowardsPosition() {
        SetNextPosition();
        if (myPosition != nextPosition && rerouteCount < rerouteLimit) {
            float speed = movement.GetMovementSpeed();
            Vector2 distanceXY = GetXYDistancesFromMyPositonToNextPosition();
            collisionAvoider.SetCollisionDetector();
            //to walk as the crow flies diagonally, a percentage is captured for each axis
            float percentageOfTravelX = (100f / distanceXY.x) * (1 * speed * Time.deltaTime);
            float percentageOfTravelY = (100f / distanceXY.y) * (1 * speed * Time.deltaTime);
            //the greatest distance of an axis will go at the normal speed, while the shorter distance will move at the percentage of the distance travelled by the other axis
            float newX = distanceXY.x > distanceXY.y ? (speed * Time.deltaTime) : distanceXY.x / 100 * percentageOfTravelY;
            float newY = distanceXY.y > distanceXY.x ? (speed * Time.deltaTime) : distanceXY.y / 100 * percentageOfTravelX;
            int xModifier = (myPosition.x >= nextPosition.x) ? -1 : 1;
            int yModifier = (myPosition.y >= nextPosition.y) ? -1 : 1;
            SetMyOrder(characterParts);
            SetMyPosition(xModifier, yModifier, newX, newY);
        } else {
            currentMovementDecision.EndDecision();
            rerouteCount = 0;
        }

    }

    public void StopMoving() {
        ToggleIsMoving(false);
        ResetRerouteCount();
        EndRedirect();
    }

    private void SetMyPosition(int xModifier, int yModifier, float newX, float newY) {
        character.transform.Translate(new Vector2(xModifier * newX, yModifier * newY));
        myPosition = character.GetMyPosition();
        if (myPosition == interimPosition) {
            redirecting = false;
        }


    }

    public void SetInterimPosition(Vector2 position, bool redirect) {
        interimPosition = position;
        redirecting = redirect;
        
        rerouteCount++;
    }

    public void ResetRerouteCount() {
        rerouteCount = 0;
    }

    public int GetRerouteCount() {
        return rerouteCount;
    }

    public void SetMyDirection(Vector2 targettedPosition, Vector2 myPosition) {
        //depending on which direction the player is going, changes the modifier to decrease or increase on that axis
        int xModifier = myPosition.x >= targettedPosition.x ? -1 : 1;
        Vector3 spriteDirection = new Vector3(xModifier,
                                      imageRoot.localScale.y,
                                      imageRoot.GetComponent<Transform>().localScale.z);
        if (imageRoot.GetComponent<Transform>().localScale.x != spriteDirection.x) {
            imageRoot.GetComponent<Transform>().localScale = spriteDirection;
        }
    }

    public void SetMyOrder(GameObject[] characterParts) {
        ImageLayerOrder.SetOrderOnGameObjectArray(characterParts, ImageLayerOrder.GetOrderInt(gameObject) - 1);
        ImageLayerOrder.SetZ(character.gameObject);
    }

    public Vector2 GetXYDistancesFromMyPositonToNextPosition() {
        return World.GetVector2DistanceFromPositions2D(myPosition, nextPosition);
    }

    public float GetDistanceFromMyPosition(Vector2 otherPosition) {
        return World.GetDistanceFromPositions2D(myPosition, otherPosition);
    }
    
    public void ToggleIsMoving(bool moving) {
        print("moving? : " + moving);
        isMoving = moving;
    }

    public bool GetIsMoving() {
        return isMoving;
    }

    public CharacterMovement GetMyMovement() {
        return movement;
    }

    public void SetMovementDecision(CharacterMovementDecision movementDecision) {
        currentMovementDecision = movementDecision;
    }

    public void EndRedirect() {
        redirecting = false;
    }

    public float GetInteractionDistance() {
        return 1f;
    }

    public void ProcessMovement(CharacterMovement movementType) {
        movement = movementType;
        SetMyDirection(targetPosition, myPosition);
        print(character);
        CharacterDecision decision = character.MyDecision;
        print(decision);
        if (decision.GetType() == typeof(CharacterMovementDecision)) {
            collisionAvoider = character.MyDecision.GetComponent<CollisionAvoider>();
        }
        movement.MakeAction();
        ToggleIsMoving(true);
    }



}
