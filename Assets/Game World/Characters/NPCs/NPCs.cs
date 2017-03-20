using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Responsible for providing an easy way for other classes to retrieve NPC
/// character objects in the scene. Most of the time this is by the name of the
/// character, so a dictionary is built at the start of the scene to reference
/// the character objects by string.
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
            string name = npcChar.CharacterName;
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
