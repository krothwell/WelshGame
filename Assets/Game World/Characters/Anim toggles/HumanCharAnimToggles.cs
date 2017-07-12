using UnityEngine;
using System.Collections;

/// <summary>
/// Handles special cases in the animator where the next animation is dependant on what frame another animation is at.
/// These methods are used in the frames of the animation to switch to the correct animation at the right time.
/// </summary>
public class HumanCharAnimToggles : MonoBehaviour {
    Animator myAnimator;
    Character character;
    private CharacterMovement myMovement;
    public CharacterMovement MyMovement {
        get { return myMovement; }
        set { myMovement = value; }
    }

    void Start() {
        character = GetComponent<Character>();
        myAnimator = gameObject.GetComponent<Animator>();
    }
    //public void StartWalking() {
    //    if (isWalking) {

    //        myAnimator.SetBool("isFighting", false);
    //    }

    //}

    //public void StartRunning() {
    //    if (isRunning) {
    //        myAnimator.SetBool("isRunning", true);
    //        myAnimator.SetBool("isWalking", false);
    //        myAnimator.SetBool("isFighting", false);
    //    }

    //}

    public void Idle1() {
        //if (!isMoving()) {
        //    if (GetMovement().gameObject) {
        //        GetMovement().StopAction();
        //        print(myAnimator.transform);
                myAnimator.SetBool("idle1", true);
       //     }
        
    }
    public void Idle2() {
        //if (!isMoving()) {
        //    if (GetMovement().gameObject) {
        //        GetMovement().StopAction();
                myAnimator.SetBool("idle1", false);
        //    }
        //}
    }

    private bool isMoving() {
        return character.MovementController.GetIsMoving();
    }

    public void StartFighting() {
        //if (character.IsFighting) {
        //    myAnimator.SetBool("isFighting", true);
        //} else {
        //    myAnimator.SetBool("isFighting", false);
        //}
    }

    public void Strike() {
        myAnimator.SetTrigger("Strike");
    }

    public CharacterMovement GetMovement() {
        print(character.MovementController);
        print(character.MovementController.GetMyMovement());
        return character.MovementController.GetMyMovement();
    }
}
