using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour {
    private int id;
    public int ID {
        get { return id; }
        set { id = value; }
    }
    private string saveRef;
    public string SaveRef {
        get { return saveRef; }
        set { saveRef = value; }
    }
    private string charName;
    public string CharName {
        get { return charName; }
        set { charName = value; }
    }
    private string saveDate;
    public string SaveDate {
        get { return saveDate; }
        set { saveDate = value; }
    }

    SaveGameManager saveGameManager;
    GameObject panel;

    // Use this for initialization
    void Start () {
        panel = transform.FindChild("Panel").gameObject;
        saveGameManager = FindObjectOfType<SaveGameManager>();
	}

    // Update is called once per frame
    void Update() {
        SetSelectionOfSaveOnClick();
    }

    public void SetSelectionOfSaveOnClick() {
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();
            if (SelectController.ClickedDifferentGameObjectTo(this.gameObject)
                && !SelectController.IsClickedGameObjectTag("SaveOption")) {
                SetDeselected();
                //            DisableEdits();
                //            englishText.GetComponent<InputField>().text = CurrentEnglish;
                //            welshText.GetComponent<InputField>().text = CurrentWelsh;
                //            translationUI.SetTranslationSelectedProperties(-1);
                //            if (!translationUI.TranslationSelected()) {
                //                translationUI.FillRulesNotSelected();
                //            }
                //        }
            }
        }
    }

    public void SetSelected() {
        panel.GetComponent<Image>().color = new Color(0.9f, 0.99f, 0.95f);
        saveGameManager.SelectedSave = gameObject.GetComponent<SaveGame>();
        if (saveGameManager.newSaveInput != null) {
            saveGameManager.ShowSelectedSaveInInput();
        }
    }

    public void SetDeselected() {
        panel.GetComponent<Image>().color = new Color(0.82f, 0.94f, 0.9999f);
        if (saveGameManager.SelectedSave == gameObject.GetComponent<SaveGame>()) { 
            saveGameManager.SelectedSave = null;
        }
        if (saveGameManager.newSaveInput != null && saveGameManager.SelectedSave == null) {
            saveGameManager.ShowDefaultTextInInput();
        }
    }
}
