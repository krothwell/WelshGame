using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
//using UnityEditor;
/* Deals with the higher level functions of the Translation Manager such as displaying lists relating to translations from the database
   and a means to enter new translation related data */
public class TranslationUI : MonoBehaviour {
    //high level cross cutting objects and holders
	GameObject sidePanel;
    UI ui;
    Color btnColor;
    Color txtColor = new Color(0.13f, 0.13f, 0.13f);

    //Grammar rules
    GameObject grammarMenu,
               submitNewRule, activateEditNewRuleBtn,
               grammarRule, activeRule;
    public GameObject ruleList,
                      newRulePanel,
                      grammarRulePrefab;
    InputField inputRuleSdescTxt, inputRuleLdescTxt;
    Text grammarRulesLbl, editNewRuleBtnText;
    public bool editingRule = false;

    //proficienies
    GameObject proficienciesMenu, proficiencyThresholds,
               submitNewProficiency, newProficiencyPanel, activateNewProficiencyBtn,
               proficiency;
    public GameObject proficiencyPrefab;
    InputField inputProficiencyTxt, inputThresholdTxt;
    Text proficienciesLbl;

    //translations
    GameObject translations,
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

	void Start () {
        ui = FindObjectOfType<UI>();
		//DbSetup.PrintTable("Translations");
		sidePanel = GameObject.Find("SidePanel");
		grammarRulesLbl = sidePanel.transform.FindChild("GrammarRulesLbl").gameObject.GetComponent<Text>();
		proficienciesLbl = sidePanel.transform.FindChild("ProficienciesLbl").gameObject.GetComponent<Text>();
		searchTranslations = GameObject.Find("SearchTranslations").GetComponent<InputField>();
		grammarMenu = sidePanel.transform.FindChild("GrammarManager").gameObject;
		proficienciesMenu = sidePanel.transform.FindChild("ProficienciesManager").gameObject;
        submitNewTranslation =  GameObject.Find("SubmitNewTranslation"); //doesn't need to bounce off menu since the translation menu remains active;
		submitNewRule =  grammarMenu.transform.FindChild("SubmitNewRule").gameObject;
        submitNewProficiency =  proficienciesMenu.transform.FindChild("SubmitNewProficiency").gameObject;
		newTranslationPanel = submitNewTranslation.transform.Find("NewTranslationPanel").gameObject;
		newRulePanel = submitNewRule.transform.FindChild("NewRulePanel").gameObject;
		newProficiencyPanel = submitNewProficiency.transform.Find("NewProficiencyPanel").gameObject;
		activateNewTranslationBtn = submitNewTranslation.transform.Find("ActivateNewTranslationBtn").gameObject;
		activateEditNewRuleBtn = submitNewRule.transform.FindChild("ActivateNewRuleBtn").gameObject;
		activateNewProficiencyBtn = submitNewProficiency.transform.FindChild("ActivateNewProficiencyBtn").gameObject;
		inputTranslationEnTxt = newTranslationPanel.transform.Find("EnglishVocab").GetComponent<InputField>();
		inputTranslationCyTxt = newTranslationPanel.transform.Find("WelshVocab").GetComponent<InputField>();
		inputRuleSdescTxt = newRulePanel.transform.Find("InputShortDescriptionText").GetComponent<InputField>();
        inputRuleLdescTxt = newRulePanel.transform.Find("InputLongDescriptionText").GetComponent<InputField>();
        inputProficiencyTxt = newProficiencyPanel.transform.Find("InputProficiency").GetComponent<InputField>();
		inputThresholdTxt = newProficiencyPanel.transform.Find("ThresholdInput").GetComponent<InputField>();
		btnColor = activateNewProficiencyBtn.GetComponent<Text>().color;
		translations = GameObject.Find("VocabTranslations");
		proficiencyThresholds = proficienciesMenu.transform.FindChild("ProficiencyThresholds").gameObject;
		ruleList = grammarMenu.transform.FindChild("RuleList").gameObject;
		ui.FillDisplayFromDb(DbSetup.GetTranslationsDisplayQry(), translations.transform, BuildTranslation);
        editNewRuleBtnText = activateEditNewRuleBtn.GetComponent<Text>();
    }

	void Update () {
		if (searchCountDown) {
			searchWait -= Time.deltaTime;
			if (searchWait <= 0) {
				SearchTranslationNow();
				searchCountDown = false;
			}
		}
	}

