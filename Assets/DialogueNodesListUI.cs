using UnityEngine;
using DataUI.ListItems;
using DbUtilities;
using UnityEngine.UI;
using UnityUtilities;

namespace DataUI {
    public class DialogueNodesListUI : UIController {
        //PLAYER CHOICES
        DialogueUI dialogueUI;
        Transform dialogueNodesList;
        public GameObject DialogueNodeTextOnlyPrefab, DialogueNodeVocabTestPrefab;
        public string SelectedNode;

        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
            //PLAYER CHOICE COMPONENTS
            dialogueNodesList = GetPanel().transform.Find("ScrollView").GetComponentInChildren<VerticalLayoutGroup>().transform;
            SelectedNode = "SelectedNode";
            CreateSelectionToggleGroup(SelectedNode);
        }

        public Transform BuildDialogueNodeTextOnly(string[] strArray) {
            string idStr = strArray[0];
            string nodeText = strArray[1];
            GameObject dialogueNodeTextOnly = Instantiate(DialogueNodeTextOnlyPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            dialogueNodeTextOnly.GetComponent<DialogueNodeTextOnly>().InitialiseDisplay(nodeText, idStr);
            dialogueNodeTextOnly.GetComponent<DialogueNode>().InitialiseMe(nodeText, idStr);
            return dialogueNodeTextOnly.transform;
        }

        public Transform BuildDialogueNodeVocabTest(string[] strArray) {
            string idStr = strArray[0];
            print(idStr);
            string enText = strArray[1];
            string cyText = strArray[2];
            GameObject dialogueNodeVocabTest = Instantiate(DialogueNodeVocabTestPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            //playerChoiceVocab.GetComponent<>
            dialogueNodeVocabTest.GetComponent<DialogueNodeVocabTest>().InitialiseDisplay(enText, cyText, idStr);
            print(idStr);
            print(enText);
            print(dialogueNodeVocabTest.GetComponent<DialogueNode>());
            dialogueNodeVocabTest.GetComponent<DialogueNode>().InitialiseMe(enText, idStr);
            return dialogueNodeVocabTest.transform;
        }

        public void DisplayNodesRelatedToDialogue() {
            DisplayComponents();
            Dialogue currentDialogue = (dialogueUI.GetSelectedItemFromGroup(dialogueUI.selectedDialogue) as Dialogue);
            FillDisplayFromDb(DbQueries.GetDialogueNodeDisplayQry(currentDialogue.MyID), dialogueNodesList, BuildDialogueNodeTextOnly);
            AppendDisplayFromDb(DbQueries.GetDialogueNodeVocabDisplayQry(currentDialogue.MyID), dialogueNodesList, BuildDialogueNodeVocabTest);
            Debugging.PrintDbQryResults(DbQueries.GetDialogueNodeVocabDisplayQry(currentDialogue.MyID));
            Debugging.PrintDbTable("DialogueNodesVocabTests");
            Debugging.PrintDbTable("DialogueNodes");
        }
    }
}