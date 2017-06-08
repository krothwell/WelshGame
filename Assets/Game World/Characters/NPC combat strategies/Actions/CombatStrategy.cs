using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Instantiated by NPCs combat controller.
//Uses an array of CombatStrategyActions, iterates through the list.
//for each iteration, the strategy action is instantiated and ran.
//While this class is instantiated, it checks the currently instantiated strategy action
//If the strategy action game object becomes null, it will move on to the next strategy action in the list
//Once the list has been iterated, it will start again from the beginning
public class CombatStrategy : MonoBehaviour {
    private Character myCharacter;
    public Character MyCharacter {
        get { return myCharacter; }
        set { myCharacter = value; }
    }
    public GameObject[] CombatStrategyActions;
    CombatStrategyAction currentCombatStrategyAction;
    private int nextActionIndex = 0;

    void Update() {
        if (currentCombatStrategyAction != null) {
            if (currentCombatStrategyAction.IsActionOver()) {
                StopCurrentAction();
                StartNextAction();
            }
        }
    }

    public void StopCurrentAction() {
        if (currentCombatStrategyAction != null) {
            currentCombatStrategyAction.EndAction();
        }
    }

    public void StartNextAction() {
        if (nextActionIndex < CombatStrategyActions.Length) {
            GameObject combatStrategyAction = Instantiate(CombatStrategyActions[nextActionIndex], new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            //print(combatStrategyAction);
            //print(myCharacter);
            combatStrategyAction.transform.SetParent(myCharacter.transform, false);
            
            currentCombatStrategyAction = combatStrategyAction.GetComponent<CombatStrategyAction>();
            //print(currentCombatStrategyAction.MyAction);
            currentCombatStrategyAction.MyAction.transform.SetParent(myCharacter.transform, false);
            currentCombatStrategyAction.InitialiseAction(myCharacter);
            currentCombatStrategyAction.transform.SetParent(myCharacter.transform, false);
            currentCombatStrategyAction.DoAction();
            nextActionIndex++;
        } else {
            if (nextActionIndex != 1) {
                nextActionIndex = 0;
                StartNextAction();
            }
        }
    }

    public void DestroyMe() {
        if (currentCombatStrategyAction != null) {
            currentCombatStrategyAction.EndAction();
            currentCombatStrategyAction = null;
        }
        Destroy(currentCombatStrategyAction);
        Destroy(gameObject);
        Destroy(this);
    }
}
