using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {

        public class Translation : UIInputListItem, ISelectableUI {
            VocabTranslationListUI vocabTranslationListUI;
            GrammarListUI grammarListUI;
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
            bool editing;

            // Use this for initialization
            void Start() {
                vocabTranslationListUI = FindObjectOfType<VocabTranslationListUI>();
                translationOptions = gameObject.transform.Find("TranslationOptions").gameObject;
                englishText = gameObject.transform.Find("EnglishVocab").gameObject;
                welshText = gameObject.transform.Find("WelshVocab").gameObject;
                //panel = gameObject.transform.Find("Panel").gameObject;
                saveBtn = gameObject.transform.Find("Save").gameObject;
                translationUI = FindObjectOfType<TranslationUI>();
            }

            public void ActivateTranslationOptions() {
                print("activating translation options for: " + currentEnglish);
                print(translationOptions);
                translationOptions.SetActive(true);
            }

            public void DeactivateTranslationOptions() {
                DisableEdits();
                print("DEACTIVATING translation options for " + currentEnglish);
                translationOptions.SetActive(false);
                
            }

            public void EnableEdits() {
                editing = true;
                englishText.GetComponent<InputField>().readOnly = false;
                welshText.GetComponent<InputField>().readOnly = false;
                saveBtn.SetActive(true);
            }

            public void DisableEdits() {
                editing = false;
                englishText.GetComponent<InputField>().readOnly = true;
                welshText.GetComponent<InputField>().readOnly = true;
                saveBtn.SetActive(false);
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
                grammarListUI.FillRulesNotSelected();
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
                grammarListUI.FillRulesNotSelected();
            }

            public void SelectSelf() {
                if (!translationOptions.activeSelf && !editing) {
                    print("mouse up");
                    ActivateTranslationOptions();
                    vocabTranslationListUI.CurrentTranslation = this;
                    translationUI.ToggleSideMenu();
                }
            }

            public void DeselectSelf() {
                DeactivateTranslationOptions();
                DisableEdits();
            }

            void OnMouseUpAsButton() {
                if (!editing) {
                    print(vocabTranslationListUI);
                    print(vocabTranslationListUI.VocabTranslationSelected);
                    vocabTranslationListUI.ToggleSelectionTo(this, vocabTranslationListUI.VocabTranslationSelected);
                }
            }

        }
    }
}