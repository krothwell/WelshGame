using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
public abstract class QuestTaskPart : MonoBehaviour {
    public string MyLabel;
    protected string partID, taskID, questName;
    protected bool isQuestComplete;
    protected QuestsUI questsUI;

    public abstract void CheckQuestPartComplete();

    void Update() {
        if (isQuestComplete) {
            SetQuestTaskPartComplete();
        } else { 
            CheckQuestPartComplete();
        }
    }

    public void SetQuestTaskPartComplete() {
        questsUI.CompleteTaskPart(partID, taskID, questName);
    }

    public void InitialiseMe(string partIDstr, string taskIDstr, string questNameStr) {
        SetTaskPartID(partIDstr);
        SetTaskID(taskIDstr);
        SetQuestName(questNameStr);
    }

    public void SetTaskPartID(string idStr) {
        partID = idStr;
    }

    public void SetTaskID(string idStr) {
        taskID = idStr;
    }

    public void SetQuestName(string nameStr) {
        questName = nameStr;
    }

    public string GetMyLabel() {
        return MyLabel;
    }
}
