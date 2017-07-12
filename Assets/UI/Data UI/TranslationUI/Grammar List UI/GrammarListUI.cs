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
        VocabTranslationListUI vocabTranslationListUI;
        ProficienciesListUI proficienciesListUI;
        private Button grammarRulesBtn;
        public Button GrammarRulesBtn {
            get { return grammarRulesBtn; }
            set { grammarRulesBtn = value; }
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

        private string selectedGrammarRule;
        public string SelectedGrammarRule {
            get { return selectedGrammarRule; }
            set { selectedGrammarRule = value; }
        }

        void Start() {
            vocabTranslationListUI = FindObjectOfType<VocabTranslationListUI>();
            grammarRulesBtn = transform.Find("GrammarRulesBtn").gameObject.GetComponent<Button>();

            proficienciesListUI = FindObjectOfType<ProficienciesListUI>();
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
            selectedGrammarRule = "SelectedGrammarRule";
            CreateSelectionToggleGroup(selectedGrammarRule);
        }

        public void ToggleSideMenuToGrammarList() {
            if (!GetPanel().activeSelf) {
                print("panelactive");
                translationUI.ToggleMenuTo(GetComponent<UIController>(), translationUI.SideMenuGroup);
                proficienciesListUI.ProficienciesBtn.colors.normalColor.Equals(Colours.colorDataUIbtn);
                grammarRulesBtn.colors.normalColor.Equals(Colours.colorDataUItxt);
            }
            Translation translation = (Translation)(vocabTranslationListUI.GetSelectedItemFromGroup(vocabTranslationListUI.VocabTranslationSelected));
            string eng, cym;
            if (translation == null) {
                eng = cym = null;
            } else {
                eng = translation.CurrentEnglish; cym = translation.CurrentWelsh;
            }
            FillDisplayFromDb(DbQueries.GetGrammarRuleDisplayQry(eng, cym),
                grammarList.transform,
                BuildRule,
                eng,
                cym
            );
        }

        public void SetActiveRuleEdit() {
            viewGrammarDetailsBtnText.text = "Edit rule";
            editingRule = true;
            ActivateNewRule();
        }

        public void ClearTagging() {
            foreach(Transform grt in grammarList.transform) {
                GrammarRule gr = grt.GetComponent<GrammarRule>();
                gr.DeactivateTag();
            }
        }

        //Display a panel to add or edit a new grammar rule 
        public void ActivateNewRule() {
            newGrammarRulePanel.SetActive(true);
            activateGrammarDetailsBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUItxt); //indicate to user that button no longer functions.
            if (editingRule) {
                GrammarRule selectedRule = (GrammarRule)(GetSelectedItemFromGroup(selectedGrammarRule));
                string[] ruleDesc = DbCommands.GetTupleFromTable("VocabGrammar",
                    "RuleIDs = " + selectedRule.RuleNumber);
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
                    GrammarRule selectedRule = (GrammarRule)(GetSelectedItemFromGroup(selectedGrammarRule));
                    DbCommands.UpdateTableTuple("VocabGrammar", "RuleIDs = " + selectedRule.RuleNumber, fieldVals);
                    selectedRule.UpdateRuleDisplay(inputRuleSdescTxt.text);
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
            if (vocabTranslationListUI.IsItemSelectedInGroup(vocabTranslationListUI.VocabTranslationSelected)) {
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
