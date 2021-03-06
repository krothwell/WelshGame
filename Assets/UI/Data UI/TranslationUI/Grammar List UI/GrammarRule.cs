﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DbUtilities;
using DataUI.Utilities;

namespace DataUI {
    namespace ListItems {
        public class GrammarRule : MonoBehaviour, ISelectableUI {
            VocabTranslationListUI vocabTranslationListUI;
            GrammarListUI grammarListUI;
            private string currentDescription;
            public string CurrentDescription {
                get { return currentDescription; }
                set { currentDescription = value; }
            }

            private string ruleNumber;
            public string RuleNumber {
                get { return ruleNumber; }
                set { ruleNumber = value; }
            }

            private bool isAssignedToSelectedTranslation;
            public bool IsAssignedToSelectedTranslation {
                get { return isAssignedToSelectedTranslation; }
                set { isAssignedToSelectedTranslation = value; }
            }

            GameObject ruleOptions;
            GameObject descriptionInput;
            GameObject addRuleBtn;

            // Use this for initialization
            void Start() {
                grammarListUI = FindObjectOfType<GrammarListUI>();
                vocabTranslationListUI = FindObjectOfType<VocabTranslationListUI>();
                ruleOptions = gameObject.transform.Find("RuleOptions").gameObject;
                descriptionInput = gameObject.transform.Find("DescriptionInput").gameObject;
                
            }

            public void ToggleInsertRemoveToRuleList() {
                if (isAssignedToSelectedTranslation) {
                    DeleteFromRuleList();
                }
                else {
                    InsertToRuleList();
                }
            }

            public void InsertToRuleList() {
                Translation currentTranslation = (Translation)vocabTranslationListUI.GetSelectedItemFromGroup(vocabTranslationListUI.VocabTranslationSelected);
                string english = currentTranslation.CurrentEnglish;
                string welsh = currentTranslation.CurrentWelsh;
                DbCommands.InsertTupleToTable("VocabRuleList", english, welsh, ruleNumber);
                addRuleBtn.GetComponent<Text>().text = "Remove";
                Image btnImg = gameObject.transform.Find("DescriptionInput").GetComponent<Image>();
                btnImg.color = Color.green;
                IsAssignedToSelectedTranslation = true;
            }


            public void DeleteFromRuleList() {
                Translation currentTranslation = (Translation)vocabTranslationListUI.GetSelectedItemFromGroup(vocabTranslationListUI.VocabTranslationSelected);
                string[,] ruleFields = new string[,] { { "WelshText", currentTranslation.CurrentWelsh }, { "RuleIDs", RuleNumber } };
                DbCommands.DeleteTupleInTable("VocabRuleList", ruleFields);
                addRuleBtn.GetComponent<Text>().text = "Add rule";
                Image btnImg = gameObject.transform.Find("DescriptionInput").GetComponent<Image>();
                btnImg.color = Color.white;
                IsAssignedToSelectedTranslation = false;
            }

            public void ActivateAddRule(bool related) {
                addRuleBtn = gameObject.transform.Find("AddRemoveRule").gameObject;
                if (related) {
                    addRuleBtn.GetComponent<Text>().text = "Remove";
                    Image btnImg = gameObject.transform.Find("DescriptionInput").GetComponent<Image>();
                    btnImg.color = Color.green;
                    IsAssignedToSelectedTranslation = true;
                }
                else { IsAssignedToSelectedTranslation = false; }
                addRuleBtn.SetActive(true);
            }

            public void DeactivateTag() {
                addRuleBtn = gameObject.transform.Find("AddRemoveRule").gameObject;
                Image btnImg = gameObject.transform.Find("DescriptionInput").GetComponent<Image>();
                btnImg.color = Colours.colorDataUIPanelInactive;
                IsAssignedToSelectedTranslation = false;
                addRuleBtn.SetActive(false);
            }

            public void ActivateRuleOptions() {
                Image btnImg = descriptionInput.GetComponent<Image>();
                btnImg.color = Color.cyan;
                ruleOptions.SetActive(true);
            }

            public void EditRule() {
                grammarListUI.SetActiveRuleEdit();
            }

            public void UpdateRuleDisplay(string newText) {
                descriptionInput.GetComponent<InputField>().text = newText;
            }

            public void DeactivateRuleOptions() {
                Image btnImg = descriptionInput.GetComponent<Image>();
                btnImg.color = Color.white;
                ruleOptions.SetActive(false);
            }

            public void SelectText() {
                EventSystem.current.SetSelectedGameObject(descriptionInput, null);
            }

            public void DeleteRule() {
                string[,] fields = { { "RuleIDs", RuleNumber } };
                DbCommands.DeleteTupleInTable("VocabGrammar",
                                             fields);
                Destroy(gameObject);
            }

            public void SelectSelf() {
                vocabTranslationListUI.DeactivateSelectedTranslation();
                grammarListUI.ClearTagging();
                ActivateRuleOptions();
            }

            public void DeselectSelf() {
                DeactivateRuleOptions();
            }

            void OnMouseUpAsButton() {
                grammarListUI.ToggleSelectionTo(this, grammarListUI.SelectedGrammarRule);
            }

        }
    }
}