using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataUI;

public class DataUIController : UIController {
    string dataUImenusToggleGroupName;
    DialogueUI dialogueUI;
    TranslationUI translationUI;
    void Start() {
        dialogueUI = GetPanel().GetComponentInChildren<DialogueUI>();
        translationUI = GetPanel().GetComponentInChildren<TranslationUI>();
        dataUImenusToggleGroupName = "DataUImenusToggleGroup";
        CreateNewMenuToggleGroup(dataUImenusToggleGroupName);
    }

    public void ActivateDialogueUI() {
        ToggleMenuTo(dialogueUI, dataUImenusToggleGroupName);
    }

    public void ActivateTranslationUI() {
        ToggleMenuTo(translationUI, dataUImenusToggleGroupName);
    }
}