	public void ActivateGrammarRulesMenu (string englishTxt = null, string welshTxt = null) {
		if (!grammarMenu.activeSelf) { 
			grammarMenu.SetActive(true);
			proficienciesMenu.SetActive(false);
			proficienciesLbl.color = grammarRulesLbl.color;
			grammarRulesLbl.color = btnColor;
			ui.FillDisplayFromDb(DbSetup.GetGrammarRuleDisplayQry(DbSetup.GetParameterNameFromValue(englishTxt), DbSetup.GetParameterNameFromValue(welshTxt)), 
						ruleList.transform, 
						BuildRule, 
						null, 
						englishTxt,
						welshTxt);
		} else {
			ui.FillDisplayFromDb(DbSetup.GetGrammarRuleDisplayQry(DbSetup.GetParameterNameFromValue(englishTxt), DbSetup.GetParameterNameFromValue(welshTxt)), 
						ruleList.transform, 
						BuildRule, 
						null, 
						englishTxt,
						welshTxt);
		}
    }

	public void ActivateGrammarRulesBtnClick () {
		ActivateGrammarRulesMenu ();
    }

    public void SetActiveRule(GameObject rule) {
        activeRule = rule;
    }

    public void SetActiveRuleEdit() {
        editNewRuleBtnText.text = "Edit rule";
        editingRule = true;
        ActivateNewRule();
    }


    public void ActivateProficiencyMenu () {
		if (!proficienciesMenu.activeSelf) {
			proficienciesMenu.SetActive(true);
			grammarMenu.SetActive(false);
			grammarRulesLbl.color = proficienciesLbl.color;
			proficienciesLbl.color = btnColor;
			ui.FillDisplayFromDb(DbSetup.GetProficienciesDisplayQry(), proficiencyThresholds.transform, BuildProficiency);
		}
    }

    //Display a panel to add or edit a new grammar rule 
    public void ActivateNewRule() {
        newRulePanel.SetActive(true);
        activateEditNewRuleBtn.GetComponent<Text>().color = txtColor; //indicate to user that button no longer functions.
        if (editingRule) {
            string[] ruleDesc = DbSetup.GetTupleFromTable("VocabGrammar", 
                "RuleIDs = " + activeRule.GetComponent<GrammarRule>().RuleNumber + ";");
            inputRuleSdescTxt.text = ruleDesc[1];
            inputRuleLdescTxt.text = ruleDesc[2];
        }
    }

    //Display a panel to add a new translation 
    public void ActivateNewTranslation () {
		newTranslationPanel.SetActive(true);
		activateNewTranslationBtn.GetComponent<Text>().color = txtColor; //indicate to user that button no longer functions.
	}

	//Display a panel to add a new proficiency 
	public void ActivateNewProficiency () {
		newProficiencyPanel.SetActive(true);
		activateNewProficiencyBtn.GetComponent<Text>().color = txtColor; //indicate to user that button no longer functions.
	}

	public void DeactivateNewTranslation () {
		newTranslationPanel.SetActive(false);
		activateNewTranslationBtn.GetComponent<Text>().color = btnColor; //indicate to user that button is functioning.
	}

	public void DeactivateNewGrammarRule () {
        inputRuleSdescTxt.text = inputRuleLdescTxt.text = "";
        editNewRuleBtnText.text = "New rule";
        editingRule = false;
        newRulePanel.SetActive(false);
		activateEditNewRuleBtn.GetComponent<Text>().color = btnColor; //indicate to user that button is functioning.

    }

	public void DeactivateNewProficiency () {
		newProficiencyPanel.SetActive(false);
		activateNewProficiencyBtn.GetComponent<Text>().color = btnColor; //indicate to user that button is functioning.
	}

