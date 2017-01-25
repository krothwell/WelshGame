using UnityEngine;
using System.Collections;

public class AnimToggles : MonoBehaviour {
    Animator myAnimator;
    Character character;

    private bool isWalking;
    public bool IsWalking {
        get { return isWalking; }
        set { isWalking = value; }
    }
    private bool isRunning;
    public bool IsRunning {
        get { return isRunning; }
        set { isRunning = value; }
    }

    void Start() {
        
        myAnimator = gameObject.GetComponent<Animator>();
        character = myAnimator.gameObject.GetComponent<Character>();
    }
    public void StartWalking() {
        if (isWalking) {
            myAnimator.SetBool("isWalking", true);
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isFighting", false);
        }

    }

    public void StartRunning() {
        if (isRunning) {
            myAnimator.SetBool("isRunning", true);
            myAnimator.SetBool("isWalking", false);
            myAnimator.SetBool("isFighting", false);
        }

    }

    public void Idle1() {
        if (!isWalking && !isRunning) {
            myAnimator.SetBool("isWalking", false);
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("idle1", true);
            myAnimator.SetBool("isFighting", false);
        }
    }
    public void Idle2() {
        if (!isWalking && !isRunning) {
            myAnimator.SetBool("isWalking", false);
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("idle1", false);
            myAnimator.SetBool("isFighting", false);
        }


    }

    public void StartFighting() {
        if (character.IsFighting) {
            myAnimator.SetBool("isFighting", true);
        } else {
            myAnimator.SetBool("isFighting", false);
        }
    }

    public void Strike() {
        myAnimator.SetTrigger("Strike");
    }
}
