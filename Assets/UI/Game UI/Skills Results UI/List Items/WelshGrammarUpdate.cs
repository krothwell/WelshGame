using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameUI.Utilities;


namespace GameUI.ListItems {
    public class WelshGrammarUpdate : NewWelshGrammar {
        Slider progressSlider;
        Text progressLbl, tallyModifier, skillsIncrementer;
        Transform extraInfo;
        //WelshSkillsListUI welshSkillListUI;
        public void InitialiseMe(string grammarID, string grammarSummaryStr, string grammarDetailsStr, string tally, string tallyMod, string skillInc) {
            base.InitialiseMe(grammarID, grammarSummaryStr, grammarDetailsStr);
            //welshSkillListUI = FindObjectOfType<WelshSkillsListUI>();
            extraInfo = transform.Find("ExtraInformation");
            progressSlider = extraInfo.Find("ProgressSlider").GetComponent<Slider>();
            progressLbl = extraInfo.Find("ProgressLbl").GetComponent<Text>();
            tallyModifier = extraInfo.Find("TallyModifier").GetComponent<Text>();
            skillsIncrementer = extraInfo.Find("SkillPointIncrementer").GetComponent<Text>();
            int tallyInt = int.Parse(tally);
            int threshold;
            string proficiencyString;
            GrammarProficienciesHandler proficienciesHandler = new GrammarProficienciesHandler(grammarID);
            proficienciesHandler.GetProficiencyDetailsFromTally(tallyInt, out proficiencyString, out threshold);
            progressSlider.maxValue = threshold;
            progressSlider.value = tallyInt;
            progressLbl.text = proficiencyString;
            SetTallyModifier(tallyMod);
            SetSkillsIncrementer(skillInc);
        }

        public void SetTallyModifier(string tallyMod) {
            string tallyModString = "";
            int tallyModInt = int.Parse(tallyMod);
            if (tallyModInt > 0) {
                tallyModString = "+";
                tallyModifier.color = Colours.tallyPlus;
            }
            else if (tallyModInt < 0) {
                tallyModString = "-";
                tallyModifier.color = Colours.tallyMinus;
            }
            tallyModifier.text = tallyModString;
        }

        public void SetSkillsIncrementer(string skillInc) {
            skillsIncrementer.text = skillInc;
        }

    }
}
