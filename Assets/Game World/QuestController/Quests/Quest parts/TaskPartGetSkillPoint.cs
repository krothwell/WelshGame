using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPartGetSkillPoint : QuestTaskPart {
    private int initialSkillPoints;
    SkillsMenuUI skillsMenuUI;
    void Start() {
        skillsMenuUI = FindObjectOfType<SkillsMenuUI>();
        
    }
    public override void CheckQuestPartComplete() {
        if (skillsMenuUI.GetTotalSkillPoints() > initialSkillPoints) {
            isQuestComplete = true;
        }
    }

    public void SetInitialSkillPoint() {
        initialSkillPoints = skillsMenuUI.GetTotalSkillPoints();
    }

}
