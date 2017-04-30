using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;

public class SkillsMenuUI : UIController {
    WelshSkillsListUI welshSkillsListUI;
    int totalSkillPoints, skillPointsSpent;

    void Start() {
        totalSkillPoints = -1;
        welshSkillsListUI = GetPanel().transform.FindChild("WelshSkillsListUI").GetComponent<WelshSkillsListUI>();
        print(welshSkillsListUI);
        totalSkillPoints = GetTotalSkillPoints();
    }
    public new void DisplayComponents() {
        base.DisplayComponents();
        welshSkillsListUI.DisplayWelshSkills();
    }

    public int GetTotalSkillPoints() {
        if (totalSkillPoints == -1) {
            int writeSkillPoints = DbCommands.GetCountFromTable("AcquiredVocabWriteSkills", "SaveIDs = 0");
            int readSkillPoints = DbCommands.GetCountFromTable("AcquiredVocabReadSkills", "SaveIDs = 0");
            int grammarSkillPoints = DbCommands.GetCountFromTable("AcquiredGrammarSkills", "SaveIDs = 0");
            int total = writeSkillPoints + readSkillPoints + grammarSkillPoints;
            return total;
        } else {
            return totalSkillPoints;
        }
    }

    public int GetSkillPointsSpent() {
        return skillPointsSpent;
    }

    public void SetSkillPointsSpent(int skillPoints) {
        skillPointsSpent = skillPoints;
    }
}
