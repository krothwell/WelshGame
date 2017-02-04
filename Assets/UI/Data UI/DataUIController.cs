using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataUI;

public class DataUIController : UIController {
    string dataUImenusToggleGroupName;
    DialogueUI dialogueUI;
    TranslationUI translationUI;
    void Start() {
        dialogueUI = FindObjectOfType<DialogueUI>();
        translationUI = FindObjectOfType<TranslationUI>();
        dataUImenusToggleGroupName = "DataUImenusToggleGroup";
        CreateNewMenuToggleGroup(dataUImenusToggleGroupName);
        AddNewMenuToToggleGroup(dataUImenusToggleGroupName, dialogueUI);
        AddNewMenuToToggleGroup(dataUImenusToggleGroupName, translationUI);
    }

    public void ActivateDialogueUI() {
        ToggleMenuTo(dialogueUI, dataUImenusToggleGroupName);
    }

    public void ActivateTranslationUI() {
        ToggleMenuTo(translationUI, dataUImenusToggleGroupName);
    }
}
