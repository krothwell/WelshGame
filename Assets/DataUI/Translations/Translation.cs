using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Translation : MonoBehaviour {

	private string currentEnglish;
	public string CurrentEnglish {
		get {return currentEnglish;}
		set {currentEnglish = value;}
	}
	private string currentWelsh;
	public string CurrentWelsh {
		get {return currentWelsh;}
		set {currentWelsh = value;}
	}
	TranslationUI translationUI;
	GameObject translationOptions;
	GameObject englishText, welshText;
	GameObject saveBtn;
	GameObject panel;

	// Use this for initialization
	void Start () {
		translationOptions = gameObject.transform.FindChild("TranslationOptions").gameObject;
		englishText = gameObject.transform.FindChild("EnglishVocab").gameObject;
		welshText = gameObject.transform.FindChild("WelshVocab").gameObject;
		panel = gameObject.transform.FindChild("Panel").gameObject;
		saveBtn = gameObject.transform.FindChild("Save").gameObject;
        translationUI = FindObjectOfType<TranslationUI>();
	}

	void Update() { 
		SetSelectionOfTranslationOnClick();
	}


	public void SetSelectionOfTranslationOnClick() {
		if (Input.GetMouseButtonUp(0)){
			if(SelectController.ClickedDifferentGameObjectTo(this.gameObject)
				&& !SelectController.IsClickedGameObjectName("AddRemoveRule")) {
				if (translationOptions.activeSelf || panel.activeSelf) {
					DeactivateTranslationOptions();
					DisableEdits();
					englishText.GetComponent<InputField>().text = CurrentEnglish;
					welshText.GetComponent<InputField>().text = CurrentWelsh;
					translationUI.SetTranslationSelectedProperties(-1);
					if (!translationUI.TranslationSelected()) {
						translationUI.FillRulesNotSelected();
					}
				}
			}
		}
	}

	public void ActivateTranslationOptions() {
		//proficiencyText.GetComponent<Image>().color = new Color(0.7f,0.85f,1f);
		translationOptions.SetActive(true);
		panel.SetActive(true);
	}

	public void DeactivateTranslationOptions() {
		translationOptions.SetActive(false);
	}

	public void EnableEdits() {
		englishText.GetComponent<InputField>().readOnly = false;
		welshText.GetComponent<InputField>().readOnly = false;
		saveBtn.SetActive(true);
	}

	public void DisableEdits() {
		englishText.GetComponent<InputField>().readOnly = true;
		welshText.GetComponent<InputField>().readOnly = true;
		saveBtn.SetActive(false);
		panel.SetActive(false);
	}

	public void UpdateTranslation() {

		string newEn = englishText.GetComponent<InputField>().text;
		string newCy = welshText.GetComponent<InputField>().text;
		/* We need to check if we are updating a value with more than one entry in translations. If so, a new value needs to be inserted
		to English / Welsh tables to avoid changing all of the translation entry values to the same value. This results in the translation
		needing to be updated rather than the English / Welsh as would otherwise be updated and the update cascaded.*/
		if (DbSetup.GetCountFromTable(	"VocabTranslations", 
										"EnglishText = " + DbSetup.GetParameterNameFromValue(CurrentEnglish),
										CurrentEnglish) 
		> 1) {
			DbSetup.InsertTupleToTable("EnglishVocab", newEn);
			DbSetup.UpdateTableField(	"VocabTranslations",
									 	"EnglishText",
									 	newEn,
										"EnglishText = "  + DbSetup.GetParameterNameFromValue(CurrentEnglish) + 
											" AND " +
										"WelshText = " + DbSetup.GetParameterNameFromValue(CurrentWelsh),
										CurrentEnglish, CurrentWelsh);
		} else {
			print("Updating English tbl");
			DbSetup.UpdateTableField("EnglishVocab",
									 "EnglishText",
									 newEn,
									"EnglishText = " + DbSetup.GetParameterNameFromValue(CurrentEnglish),
									CurrentEnglish);
			print("success");
		}
		if (DbSetup.GetCountFromTable(	"VocabTranslations", 
										"WelshText = " + DbSetup.GetParameterNameFromValue(CurrentWelsh), CurrentWelsh)
		> 1) {
			DbSetup.InsertTupleToTable("Welsh", newCy);
			DbSetup.UpdateTableField("VocabTranslations",
									 "WelshText",
									 newCy,
									"EnglishText = " + DbSetup.GetParameterNameFromValue(newEn) +
										" AND " + //newEn must be used since it will have been update before, rather than simultaneously.
									"WelshText = "  + DbSetup.GetParameterNameFromValue(CurrentWelsh),
									CurrentEnglish, CurrentWelsh);
		} else {
			DbSetup.UpdateTableField("WelshVocab",
									 "WelshText",
									 newCy,
									"WelshText =  " + DbSetup.GetParameterNameFromValue(CurrentWelsh),
									CurrentWelsh);
		}
		CurrentEnglish = newEn;
		CurrentWelsh = newCy;
		translationUI.SetTranslationSelectedProperties(-1);
		translationUI.FillRulesNotSelected();
	}

	/* */
	public void DeleteTranslation() {
		string[,] translationFields = new string[,] {{"EnglishText", CurrentEnglish}, {"WelshText", CurrentWelsh}};
		string[,] englishFields = new string[,] {{"EnglishText", CurrentEnglish}};
		string[,] welshFields = new string[,] {{"WelshText", CurrentWelsh}};
		int translationsWithEnglish = DbSetup.GetCountFromTable("VocabTranslations", 
																"EnglishText = " + DbSetup.GetParameterNameFromValue(CurrentEnglish), 
																CurrentEnglish);
			int translationsWithWelsh = DbSetup.GetCountFromTable("VocabTranslations", 
																 "WelshText = " + DbSetup.GetParameterNameFromValue(CurrentWelsh),
																 CurrentWelsh);

		if (translationsWithEnglish <= 1 && translationsWithWelsh <= 1) {
			DbSetup.DeleteTupleInTable("EnglishVocab", englishFields);
			DbSetup.DeleteTupleInTable("WelshVocab", welshFields);
		} else if (translationsWithEnglish > 1 && translationsWithWelsh <= 1) {
			DbSetup.DeleteTupleInTable("VocabTranslations", translationFields);
			DbSetup.DeleteTupleInTable("WelshVocab", welshFields);
		} else if (translationsWithEnglish <= 1 && translationsWithWelsh > 1) {
			DbSetup.DeleteTupleInTable("VocabTranslations", translationFields);
			DbSetup.DeleteTupleInTable("EnglishVocab", englishFields);
		} else if (translationsWithEnglish > 1 && translationsWithWelsh > 1) {
			DbSetup.DeleteTupleInTable("VocabTranslations", translationFields);
		}
		Destroy(gameObject);
		translationUI.SetTranslationSelectedProperties(-1);
		translationUI.FillRulesNotSelected();
	}

	void OnMouseUpAsButton() {
		if (!translationOptions.activeSelf && !panel.activeSelf) {
			ActivateTranslationOptions();

			translationUI.SetTranslationSelectedProperties(1, CurrentEnglish, CurrentWelsh);

			translationUI.ActivateGrammarRulesMenu(CurrentEnglish, CurrentWelsh);
		}
	} 

}
