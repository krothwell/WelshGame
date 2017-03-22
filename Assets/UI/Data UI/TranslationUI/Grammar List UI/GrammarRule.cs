using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class GrammarRule : MonoBehaviour {
            TranslationUI translationUI;
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
            GameObject ruleSaveBtn;

            // Use this for initialization
            void Start() {
                translationUI = GameObject.FindObjectOfType<TranslationUI>().GetComponent<TranslationUI>();
                ruleOptions = gameObject.transform.FindChild("RuleOptions").gameObject;
                descriptionInput = gameObject.transform.FindChild("DescriptionInput").gameObject;
            }

            void Update() {
                SetSelectionOfTranslationOnClick();
            }

            public void SetSelectionOfTranslationOnClick() {
                if (Input.GetMouseButtonUp(0)) {
                    if (MouseSelection.IsClickedDifferentGameObjectTo(this.gameObject)) {
                        //if a rule is not being edited then the rule list is refreshed.
                        if (!translationUI.editingRule) {
                            if (ruleOptions.activeSelf) {
                                DeactivateRuleOptions();
                                translationUI.ActivateGrammarRulesMenu();
                            }
                        }
                    }
                }
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
                string welsh = translationUI.GetSelectedWelshText();
                string english = translationUI.GetSelectedEnglishText();
                Debug.Log(welsh);
                DbCommands.InsertTupleToTable("VocabRuleList", english, welsh, RuleNumber);
                addRuleBtn.GetComponent<Text>().text = "Remove";
                Image btnImg = gameObject.transform.Find("DescriptionInput").GetComponent<Image>();
                btnImg.color = Color.green;
                IsAssignedToSelectedTranslation = true;
            }

            public void DeleteFromRuleList() {
                string[,] ruleFields = new string[,] { { "WelshText", translationUI.GetSelectedWelshText() }, { "RuleIDs", RuleNumber } };
                DbCommands.DeleteTupleInTable("VocabRuleList", ruleFields);
                addRuleBtn.GetComponent<Text>().text = "Add rule";
                Image btnImg = gameObject.transform.Find("DescriptionInput").GetComponent<Image>();
                btnImg.color = Color.white;
                IsAssignedToSelectedTranslation = false;
            }

            public void ActivateAddRule(bool related) {
                addRuleBtn = gameObject.transform.FindChild("AddRemoveRule").gameObject;
                if (related) {
                    addRuleBtn.GetComponent<Text>().text = "Remove";
                    Image btnImg = gameObject.transform.Find("DescriptionInput").GetComponent<Image>();
                    btnImg.color = Color.green;
                    IsAssignedToSelectedTranslation = true;
                }
                else { IsAssignedToSelectedTranslation = false; }
                addRuleBtn.SetActive(true);
            }

            public void ActivateRule(GameObject go) {
                translationUI.SetActiveRule(go);
                print("test activaterule " + go);
            }

            public void ActivateRuleOptions() {
                Image btnImg = descriptionInput.GetComponent<Image>();
                btnImg.color = Color.cyan;
                ruleOptions.SetActive(true);
            }

            public void EditRule() {
                translationUI.SetActiveRuleEdit();
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

            void OnMouseUpAsButton() {
                ActivateRule(gameObject);
                ActivateRuleOptions();
            }
        }
    }
}