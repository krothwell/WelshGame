using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using DataUI.ListItems;
using DbUtilities;
using DataUI.Utilities;

namespace DataUI {
    /* Deals with the higher level functions of the Translation Manager such as displaying lists relating to translations from the database
       and a means to enter new translation related data */
    public class TranslationUI : DataUIController {
        //high level cross cutting objects and holders
        GameObject panel;

        //Grammar rules
        GameObject grammarListUI, grammarListUIpanel, grammarList,
                   submitNewGrammarRule, activateGrammarDetailsBtn,
                   grammarRule, activeRule;
        public GameObject newGrammarRulePanel,
                          grammarRulePrefab;
        InputField inputRuleSdescTxt, inputRuleLdescTxt;
        Button grammarRulesBtn;
        Text viewGrammarDetailsBtnText;
        public bool editingRule = false;

        //proficienies
        GameObject proficienciesListUI, proficienciesListUIpanel, proficienciesList,
                   submitNewProficiency, newProficiencyPanel, activateNewProficiencyBtn,
                   proficiency;
        public GameObject proficiencyPrefab;
        InputField inputProficiencyTxt, inputThresholdTxt;
        Button proficienciesBtn;

        //translations
        GameObject translationListsUI, translationListsUIpanel, vocabTranslationList,
                   submitNewTranslation, newTranslationPanel, activateNewTranslationBtn,
                   translation;
        public GameObject translationPrefab;
        InputField inputTranslationEnTxt, inputTranslationCyTxt;
        InputField searchTranslations;
        string selectedEnglishText;
        string selectedWelshText;
        float searchWait = 0f;
        bool searchCountDown = false;
        int translationSelectInt = 0;
        string auxiliaryDataMenusGroup;

        void Start() {
            panel = transform.FindChild("Panel").gameObject;

            //GRAMMAR UI
            grammarListUI = panel.transform.FindChild("GrammarListUI").gameObject;
            grammarRulesBtn = grammarListUI.transform.FindChild("GrammarRulesBtn").gameObject.GetComponent<Button>();
            grammarListUIpanel = grammarListUI.transform.FindChild("Panel").gameObject;
            grammarList = grammarListUIpanel.GetComponentInChildren<GridLayoutGroup>().gameObject;
            //adding and editing
            submitNewGrammarRule = grammarListUIpanel.transform.FindChild("SubmitNewRule").gameObject;
            activateGrammarDetailsBtn = submitNewGrammarRule.transform.FindChild("ActivateNewRuleBtn").gameObject;
            newGrammarRulePanel = submitNewGrammarRule.transform.FindChild("NewRulePanel").gameObject;
            inputRuleSdescTxt = newGrammarRulePanel.transform.Find("InputShortDescriptionText").GetComponent<InputField>();
            inputRuleLdescTxt = newGrammarRulePanel.transform.Find("InputLongDescriptionText").GetComponent<InputField>();
            viewGrammarDetailsBtnText = activateGrammarDetailsBtn.GetComponent<Text>();

            //PROFICIENCIES UI
            proficienciesListUI = panel.transform.FindChild("ProficienciesListUI").gameObject;
            proficienciesBtn = proficienciesListUI.transform.FindChild("ProficienciesBtn").gameObject.GetComponent<Button>();
            proficienciesListUIpanel = proficienciesListUI.transform.FindChild("Panel").gameObject;
            proficienciesList = proficienciesListUIpanel.GetComponentInChildren<GridLayoutGroup>().gameObject;
            //adding
            submitNewProficiency = proficienciesListUIpanel.transform.FindChild("SubmitNewProficiency").gameObject;
            newProficiencyPanel = submitNewProficiency.transform.Find("NewProficiencyPanel").gameObject;
            activateNewProficiencyBtn = submitNewProficiency.transform.FindChild("ActivateNewProficiencyBtn").gameObject;
            inputProficiencyTxt = newProficiencyPanel.transform.Find("InputProficiency").GetComponent<InputField>();
            inputThresholdTxt = newProficiencyPanel.transform.Find("ThresholdInput").GetComponent<InputField>();

            //TRANSLATIONS UI
            translationListsUI = panel.transform.FindChild("TranslationListsUI").gameObject;
            translationListsUIpanel = translationListsUI.transform.FindChild("Panel").gameObject;
            vocabTranslationList = translationListsUIpanel.GetComponentInChildren<GridLayoutGroup>().gameObject;
            //adding
            submitNewTranslation = translationListsUIpanel.transform.FindChild("SubmitNewTranslation").gameObject;
            activateNewTranslationBtn = submitNewTranslation.transform.Find("ActivateNewTranslationBtn").gameObject;
            newTranslationPanel = submitNewTranslation.transform.Find("NewTranslationPanel").gameObject;
            inputTranslationEnTxt = newTranslationPanel.transform.Find("EnglishVocab").GetComponent<InputField>();
            inputTranslationCyTxt = newTranslationPanel.transform.Find("WelshVocab").GetComponent<InputField>();
            //searching
            searchTranslations = translationListsUIpanel.transform.FindChild("SearchTranslations").GetComponent<InputField>();

            auxiliaryDataMenusGroup = "AuxiliaryDataMenus";
            CreateNewMenuToggleGroup(auxiliaryDataMenusGroup);
            AddNewMenuToToggleGroup(auxiliaryDataMenusGroup, grammarListUI.GetComponent<UIController>());
            AddNewMenuToToggleGroup(auxiliaryDataMenusGroup, proficienciesListUI.GetComponent<UIController>());

            FillDisplayFromDb(DbCommands.GetTranslationsDisplayQry(), vocabTranslationList.transform, BuildVocabTranslation);
        }

