using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataUI;

/// <summary>
/// Controls activation and display of panels belonging to the UI controllers 
/// underneath. 
/// </summary>
public class DataUIController : UIController {
    string dataUImenusToggleGroupName;
    DialogueUI dialogueUI;
    TranslationUI translationUI;
    QuestsUI questsUI;
    void Start() {
        dialogueUI = GetPanel().GetComponentInChildren<DialogueUI>();
        translationUI = GetPanel().GetComponentInChildren<TranslationUI>();
        questsUI = GetPanel().GetComponentInChildren<QuestsUI>();
        dataUImenusToggleGroupName = "DataUImenusToggleGroup";
        CreateNewMenuToggleGroup(dataUImenusToggleGroupName);
    }

    public void ActivateDialogueUI() {
        ToggleMenuTo(dialogueUI, dataUImenusToggleGroupName);
    }

    public void ActivateTranslationUI() {
        ToggleMenuTo(translationUI, dataUImenusToggleGroupName);
    }
    
    public void ActivateQuestsUI() {
        ToggleMenuTo(questsUI, dataUImenusToggleGroupName);
    }
}

