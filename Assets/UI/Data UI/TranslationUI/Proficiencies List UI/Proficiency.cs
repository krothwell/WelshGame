using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class Proficiency : MonoBehaviour, ISelectableUI {
            private string currentProficiencyName;
            public string CurrentProficiencyName {
                get { return currentProficiencyName; }
                set { currentProficiencyName = value; }
            }
            private int currentThreshold;
            public int CurrentThreshold {
                get { return currentThreshold; }
                set { currentThreshold = value; }
            }
            GameObject proficiencyText;
            GameObject proficiencySelector;
            GameObject proficiencyOptions;
            GameObject proficiencySaveBtn;
            GameObject thresholdText;
            Color defaultProficiencyTxtBgColor;
            ProficienciesListUI proficiencyListUI;
            bool isEditing;

            void Start() {

                proficiencyText = gameObject.transform.Find("ProficiencyText").gameObject;
                proficiencyOptions = gameObject.transform.Find("ProficiencyOptions").gameObject;
                proficiencySaveBtn = gameObject.transform.Find("Save").gameObject;
                thresholdText = gameObject.transform.Find("ThresholdInput").gameObject;
                defaultProficiencyTxtBgColor = proficiencyText.GetComponent<Image>().color;
                proficiencyListUI = FindObjectOfType<ProficienciesListUI>();
            }

            public void SelectSelf() {
                ActivateProficiencyOptions();
            }

            public void DeselectSelf() {
                DeactivateProficiencyOptions();
                DisableEdits();
                proficiencyText.GetComponent<InputField>().text = CurrentProficiencyName;
                thresholdText.GetComponent<InputField>().text = CurrentThreshold.ToString();
            }

            void OnMouseUpAsButton() {
                proficiencyListUI.ToggleSelectionTo(this, proficiencyListUI.SelectedProficiency);
            }

            public void ActivateProficiencyOptions() {
                if (!isEditing) {
                    proficiencyText.GetComponent<Image>().color = new Color(0.7f, 0.85f, 1f);
                    proficiencyOptions.SetActive(true);
                }
            }

            public void DeactivateProficiencyOptions() {
                proficiencyText.GetComponent<Image>().color = defaultProficiencyTxtBgColor;
                proficiencyOptions.SetActive(false);
            }

            public void EnableEdits() {
                proficiencyText.GetComponent<InputField>().readOnly = false;
                thresholdText.GetComponent<InputField>().readOnly = false;
                proficiencySaveBtn.SetActive(true);
                isEditing = true;
            }

            public void DisableEdits() {
                proficiencyText.GetComponent<InputField>().readOnly = true;
                thresholdText.GetComponent<InputField>().readOnly = true;
                proficiencySaveBtn.SetActive(false);
                isEditing = false;
            }

            public void SelectText() {
                EventSystem.current.SetSelectedGameObject(proficiencyText, null);
            }

            public void UpdateProficiency() {
                string newName = proficiencyText.GetComponent<InputField>().text;
                string newThreshold = thresholdText.GetComponent<InputField>().text;
                DbCommands.UpdateTableField("Proficiencies",
                                         "ProficiencyNames",
                                         newName,
                                        "ProficiencyNames = " + DbCommands.GetParameterNameFromValue(CurrentProficiencyName),
                                        CurrentProficiencyName);
                DbCommands.UpdateTableField("Proficiencies",
                                         "Thresholds",
                                         newThreshold,
                                         "ProficiencyNames = " + DbCommands.GetParameterNameFromValue(newName),
                                        newName);
                CurrentProficiencyName = newName;
                CurrentThreshold = int.Parse(newThreshold);
            }

            public void DeleteProficiency() {
                string[,] fields = { { "ProficiencyNames", CurrentProficiencyName } };
                DbCommands.DeleteTupleInTable("Proficiencies",
                                             fields);
                Destroy(gameObject);
            }

        }
    }
}