        void Update() {
            if (searchCountDown) {
                searchWait -= Time.deltaTime;
                if (searchWait <= 0) {
                    SearchTranslationNow();
                    searchCountDown = false;
                }
            }
        }

        public void ActivateGrammarRulesMenu(string englishTxt = null, string welshTxt = null) {
            if (!grammarListUIpanel.activeSelf) {
                ToggleMenuTo(grammarListUI.GetComponent<UIController>(), auxiliaryDataMenusGroup);
                proficienciesBtn.colors.normalColor.Equals(Colours.colorDataUItxt);
                grammarRulesBtn.colors.normalColor.Equals(Colours.colorDataUIbtn);
                FillDisplayFromDb(DBqueries.GetGrammarRuleDisplayQry(englishTxt,welshTxt),
                            grammarList.transform,
                            BuildRule,
                            englishTxt,
                            welshTxt);
            }
            else {
                print("Activate grammar rules");
                FillDisplayFromDb(DBqueries.GetGrammarRuleDisplayQry(englishTxt, welshTxt),
                            grammarList.transform,
                            BuildRule,
                            englishTxt,
                            welshTxt);
            }
        }

        public void ActivateGrammarRulesBtnClick() {
            ActivateGrammarRulesMenu();
        }

        public void SetActiveRule(GameObject rule) {
            activeRule = rule;
        }

        public void SetActiveRuleEdit() {
            viewGrammarDetailsBtnText.text = "Edit rule";
            editingRule = true;
            ActivateNewRule();
        }


        public void ActivateProficiencyMenu() {
            if (!proficienciesListUIpanel.activeSelf) {
                ToggleMenuTo(proficienciesListUI.GetComponent<UIController>(), auxiliaryDataMenusGroup);
                grammarRulesBtn.colors.normalColor.Equals(Colours.colorDataUItxt);
                proficienciesBtn.colors.normalColor.Equals(Colours.colorDataUIbtn);
                FillDisplayFromDb(DbCommands.GetProficienciesDisplayQry(), proficienciesList.transform, BuildProficiency);
            }
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

        //Display a panel to add a new translation 
        public void ActivateNewTranslation() {
            newTranslationPanel.SetActive(true);
            activateNewTranslationBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUItxt); //indicate to user that button no longer functions.
        }

        //Display a panel to add a new proficiency 
        public void ActivateNewProficiency() {
            newProficiencyPanel.SetActive(true);
            activateNewProficiencyBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUItxt); //indicate to user that button no longer functions.
        }

