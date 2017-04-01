using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WelshVocab : NewWelshVocab {
    Slider progressSlider;
    Text progressLbl;
    WelshSkillsListUI welshSkillListUI;
    public void InitialiseMe(string engVocab, string cymVocab, string correctTally) {
        base.InitialiseMe(engVocab, cymVocab);
        welshSkillListUI = FindObjectOfType<WelshSkillsListUI>();
        progressSlider = GetComponentInChildren<Slider>();
        progressLbl = transform.FindChild("ExtraInformation").FindChild("Progresslbl").GetComponent<Text>();
        int tallyInt = int.Parse(correctTally);
        progressSlider.value = tallyInt;
        progressLbl.text = welshSkillListUI.GetProficiencyString(tallyInt);
    }
}
