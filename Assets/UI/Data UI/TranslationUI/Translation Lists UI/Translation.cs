using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {

        public class Translation : UIInputListItem {

            private string currentEnglish;
            public string CurrentEnglish {
                get { return currentEnglish; }
                set { currentEnglish = value; }
            }
            private string currentWelsh;
            public string CurrentWelsh {
                get { return currentWelsh; }
                set { currentWelsh = value; }
            }
            TranslationUI translationUI;
            GameObject translationOptions;
            GameObject englishText, welshText;
            GameObject saveBtn;
            GameObject panel;

            // Use this for initialization
            void Start() {
                translationOptions = gameObject.transform.FindChild("TranslationOptions").gameObject;
                englishText = gameObject.transform.FindChild("EnglishVocab").gameObject;
                welshText = gameObject.transform.FindChild("WelshVocab").gameObject;
                panel = gameObject.transform.FindChild("Panel").gameObject;
                saveBtn = gameObject.transform.FindChild("Save").gameObject;
                translationUI = FindObjectOfType<TranslationUI>();
            }

            void Update() {
                SetSelectionOfTranslationOnClick();
            }


            public void SetSelectionOfTranslationOnClick() {
                if (Input.GetMouseButtonUp(0)) {
                    if (MouseSelection.IsClickedDifferentGameObjectTo(this.gameObject)
                        && !MouseSelection.IsClickedGameObjectName("AddRemoveRule")) {
                        if (translationOptions.activeSelf || panel.activeSelf) {
                            DeactivateTranslationOptions();
                            DisableEdits();
                            englishText.GetComponent<InputField>().text = CurrentEnglish;
                            welshText.GetComponent<InputField>().text = CurrentWelsh;
                            translationUI.SetTranslationSelectedProperties(-1);
                            if (!translationUI.TranslationSelected()) {
                                translationUI.FillRulesNotSelected();
                            }
                        }
                    }
                }
            }

            public void ActivateTranslationOptions() {
                //proficiencyText.GetComponent<Image>().color = new Color(0.7f,0.85f,1f);
                translationOptions.SetActive(true);
                panel.SetActive(true);
            }

            public void DeactivateTranslationOptions() {
                translationOptions.SetActive(false);
            }

            public void EnableEdits() {
                englishText.GetComponent<InputField>().readOnly = false;
                welshText.GetComponent<InputField>().readOnly = false;
                saveBtn.SetActive(true);
            }

            public void DisableEdits() {
                englishText.GetComponent<InputField>().readOnly = true;
                welshText.GetComponent<InputField>().readOnly = true;
                saveBtn.SetActive(false);
                panel.SetActive(false);
            }

            public void UpdateTranslation() {

                string newEn = englishText.GetComponent<InputField>().text;
                string newCy = welshText.GetComponent<InputField>().text;
                /* We need to check if we are updating a value with more than one entry in translations. If so, a new value needs to be inserted
                to English / Welsh tables to avoid changing all of the translation entry values to the same value. This results in the translation
                needing to be updated rather than the English / Welsh as would otherwise be updated and the update cascaded.*/
                if (DbCommands.GetCountFromTable("VocabTranslations",
                                                "EnglishText = " + DbCommands.GetParameterNameFromValue(CurrentEnglish),
                                                CurrentEnglish)
                > 1) {
                    DbCommands.InsertTupleToTable("EnglishVocab", newEn);
                    DbCommands.UpdateTableField("VocabTranslations",
                                                 "EnglishText",
                                                 newEn,
                                                "EnglishText = " + DbCommands.GetParameterNameFromValue(CurrentEnglish) +
                                                    " AND " +
                                                "WelshText = " + DbCommands.GetParameterNameFromValue(CurrentWelsh),
                                                CurrentEnglish, CurrentWelsh);
                }
                else {
                    print("Updating English tbl");
                    DbCommands.UpdateTableField("EnglishVocab",
                                             "EnglishText",
                                             newEn,
                                            "EnglishText = " + DbCommands.GetParameterNameFromValue(CurrentEnglish),
                                            CurrentEnglish);
                    print("success");
                }
                if (DbCommands.GetCountFromTable("VocabTranslations",
                                                "WelshText = " + DbCommands.GetParameterNameFromValue(CurrentWelsh), CurrentWelsh)
                > 1) {
                    DbCommands.InsertTupleToTable("Welsh", newCy);
                    DbCommands.UpdateTableField("VocabTranslations",
                                             "WelshText",
                                             newCy,
                                            "EnglishText = " + DbCommands.GetParameterNameFromValue(newEn) +
                                                " AND " + //newEn must be used since it will have been update before, rather than simultaneously.
                                            "WelshText = " + DbCommands.GetParameterNameFromValue(CurrentWelsh),
                                            CurrentEnglish, CurrentWelsh);
                }
                else {
                    DbCommands.UpdateTableField("WelshVocab",
                                             "WelshText",
                                             newCy,
                                            "WelshText =  " + DbCommands.GetParameterNameFromValue(CurrentWelsh),
                                            CurrentWelsh);
                }
                CurrentEnglish = newEn;
                CurrentWelsh = newCy;
                translationUI.SetTranslationSelectedProperties(-1);
                translationUI.FillRulesNotSelected();
            }

            /* */
            public void DeleteTranslation() {
                string[,] translationFields = new string[,] { { "EnglishText", CurrentEnglish }, { "WelshText", CurrentWelsh } };
                string[,] englishFields = new string[,] { { "EnglishText", CurrentEnglish } };
                string[,] welshFields = new string[,] { { "WelshText", CurrentWelsh } };
                int translationsWithEnglish = DbCommands.GetCountFromTable("VocabTranslations",
                                                                        "EnglishText = " + DbCommands.GetParameterNameFromValue(CurrentEnglish),
                                                                        CurrentEnglish);
                int translationsWithWelsh = DbCommands.GetCountFromTable("VocabTranslations",
                                                                     "WelshText = " + DbCommands.GetParameterNameFromValue(CurrentWelsh),
                                                                     CurrentWelsh);

                if (translationsWithEnglish <= 1 && translationsWithWelsh <= 1) {
                    DbCommands.DeleteTupleInTable("EnglishVocab", englishFields);
                    DbCommands.DeleteTupleInTable("WelshVocab", welshFields);
                }
                else if (translationsWithEnglish > 1 && translationsWithWelsh <= 1) {
                    DbCommands.DeleteTupleInTable("VocabTranslations", translationFields);
                    DbCommands.DeleteTupleInTable("WelshVocab", welshFields);
                }
                else if (translationsWithEnglish <= 1 && translationsWithWelsh > 1) {
                    DbCommands.DeleteTupleInTable("VocabTranslations", translationFields);
                    DbCommands.DeleteTupleInTable("EnglishVocab", englishFields);
                }
                else if (translationsWithEnglish > 1 && translationsWithWelsh > 1) {
                    DbCommands.DeleteTupleInTable("VocabTranslations", translationFields);
                }
                Destroy(gameObject);
                translationUI.SetTranslationSelectedProperties(-1);
                translationUI.FillRulesNotSelected();
            }

            void OnMouseUp() {
                print("selected");
                if (!translationOptions.activeSelf && !panel.activeSelf) {
                    print("selected");
                    ActivateTranslationOptions();

                    translationUI.SetTranslationSelectedProperties(1, CurrentEnglish, CurrentWelsh);

                    translationUI.ActivateGrammarRulesMenu(CurrentEnglish, CurrentWelsh);
                }
            }

        }
    }
}