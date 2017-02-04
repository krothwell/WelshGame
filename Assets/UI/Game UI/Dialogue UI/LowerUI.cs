using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using GameUI;
using GameUI.ListItems;
using DbUtilities;

public class LowerUI : MonoBehaviour {
    bool submissionScored = false;
	private string testEnglish;
	private string testWelsh;
	public string TestEnglish {
		get {return testEnglish;}
		set {testEnglish = value;}
	}
	public string TestWelsh {
		get {return testWelsh;}
		set {testWelsh = value;}
	}
	Text lowerUItxt;
	Text answerTxt;
	Text percentageTxt;
    public GameObject components;
    GameObject answerField;
    InputField answerInput;
    GameObject submitBtn;
    CombatUI combatUI;
    PlayerController mainCharacter;
    Text btnTxt;
    Animator animator;
    DialogueUI dialogueUI;
    Image objPortrait;

    void Start () {
        components = transform.FindChild("Components").gameObject;
        lowerUItxt = components.transform.FindChild("Text").GetComponent<Text>();
        answerField = components.transform.FindChild("InputField").gameObject;
        answerInput = answerField.GetComponent<InputField>();
        answerTxt = answerInput.transform.FindChild("Text").GetComponent<Text>();
        submitBtn =  components.transform.FindChild("SubmitBtn").gameObject;
        percentageTxt = components.transform.FindChild("PercentageTxt").GetComponent<Text>();
        combatUI = FindObjectOfType<CombatUI>();
        btnTxt = submitBtn.transform.FindChild("Text").gameObject.GetComponent<Text>();
        mainCharacter = FindObjectOfType<PlayerController>();
        objPortrait = components.transform.FindChild("CharacterPortrait").GetComponent<Image>();
    }

    void Update() {

    }

	public void SetRandomVocab() {
		string[] testStrings = DbCommands.GetRandomTupleFromTable("VocabTranslations");
		testEnglish = testStrings[0];
		testWelsh = testStrings[1];
		lowerUItxt.text = "Translate the following into Welsh: " + testEnglish;
	}

	public void SetPercentageCorrect() {
		int welshLength = testWelsh.Length;
		int percentage = 0;
		int countCorrect = 0;
		string answer = answerTxt.text;
		for(int i = 0; i < welshLength; i++) {
			if (i < answer.Length) {
				if (answer[i] == testWelsh[i]) {
					countCorrect++;
				} 
			} else { break; }
		}

		percentage = (int)Mathf.Round((100f/welshLength) * countCorrect);
		Debug.Log(countCorrect);
		percentageTxt.text = percentage.ToString() + "%";

	}

    public void ActivateAnswerField() {
        //answerField.SetActive(true);
        //answerInput.Select();
    }

    public void SetInUse() {
        animator = GetComponent<Animator>();
        animator.SetBool("InUse", true);
    }


    public void SetNotInUse() {
        animator.SetBool("InUse", false);
        mainCharacter.DestroySelectionCircleOfInteractiveObject();
    }

    public void SetBtnText(string newText) {
        btnTxt.text = newText;
    }

    public void ManageLowerUISubmission() {
        if (submissionScored) {
            if (combatUI.currentAbility == (CombatUI.CombatAbilities.strike)) { 
                mainCharacter.StrikeSelectedEnemy();
                combatUI.ToggleCombatMode();
                submissionScored = false;
                SetBtnText("Submit answer");
                SetNotInUse();
            }
        } else {
            SetPercentageCorrect();
            submissionScored = true;
            SetBtnText("Finish move");
        }
    }

    public void ProcessCharacterDialogue(Character character) {
        dialogueUI = GetComponent<DialogueUI>();
        dialogueUI.StartNewDialogue(character);
    }

    public void SetObjectPortrait(Sprite portrait) {
        objPortrait.sprite = portrait;
    }

}
