using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatStrategyAction : MonoBehaviour {

    public GameObject myActionPrefab;
    protected GameObject myAction;
    public GameObject MyAction {
        get { return myAction; }
        set { myAction = value; }
    }
    protected Character myCharacter;
    public Character MyCharacter {
        get { return myCharacter; }
        set { myCharacter = value; }
    }

    void Awake() {
        BuildAction();
    }

    public abstract void DoAction();
    public bool IsActionOver() {
        if (myAction.gameObject == null) {
            return true;
        }
        else {
            return false;
        }
    }
    public void EndAction() {
        if (gameObject != null) {
            Destroy(gameObject);
        }
        Destroy(this);
    }

    protected void BuildAction() {
        if (myAction == null) {
            //print(myActionPrefab);
            myAction = Instantiate(myActionPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            //print("build action: " + myAction);        
        }
    }

    public abstract void InitialiseAction(Character characterIn);
}