        public void DeactivateNewTranslation() {
            newTranslationPanel.SetActive(false);
            activateNewTranslationBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUIbtn); //indicate to user that button is functioning.
        }

        public void DeactivateNewGrammarRule() {
            inputRuleSdescTxt.text = inputRuleLdescTxt.text = "";
            viewGrammarDetailsBtnText.text = "New rule";
            editingRule = false;
            newGrammarRulePanel.SetActive(false);
            activateGrammarDetailsBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUIbtn); //indicate to user that button is functioning.

        }

        public void DeactivateNewProficiency() {
            newProficiencyPanel.SetActive(false);
            activateNewProficiencyBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUIbtn); //indicate to user that button is functioning.
        }

        public void InsertTranslation() {
            DbCommands.InsertTupleToTable("EnglishVocab", inputTranslationEnTxt.text);
            DbCommands.InsertTupleToTable("WelshVocab", inputTranslationCyTxt.text);
            DbCommands.InsertTupleToTable("VocabTranslations", inputTranslationEnTxt.text, inputTranslationCyTxt.text);
            FillDisplayFromDb(DbCommands.GetTranslationsDisplayQry(), vocabTranslationList.transform, BuildVocabTranslation);
            inputTranslationEnTxt.text = "";
            inputTranslationCyTxt.text = "";
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
                    FillDisplayFromDb(DBqueries.GetGrammarRuleDisplayQry(), grammarList.transform, BuildRule);
                }
            }

        }

        public void InsertProficiency() {
            if ((inputProficiencyTxt.text != null) && (inputProficiencyTxt.text != "")) {
                DbCommands.InsertTupleToTable("Proficiencies", inputProficiencyTxt.text, inputThresholdTxt.text);
                FillDisplayFromDb(DbCommands.GetProficienciesDisplayQry(), proficienciesList.transform, BuildProficiency);
                inputProficiencyTxt.text = "";
                inputThresholdTxt.text = "";
            }
        }

        public void FillRulesNotSelected() {
            FillDisplayFromDb(DBqueries.GetGrammarRuleDisplayQry(), grammarList.transform, BuildRule);
        }

        private Transform BuildVocabTranslation(string[] strArray) {
            string EnStr = strArray[0];
            string CyStr = strArray[1];
            translation = Instantiate(translationPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            translation.transform.Find("EnglishVocab").GetComponent<InputField>().text = EnStr;
            translation.transform.Find("WelshVocab").GetComponent<InputField>().text = CyStr;
            translation.GetComponent<Translation>().CurrentEnglish = EnStr;
            translation.GetComponent<Translation>().CurrentWelsh = CyStr;
            //translation.transform.SetParent(translations.transform, false);
            return translation.transform;
        }

        public Transform BuildRule(string[] strArray) {
            string ruleID = strArray[0];
            string descriptionStr = strArray[1];
            int translationRule = int.Parse(strArray[2]);
            grammarRule = Instantiate(grammarRulePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            grammarRule.transform.Find("DescriptionInput").GetComponent<InputField>().text = descriptionStr;
            if (TranslationSelected()) {
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

        private Transform BuildProficiency(string[] strArray) {
            string proficiencyStr = (strArray[0]);
            string thresholdStr = (strArray[1]);
            proficiency = Instantiate(proficiencyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            proficiency.transform.Find("ProficiencyText").GetComponent<InputField>().text = proficiencyStr;
            proficiency.transform.Find("ThresholdInput").GetComponent<InputField>().text = thresholdStr;
            proficiency.GetComponent<Proficiency>().CurrentProficiencyName = proficiencyStr;
            proficiency.GetComponent<Proficiency>().CurrentThreshold = int.Parse(thresholdStr);
            //proficiency.transform.SetParent(proficiencyThresholds.transform, false);
            return proficiency.transform;
        }

        private int GetDefaultProficiencyThreshold() {
            int defaultProficiencyThreshold = (GameObject.FindObjectsOfType<Proficiency>().Length - 1) * 10;
            return defaultProficiencyThreshold;

        }

        public void EmptyRulesDisplayExcept(int exception) {
            foreach (Transform item in grammarList.transform) {
                if (int.Parse(item.gameObject.GetComponent<GrammarRule>().RuleNumber) != exception) {
                    Destroy(item.gameObject);
                }
            }
        }

        public void SearchTranslations() {
            searchWait = 0.5f;
            searchCountDown = true;
        }

        private void SearchTranslationNow() {
            if (searchTranslations.text == "") {
                FillDisplayFromDb(DbCommands.GetTranslationsDisplayQry(), vocabTranslationList.transform, BuildVocabTranslation);
            } else { 
                string searchText = "%" + searchTranslations.text + "%";
                string sqlqry = DBqueries.GetTranslationSearchQry(searchText);
                FillDisplayFromDb(sqlqry, vocabTranslationList.transform, BuildVocabTranslation, searchText);
            }
        }

        public void SetTranslationSelectedProperties(int selectionInt, string englishTxt = null, string welshTxt = null) {
            translationSelectInt += selectionInt;
            if (englishTxt != null) {
                selectedEnglishText = englishTxt;
            }
            if (welshTxt != null) {
                selectedWelshText = welshTxt;
            }
        }

        public bool TranslationSelected() {
            return translationSelectInt > 0 ? true : false;
        }

        public string GetSelectedWelshText() {
            if (TranslationSelected()) {
                return selectedWelshText;
            }
            else {
                return null;
            }
        }

        public string GetSelectedEnglishText() {
            if (TranslationSelected()) {
                return selectedEnglishText;
            }
            else {
                return null;
            }
        }

    }
}