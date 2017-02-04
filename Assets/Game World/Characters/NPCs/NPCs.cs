using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// Builds a dictionary in start to hold all the characters so another class can easily get the character
/// object from the name of the character. Useful functions related to character retrieval are also provided
/// for updating related database tables as required.
/// </summary>
public class NPCs : MonoBehaviour {
    private Dictionary<string, Character> npcCharDict;
    // Use this for initialization
    void Awake () {
        npcCharDict = new Dictionary<string, Character>();
        SetCharacterDictionary();
    }

    private void SetCharacterDictionary() {
        foreach (Transform npc in transform) {
            Character npcChar = npc.GetComponent<Character>();
            string name = npcChar.nameID;
            npcCharDict.Add(name, npcChar);
        }
    }

    public Dictionary<string, Character> GetCharDict() {
        return npcCharDict;
    }

    public List<string> GetCharNamesList() {
        List<string> charNamesList = new List<string>(npcCharDict.Keys);
        return charNamesList;
    }

    public Character GetCharacterFromName(string name) {
        if (npcCharDict.ContainsKey(name)) {
            return npcCharDict[name];
        } else {
            Debug.Log("Character name not found in NPCs list");
            return null;
        }
    }

    public bool IsNameInCharDict(string name) {
        if (npcCharDict.ContainsKey(name)) {
            return true;
        }
        else {
            return false;
        }
    }
}
