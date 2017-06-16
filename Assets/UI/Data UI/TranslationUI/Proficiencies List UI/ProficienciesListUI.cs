using UnityEngine;
using UnityEngine.UI;
using DataUI.ListItems;
using DbUtilities;
using DataUI.Utilities;

namespace DataUI {
    public class ProficienciesListUI : UIController {
        //high level cross cutting objects and holders
        TranslationUI translationUI;
        private string selectedProficiency;
        public string SelectedProficiency {
            get { return selectedProficiency; }
            set { selectedProficiency = value; }
        }
        //proficienies
        private GameObject proficienciesListUIpanel, proficienciesList,
                   submitNewProficiency, newProficiencyPanel, activateNewProficiencyBtn,
                   proficiency;
        public GameObject ProficienciesList {
            set { proficienciesList = value; }
            get { return proficienciesList; }
        }
        public GameObject proficiencyPrefab;
        InputField inputProficiencyTxt, inputThresholdTxt;

        void Start() {
            translationUI = FindObjectOfType<TranslationUI>();
            //PROFICIENCIES UI
            proficienciesList = GetPanel().GetComponentInChildren<GridLayoutGroup>().gameObject;
            //adding
            submitNewProficiency = GetPanel().transform.Find("SubmitNewProficiency").gameObject;
            newProficiencyPanel = submitNewProficiency.transform.Find("NewProficiencyPanel").gameObject;
            activateNewProficiencyBtn = submitNewProficiency.transform.Find("ActivateNewProficiencyBtn").gameObject;
            inputProficiencyTxt = newProficiencyPanel.transform.Find("InputProficiency").GetComponent<InputField>();
            inputThresholdTxt = newProficiencyPanel.transform.Find("ThresholdInput").GetComponent<InputField>();
            selectedProficiency = "SelectedProficiency";
            CreateSelectionToggleGroup(selectedProficiency);
        }

        //Display a panel to add a new proficiency 
        public void ActivateNewProficiency() {
            newProficiencyPanel.SetActive(true);
            activateNewProficiencyBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUItxt); //indicate to user that button no longer functions.
        }

        public void DeactivateNewProficiency() {
            newProficiencyPanel.SetActive(false);
            activateNewProficiencyBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUIbtn); //indicate to user that button is functioning.
        }

        public void InsertProficiency() {
            if ((inputProficiencyTxt.text != null) && (inputProficiencyTxt.text != "")) {
                DbCommands.InsertTupleToTable("Proficiencies", inputProficiencyTxt.text, inputThresholdTxt.text);
                FillDisplayFromDb(DbCommands.GetProficienciesDisplayQry(), proficienciesList.transform, BuildProficiency);
                inputProficiencyTxt.text = "";
                inputThresholdTxt.text = "";
            }
        }

        public Transform BuildProficiency(string[] strArray) {
            string proficiencyStr = (strArray[0]);
            string thresholdStr = (strArray[1]);
            proficiency = Instantiate(proficiencyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            proficiency.transform.Find("ProficiencyText").GetComponent<InputField>().text = proficiencyStr;
            proficiency.transform.Find("ThresholdInput").GetComponent<InputField>().text = thresholdStr;
            proficiency.GetComponent<Proficiency>().CurrentProficiencyName = proficiencyStr;
            proficiency.GetComponent<Proficiency>().CurrentThreshold = int.Parse(thresholdStr);
            //proficiency.transform.SetParent(proficiencyThresholds.transform, false);
            return proficiency.transform;
        }

        private int GetDefaultProficiencyThreshold() {
            int defaultProficiencyThreshold = (GameObject.FindObjectsOfType<Proficiency>().Length - 1) * 10;
            return defaultProficiencyThreshold;

        }
    }
}