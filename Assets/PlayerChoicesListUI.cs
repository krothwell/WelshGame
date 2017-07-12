using UnityEngine;
using DataUI.ListItems;
using DbUtilities;

namespace DataUI {
    public class PlayerChoicesListUI : UIController {
        //PLAYER CHOICES
        DialogueNodesListUI dialogueNodesListUI;
        GameObject playerChoicesList;
        public GameObject PlayerChoicePrefab, PlayerChoiceVocabTestPrefab;
        public string SelectedChoice;

        void Start() {
            dialogueNodesListUI = FindObjectOfType<DialogueNodesListUI>();
            //PLAYER CHOICE COMPONENTS
            playerChoicesList = GetPanel().transform.Find("PlayerChoicesList").gameObject;

            SelectedChoice = "selectedChoice";
            CreateSelectionToggleGroup(SelectedChoice);
        }

        public Transform BuildPlayerChoiceTextOnly(string[] strArray) {
            string idStr = strArray[0];
            string choiceText = strArray[1];
            string nextNode = strArray[3];
            GameObject playerChoiceTextOnly = Instantiate(PlayerChoicePrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            playerChoiceTextOnly.GetComponent<PlayerChoiceTextOnly>().InitialiseDisplay(choiceText, idStr);
            playerChoiceTextOnly.GetComponent<PlayerChoice>().InitialiseMe(choiceText, idStr, nextNode);
            return playerChoiceTextOnly.transform;
        }

        public Transform BuildPlayerChoiceVocab(string[] strArray) {
            string idStr = strArray[0];
            string nextNode = strArray[1];
            string enText = strArray[2];
            string cyText = strArray[3];
            GameObject playerChoiceVocab = Instantiate(PlayerChoiceVocabTestPrefab, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            //playerChoiceVocab.GetComponent<>
            playerChoiceVocab.GetComponent<PlayerChoiceVocabTest>().InitialiseDisplay(enText, cyText, idStr);
            playerChoiceVocab.GetComponent<PlayerChoice>().InitialiseMe(enText, idStr, nextNode);
            return playerChoiceVocab.transform;
        }

        public void DisplayChoicesRelatedToNode() {
            GetPanel().SetActive(true);
            DialogueNode currentDialogueNode = (dialogueNodesListUI.GetSelectedItemFromGroup(dialogueNodesListUI.SelectedNode) as DialogueNode);
            FillDisplayFromDb(DbQueries.GetPlayerChoiceDisplayQry(currentDialogueNode.MyID), playerChoicesList.transform, BuildPlayerChoiceTextOnly);
            AppendDisplayFromDb(DbQueries.GetPlayerChoiceVocabDisplayQry(currentDialogueNode.MyID), playerChoicesList.transform, BuildPlayerChoiceVocab);
        }

        public void HidePlayerChoices() {
            GetPanel().SetActive(false);
        }




    }
}