using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;

namespace GameUI {
    public class NewWelshLearnedUI : UIController {
        public GameObject newWelshVocabPrefab, newWelshGrammarPrefab;
        private GameObject newWelshList;

        public void InsertDiscoveredVocab(string vocabEn, string vocabCy) {
            DbCommands.InsertTupleToTable("DiscoveredVocab", vocabEn, vocabCy, "0", "0");
        }

        public void InsertDiscoveredGrammar(string grammarID) {
            DbCommands.InsertTupleToTable("DiscoveredVocabGrammar", grammarID, "0", "0");
        }

        public void DisplayNewWelsh(string choiceID) {
            newWelshList = GetPanel().transform.FindChild("ScrollWindow").FindChild("NewWelshList").gameObject;
            EmptyDisplay(newWelshList.transform);
            AppendDisplayFromDb(DbQueries.GetCurrentActivateGrammarPlayerChoiceResultQry(choiceID), newWelshList.transform, BuildNewGrammar);
            AppendDisplayFromDb(DbQueries.GetCurrentActivateVocabPlayerChoiceResultQry(choiceID), newWelshList.transform, BuildNewVocab);
            Canvas.ForceUpdateCanvases();
            if (newWelshList.transform.childCount > 0) {
                DisplayComponents();
                Canvas.ForceUpdateCanvases();
            }
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

        public Transform BuildNewGrammar(string[] grammarData) {
            string id = grammarData[1];
            string summary = grammarData[2];
            string body = grammarData[3];
            NewWelshGrammar newWelshGrammar = (
                Instantiate(newWelshGrammarPrefab, new Vector3(0f, 0f), Quaternion.identity)
                ).GetComponent<NewWelshGrammar>();
            newWelshGrammar.InitialiseMe(id, summary, body);
            return newWelshGrammar.transform;
        }
    }
}