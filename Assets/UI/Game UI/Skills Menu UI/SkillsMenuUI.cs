using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsMenuUI : UIController {
    WelshSkillsListUI welshSkillsListUI;

    void Start() {
        welshSkillsListUI = FindObjectOfType<WelshSkillsListUI>();
    }
    public new void DisplayComponents() {
        base.DisplayComponents();
        welshSkillsListUI.DisplayWelshSkills();
    }
}
