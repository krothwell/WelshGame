using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;

namespace GameUI {
    public class NewWelshLearnedUI : UIController {
        public GameObject newWelshVocabPrefab;
        private GameObject newWelshList;

        public void InsertNewVocab(string vocabEn, string vocabCy) {
            DbCommands.InsertTupleToTable("DiscoveredVocab", vocabEn, vocabCy, "0", "0");
        }

        public void InsertNewGrammar() {

        }

        public void DisplayNewWelsh(string choiceID) {
            newWelshList = GetPanel().transform.FindChild("ScrollWindow").FindChild("NewWelshList").gameObject;
            FillDisplayFromDb(DbQueries.GetCurrentActivateVocabPlayerChoiceResultQry(choiceID), newWelshList.transform, BuildNewVocab);
            DisplayComponents();
        }

        public Transform BuildNewVocab(string[] vocabData) {
            string eng = vocabData[2];
            string cym = vocabData[3];
            NewWelshVocab newWelshVocab = (
                Instantiate(newWelshVocabPrefab, new Vector3(0f, 0f), Quaternion.identity)
                ).GetComponent<NewWelshVocab>();
            newWelshVocab.InitialiseMe(eng, cym);
            return newWelshVocab.transform;
        }
    }
}