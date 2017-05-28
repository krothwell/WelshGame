using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WelshGrammar : NewWelshGrammar {
    Slider progressSlider;
    Text progressLbl;
    Transform extraInfo;
    WelshSkillsListUI welshSkillListUI;
    public void InitialiseMe(string grammarID, string shortDesc, string longDesc, string tally) {
        base.InitialiseMe(grammarID, shortDesc, longDesc);
        extraInfo = transform.Find("ExtraInformation");
        welshSkillListUI = FindObjectOfType<WelshSkillsListUI>();
        progressSlider = extraInfo.Find("ProgressSlider").GetComponent<Slider>();
        progressLbl = extraInfo.Find("ProgressLbl").GetComponent<Text>();
        int tallyInt = int.Parse(tally);
        progressSlider.value = tallyInt;
        int threshold;
        string proficiencyLbl;
        GrammarProficienciesHandler proficiencyHandler = new GrammarProficienciesHandler(grammarID);
        proficiencyHandler.GetProficiencyDetailsFromTally(tallyInt, out proficiencyLbl, out threshold);
        progressLbl.text = proficiencyLbl;
        progressSlider.maxValue = threshold;
    }
}
