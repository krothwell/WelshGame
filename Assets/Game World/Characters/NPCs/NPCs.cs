using UnityEngine;
using System.Collections.Generic;
using GameUI;
using DbUtilities;

/// <summary>
/// Responsible for providing an easy way for other classes to retrieve NPC
/// character objects in the scene. Most of the time this is by the name of the
/// character, so a dictionary is built at the start of the scene to reference
/// the character objects by string.
/// </summary>
public class NPCs : MonoBehaviour {
    private Dictionary<string, Character> npcCharDict;
    private Dictionary<string, List<Character>> npcCharTags;
    // Use this for initialization
    void Awake () {
        npcCharDict = new Dictionary<string, Character>();
        npcCharTags = new Dictionary<string, List<Character>>();
        SetCharacterData();
    }

    void Start() {
        UpdateCharactersTableFromGame();
    }

    private void SetCharacterData() {
        foreach (Transform npc in transform) {
            Character npcChar = npc.GetComponent<Character>();
            string name = npcChar.CharacterName;
            string tag = npcChar.CharacterTag;
            npcCharDict.Add(name, npcChar);
            if (!npcCharTags.ContainsKey(tag)) {
                npcCharTags.Add(tag, new List<Character>());
                npcCharTags[tag].Add(npcChar);
            } else {
                npcCharTags[tag].Add(npcChar);
            }
        }
    }

    public Dictionary<string, Character> GetCharDict() {
        return npcCharDict;
    }

    public List<string> GetCharNamesList() {
        List<string> charNamesList = new List<string>(npcCharDict.Keys);
        return charNamesList;
    }

    public List<string> GetCharTagsList() {
        List<string> npcCharTagsList = new List<string>(npcCharTags.Keys);
        return npcCharTagsList;
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

    public void RemoveCharacter(string charName) {
        if (npcCharDict.ContainsKey(charName)) {
            Character charRef = npcCharDict[charName];
            npcCharTags[charRef.CharacterTag].Remove(charRef);
            if (npcCharTags[charRef.CharacterTag].Count < 1) {
                npcCharTags.Remove(charRef.CharacterTag);
                FindObjectOfType<QuestsUI>().CompleteDefeatEnemyTagTaskPart(charRef.CharacterTag);
            }
            npcCharDict.Remove(charName);
        }
    }

    /// <summary>
    /// The characters from the game are updated so that you know which ones still exist in the scene and are given the option
    /// to delete them (from the Data UI -> Dialogues UI -> Character lists(s?)) when the list is built. New characters in the scene are also added.
    /// </summary>
    public void UpdateCharactersTableFromGame() {
        SceneLoader sceneLoader = new SceneLoader();
        string currentScene = sceneLoader.GetCurrentSceneName();
        InsertCharsNotInDbFromScene(currentScene);
        List<string[]> characterNamesList = new List<string[]>();
        DbCommands.GetDataStringsFromQry(DbQueries.GetCharacterNamesWithScene(currentScene), out characterNamesList, currentScene);
        UpdateCharsInDbNoLongerInScene(characterNamesList);
    }

    /// <summary>
    /// Inserts any character in the scene that is not yet in the characters table.
    /// </summary>
    private void InsertCharsNotInDbFromScene(string currentScene) {
        NPCs npcs = FindObjectOfType<NPCs>();
        foreach (string npcName in npcs.GetCharNamesList()) {
            DbCommands.InsertTupleToTable("Characters", npcName, currentScene);
        }
    }

    /// <summary>
    /// Check a list of npc names from the database against the npc names in the current scene so
    /// if the names are not there (meaning they have been removed from the scene) then the table 
    /// of characters is updated to remove the scene from the character table scene field and in 
    /// turn informs the user using the data dialogue UI.
    /// </summary>
    /// <param name="namesList">A list of character names</param>
    private void UpdateCharsInDbNoLongerInScene(List<string[]> namesList) {
        NPCs npcs = FindObjectOfType<NPCs>();
        foreach (string[] nameBox in namesList) {
            string npcName = nameBox[0];
            string npcParam;
            if (npcName == "") {
                npcParam = "''";
            }
            else {
                npcParam = DbCommands.GetParameterNameFromValue(npcName);
            }
            if (!npcs.IsNameInCharDict(npcName)) {
                DbCommands.UpdateTableField(
                    "Characters",
                    "Scenes",
                    "null",
                    "CharacterNames = " + npcParam,
                    npcName
                    );
            }
        }
    }
}
