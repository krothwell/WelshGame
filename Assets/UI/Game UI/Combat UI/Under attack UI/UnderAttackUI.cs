using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderAttackUI : UIController {
    public GameObject EnemySelectorBtnPrefab;
    Transform enemiesList;
    Dictionary<Character, GameObject> enemyDict;

    void Start() {
        enemiesList = GetPanel().transform.Find("EnemiesList");
        enemyDict = new Dictionary<Character, GameObject>();
        Reset();
    }

    public void InsertAttacker(Character charIn) {
        GameObject enemySelectorBtn = Instantiate(EnemySelectorBtnPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
        enemySelectorBtn.GetComponent<EnemySelectorBtn>().InitialiseMe(charIn.GetMyPortrait(), ((NPCCharacter)charIn).MySelector);
        enemySelectorBtn.transform.SetParent(enemiesList, false);
        print("enemy added");
        enemyDict.Add(charIn, enemySelectorBtn);
    }

    public void RemoveAttacker(Character charIn) {
        Destroy(enemyDict[charIn]);
        enemyDict.Remove(charIn);
    }

    public void Reset() {
        if (enemyDict.Count < 1) {
            foreach(Transform gotransform in enemiesList.transform) {
                Destroy(gotransform.gameObject);
            }
            HideComponents();
        }
    }
}
