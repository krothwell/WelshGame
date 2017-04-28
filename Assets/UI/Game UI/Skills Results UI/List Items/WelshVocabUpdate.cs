using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.ListItems {
    public class WelshVocabUpdate : NewWelshVocab {
        Slider progressSlider;
        Text progressLbl, tallyModifier, skillsIncrementer;
        Transform extraInfo;
        WelshSkillsListUI welshSkillListUI;
        public void InitialiseMe(string english, string welsh, string tally, int tallyMod, bool skillInc, DialogueTestDataController.TestType testType) {
            base.InitialiseMe(english, welsh);
            welshSkillListUI = FindObjectOfType<WelshSkillsListUI>();
            print("WELSH SKILLS LIST UI: " + welshSkillListUI);
            extraInfo = transform.FindChild("ExtraInformation");
            progressSlider = extraInfo.FindChild("ProgressSlider").GetComponent<Slider>();
            progressLbl = extraInfo.FindChild("ProgressLbl").GetComponent<Text>();
            tallyModifier = extraInfo.FindChild("TallyModifier").GetComponent<Text>();
            skillsIncrementer = extraInfo.FindChild("SkillPointIncrementer").GetComponent<Text>();
            int tallyInt = int.Parse(tally);
            int threshold;
            string proficiencyString;
            SetProficiencyDetailsFromTally(tallyInt, english, welsh, out proficiencyString, out threshold, testType);
            progressSlider.maxValue = threshold;
            progressSlider.value = tallyInt;
            progressLbl.text = proficiencyString;
            SetTallyModifier(tallyMod);
            SetSkillsIncrementer(skillInc);
        }

        public void SetTallyModifier(int tallyMod) {
            string tallyModString = "";
            if (tallyMod > 0) {
                tallyModString = "+";
            }
            else if (tallyMod < 0) {
                tallyModString = "-";
            }
            tallyModifier.text = tallyModString;
        }

        public void SetSkillsIncrementer(bool skillInc) {
            string skillPointsIncString = "";
            if (skillInc) {
                skillPointsIncString = "+";
            }
            skillsIncrementer.text = skillPointsIncString;
        }

        private void SetProficiencyDetailsFromTally(int tally, string eng, string cym, out string proficiencyName, out int threshold, DialogueTestDataController.TestType testType) {
            proficiencyName = "";
            threshold = 0;
            if (testType == DialogueTestDataController.TestType.read) {
                ReadProficienciesHandler readProficiencyHandler = new ReadProficienciesHandler(eng, cym);
                readProficiencyHandler.GetProficiencyDetailsFromTally(tally, out proficiencyName, out threshold);
            } else if (testType == DialogueTestDataController.TestType.write) {
                WriteProficienciesHandler writeProficiencyHandler = new WriteProficienciesHandler(eng, cym);
                writeProficiencyHandler.GetProficiencyDetailsFromTally(tally, out proficiencyName, out threshold);
            }

        }
    }
}