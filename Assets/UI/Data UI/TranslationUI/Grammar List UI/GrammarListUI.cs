using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using DataUI.ListItems;
using DataUI.Utilities;

namespace DataUI {
    public class GrammarListUI : UIController {
        TranslationUI translationUI;
        //GRAMMAR UI
        //Grammar rules
        private string translationSelected;
        public string TranslationSelected {
            get { return translationSelected; }
            set { translationSelected = value; }
        }
        
        private GameObject grammarListUIpanel, grammarList,
                   submitNewGrammarRule, activateGrammarDetailsBtn,
                   grammarRule, activeRule;
        public GameObject GrammarList {
            get { return grammarList; }
            set { grammarList = value; }
        }
        public GameObject grammarRulePrefab;
        private GameObject newGrammarRulePanel;
            InputField inputRuleSdescTxt, inputRuleLdescTxt;
            Text viewGrammarDetailsBtnText;
            public bool editingRule = false;

        void Start() {
            translationSelected = "TranslationSelected";
            grammarListUIpanel = transform.Find("Panel").gameObject;
            grammarList = grammarListUIpanel.GetComponentInChildren<GridLayoutGroup>().gameObject;
            //adding and editing
            submitNewGrammarRule = grammarListUIpanel.transform.Find("SubmitNewRule").gameObject;
            activateGrammarDetailsBtn = submitNewGrammarRule.transform.Find("ActivateNewRuleBtn").gameObject;
            newGrammarRulePanel = submitNewGrammarRule.transform.Find("NewRulePanel").gameObject;
            inputRuleSdescTxt = newGrammarRulePanel.transform.Find("InputShortDescriptionText").GetComponent<InputField>();
            inputRuleLdescTxt = newGrammarRulePanel.transform.Find("InputLongDescriptionText").GetComponent<InputField>();
            viewGrammarDetailsBtnText = activateGrammarDetailsBtn.GetComponent<Text>();
            translationUI = FindObjectOfType<TranslationUI>();
            CreateSelectionToggleGroup(translationSelected);
        }

        public void SetActiveRule(GameObject rule) {
            activeRule = rule;
        }

        public void SetActiveRuleEdit() {
            viewGrammarDetailsBtnText.text = "Edit rule";
            editingRule = true;
            ActivateNewRule();
        }

        //Display a panel to add or edit a new grammar rule 
        public void ActivateNewRule() {
            newGrammarRulePanel.SetActive(true);
            activateGrammarDetailsBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUItxt); //indicate to user that button no longer functions.
            if (editingRule) {
                string[] ruleDesc = DbCommands.GetTupleFromTable("VocabGrammar",
                    "RuleIDs = " + activeRule.GetComponent<GrammarRule>().RuleNumber + ";");
                inputRuleSdescTxt.text = ruleDesc[1];
                inputRuleLdescTxt.text = ruleDesc[2];
            }
        }

        public void UpdateInsertRule() {
            if ((inputRuleSdescTxt.text != null) && (inputRuleSdescTxt.text != "")) {
                if (editingRule) {
                    string[,] fieldVals = new string[,] {
                                                { "ShortDescriptions", inputRuleSdescTxt.text },
                                                { "LongDescriptions", inputRuleLdescTxt.text },
                                            };
                    print(activeRule);
                    print(editingRule);
                    DbCommands.UpdateTableTuple("VocabGrammar", "RuleIDs = " + activeRule.GetComponent<GrammarRule>().RuleNumber, fieldVals);
                    activeRule.GetComponent<GrammarRule>().UpdateRuleDisplay(inputRuleSdescTxt.text);
                }
                else {
                    string ruleID = DbCommands.GenerateUniqueID("VocabGrammar", "RuleIDs", "RuleID");
                    DbCommands.InsertTupleToTable("VocabGrammar", ruleID, inputRuleSdescTxt.text, inputRuleLdescTxt.text);
                    FillDisplayFromDb(DbQueries.GetGrammarRuleDisplayQry(), grammarList.transform, BuildRule);
                }
            }

        }

        public Transform BuildRule(string[] strArray) {
            string ruleID = strArray[0];
            string descriptionStr = strArray[1];
            int translationRule = int.Parse(strArray[2]);
            grammarRule = Instantiate(grammarRulePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            grammarRule.transform.Find("DescriptionInput").GetComponent<InputField>().text = descriptionStr;
            if (IsItemSelectedInGroup(TranslationSelected)) {
                if (translationRule == 1) {
                    grammarRule.GetComponent<GrammarRule>().ActivateAddRule(true);
                }
                else {
                    grammarRule.GetComponent<GrammarRule>().ActivateAddRule(false);
                }
            }
            grammarRule.transform.Find("RuleNumber").GetComponent<Text>().text = ruleID;
            grammarRule.GetComponent<GrammarRule>().CurrentDescription = descriptionStr;
            grammarRule.GetComponent<GrammarRule>().RuleNumber = ruleID;
            //grammarRule.transform.SetParent(ruleList.transform, false);
            return grammarRule.transform;
        }

        public void DeactivateNewGrammarRule() {
            inputRuleSdescTxt.text = inputRuleLdescTxt.text = "";
            viewGrammarDetailsBtnText.text = "New rule";
            editingRule = false;
            newGrammarRulePanel.SetActive(false);
            activateGrammarDetailsBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUIbtn); //indicate to user that button is functioning.

        }

        public void FillRulesNotSelected() {
            FillDisplayFromDb(DbQueries.GetGrammarRuleDisplayQry(), grammarList.transform, BuildRule);
        }

        public void EmptyRulesDisplayExcept(int exception) {
            foreach (Transform item in grammarList.transform) {
                if (int.Parse(item.gameObject.GetComponent<GrammarRule>().RuleNumber) != exception) {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}
