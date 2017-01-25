using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NPCs : MonoBehaviour {
    private Dictionary<string, Character> npcCharDict;
    // Use this for initialization
    void Start () {
        npcCharDict = new Dictionary<string, Character>();
        SetCharacterDictionary();
    }

    private void SetCharacterDictionary() {
        foreach (Transform npc in transform) {
            Character npcChar = npc.GetComponent<Character>();
            Debug.Log(npcChar);
            string name = npcChar.nameID;
            npcCharDict.Add(name, npcChar);
        }
    }

    public Character GetCharacterFromName(string name) {
        if (npcCharDict.ContainsKey(name)) {
            return npcCharDict[name];
        } else {
            Debug.Log("Character name not found in NPCs list");
            return null;
        }
    }
}
