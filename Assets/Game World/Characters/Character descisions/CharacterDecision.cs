using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterDecision : MonoBehaviour {
    protected Character myCharacter;
    protected bool isEnding;
    public bool IsEnding {
        get { return isEnding; }
        set { isEnding = value; }
    }
    public void InitialiseMe(Character character) {
        myCharacter = character;
    }

    public abstract void ProcessDecision();

    public abstract void EndDecision();


}
