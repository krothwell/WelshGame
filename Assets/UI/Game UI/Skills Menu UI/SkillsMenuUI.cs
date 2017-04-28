using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsMenuUI : UIController {
    WelshSkillsListUI welshSkillsListUI;
    int skillPointsSpent;

    void Start() {
        welshSkillsListUI = GetPanel().transform.FindChild("WelshSkillsListUI").GetComponent<WelshSkillsListUI>();
        print(welshSkillsListUI);
    }
    public new void DisplayComponents() {
        base.DisplayComponents();
        welshSkillsListUI.DisplayWelshSkills();
    }

    public int GetSkillPointsSpent() {
        return skillPointsSpent;
    }

    public void SetSkillPointsSpent(int skillPoints) {
        skillPointsSpent = skillPoints;
    }
}