	public void InsertTranslation() {
		DbSetup.InsertTupleToTable("EnglishVocab", inputTranslationEnTxt.text);
		DbSetup.InsertTupleToTable("WelshVocab", inputTranslationCyTxt.text);
		DbSetup.InsertTupleToTable("VocabTranslations", inputTranslationEnTxt.text, inputTranslationCyTxt.text);
		ui.FillDisplayFromDb(DbSetup.GetTranslationsDisplayQry(), translations.transform, BuildTranslation);
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
                DbSetup.UpdateTableTuple("VocabGrammar", "RuleIDs = " + activeRule.GetComponent<GrammarRule>().RuleNumber,fieldVals);
                activeRule.GetComponent<GrammarRule>().UpdateRuleDisplay(inputRuleSdescTxt.text);
            }
            else {
                string ruleID = DbSetup.GenerateUniqueID("VocabGrammar", "RuleIDs", "RuleID");
                DbSetup.InsertTupleToTable("VocabGrammar", ruleID, inputRuleSdescTxt.text, inputRuleLdescTxt.text);
                ui.FillDisplayFromDb(DbSetup.GetGrammarRuleDisplayQry(), ruleList.transform, BuildRule);
            }
        }
        
    }

	public void InsertProficiency() {
		if ((inputProficiencyTxt.text != null) && (inputProficiencyTxt.text != "")) {
			DbSetup.InsertTupleToTable("Proficiencies", inputProficiencyTxt.text, inputThresholdTxt.text);
			ui.FillDisplayFromDb(DbSetup.GetProficienciesDisplayQry(), proficiencyThresholds.transform, BuildProficiency);
			inputProficiencyTxt.text = "";
			inputThresholdTxt.text = "";
		}
	}

	public void FillRulesNotSelected() {

		ui.FillDisplayFromDb(DbSetup.GetGrammarRuleDisplayQry(), ruleList.transform, BuildRule);
	}

	private Transform BuildTranslation(IDataReader _dbr) {
		string EnStr = (_dbr["EnglishText"].ToString());
		string CyStr = (_dbr["WelshText"].ToString());
		translation = Instantiate(translationPrefab, new Vector3(0f,0f,0f),Quaternion.identity) as GameObject;
		translation.transform.Find("EnglishVocab").GetComponent<InputField>().text = EnStr;
		translation.transform.Find("WelshVocab").GetComponent<InputField>().text = CyStr;
		translation.GetComponent<Translation>().CurrentEnglish = EnStr;
		translation.GetComponent<Translation>().CurrentWelsh = CyStr;
		//translation.transform.SetParent(translations.transform, false);
		return translation.transform;
	}

	public Transform BuildRule(IDataReader _dbr) {
		string descriptionStr = (_dbr["ShortDescriptions"].ToString());
		string ruleID = (_dbr["RuleIDs"].ToString());
		int translationRule = int.Parse((_dbr["TranslationRules"]).ToString());
		grammarRule = Instantiate(grammarRulePrefab, new Vector3(0f,0f,0f),Quaternion.identity) as GameObject;
		grammarRule.transform.Find("DescriptionInput").GetComponent<InputField>().text = descriptionStr;
		if (TranslationSelected()) {
			if (translationRule == 1) {
				grammarRule.GetComponent<GrammarRule>().ActivateAddRule(true);
			} else {
				grammarRule.GetComponent<GrammarRule>().ActivateAddRule(false);
			}
		}
		grammarRule.transform.Find("RuleNumber").GetComponent<Text>().text = ruleID;
		grammarRule.GetComponent<GrammarRule>().CurrentDescription = descriptionStr;
		grammarRule.GetComponent<GrammarRule>().RuleNumber = ruleID;
		//grammarRule.transform.SetParent(ruleList.transform, false);
		return grammarRule.transform;
	}

	private Transform BuildProficiency(IDataReader _dbr) {
		string proficiencyStr = (_dbr["ProficiencyNames"].ToString());
		string thresholdStr = (_dbr["Thresholds"].ToString());
		proficiency = Instantiate(proficiencyPrefab, new Vector3(0f,0f,0f),Quaternion.identity) as GameObject;
		proficiency.transform.Find("ProficiencyText").GetComponent<InputField>().text = proficiencyStr;
		proficiency.transform.Find("ThresholdInput").GetComponent<InputField>().text = thresholdStr;
		proficiency.GetComponent<Proficiency>().CurrentProficiencyName = proficiencyStr;
		proficiency.GetComponent<Proficiency>().CurrentThreshold = int.Parse(thresholdStr);
		//proficiency.transform.SetParent(proficiencyThresholds.transform, false);
		return proficiency.transform;
	}

	private int GetDefaultProficiencyThreshold() {
		int defaultProficiencyThreshold = ( GameObject.FindObjectsOfType<Proficiency>().Length - 1 ) * 10;
		return defaultProficiencyThreshold;

	}

	public void EmptyRulesDisplayExcept(int exception) {
		foreach (Transform item in ruleList.transform) {
			if (int.Parse(item.gameObject.GetComponent<GrammarRule>().RuleNumber) != exception) {
				Destroy(item.gameObject);
			}
		}
	}

	public void SearchTranslations () {
		searchWait = 0.5f;
		searchCountDown = true;
	}

	private void SearchTranslationNow() {
		string searchText = searchTranslations.text;
		string sqlqry = DbSetup.GetTranslationSearchQry();
		ui.FillDisplayFromDb(sqlqry, translations.transform, BuildTranslation, searchText);
	}

	public void SetTranslationSelectedProperties(int selectionInt, string englishTxt=null, string welshTxt=null) {
		translationSelectInt += selectionInt;
		if (englishTxt!=null) {
			selectedEnglishText = englishTxt;
		}
		if (welshTxt!=null) {
			selectedWelshText = welshTxt;
		}
	}

	public bool TranslationSelected() {
		return translationSelectInt > 0 ? true : false;
	}

	public string GetSelectedWelshText() {
		if (TranslationSelected()) {
			return selectedWelshText;
		} else {
			return null;
		}
	}

	public string GetSelectedEnglishText() {
		if (TranslationSelected()) {
			return selectedEnglishText;
		} else {
			return null;
		}
	}

	public void DeactivateSelf() {
		gameObject.SetActive(false);
	}

	public void ActivateSelf() {
		gameObject.SetActive(true);
	}

}
