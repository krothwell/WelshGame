using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class Proficiency : MonoBehaviour {
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

            void Start() {

                proficiencyText = gameObject.transform.FindChild("ProficiencyText").gameObject;
                proficiencySelector = proficiencyText.transform.FindChild("Selector").gameObject;
                proficiencyOptions = gameObject.transform.FindChild("ProficiencyOptions").gameObject;
                proficiencySaveBtn = gameObject.transform.FindChild("Save").gameObject;
                thresholdText = gameObject.transform.FindChild("ThresholdInput").gameObject;
                defaultProficiencyTxtBgColor = proficiencyText.GetComponent<Image>().color;
            }



            void Update() {
                DeselectProficiencyOnClickAway();
            }

            public void DeselectProficiencyOnClickAway() {
                if (Input.GetMouseButtonUp(0)) {
                    MouseSelection.ClickSelect();
                    if (MouseSelection.ClickedDifferentGameObjectTo(this.gameObject)) {
                        ActivateSelectorBtn();
                        DeactivateProficiencyOptions();
                        DisableEdits();
                        proficiencyText.GetComponent<InputField>().text = CurrentProficiencyName;
                        thresholdText.GetComponent<InputField>().text = CurrentThreshold.ToString();
                    }
                }
            }

            public void ActivateSelectorBtn() {
                proficiencySelector.gameObject.SetActive(true);
            }

            public void DeactivateProficiencySelectorBtn() {
                proficiencySelector.gameObject.SetActive(false);
            }

            public void ActivateProficiencyOptions() {
                proficiencyText.GetComponent<Image>().color = new Color(0.7f, 0.85f, 1f);
                proficiencyOptions.SetActive(true);
            }

            public void DeactivateProficiencyOptions() {
                proficiencyText.GetComponent<Image>().color = defaultProficiencyTxtBgColor;
                proficiencyOptions.SetActive(false);
            }

            public void EnableEdits() {
                proficiencyText.GetComponent<InputField>().readOnly = false;
                thresholdText.GetComponent<InputField>().readOnly = false;
                proficiencySaveBtn.SetActive(true);
            }

            public void DisableEdits() {
                proficiencyText.GetComponent<InputField>().readOnly = true;
                thresholdText.GetComponent<InputField>().readOnly = true;
                proficiencySaveBtn.SetActive(false);
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
                ActivateSelectorBtn();
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