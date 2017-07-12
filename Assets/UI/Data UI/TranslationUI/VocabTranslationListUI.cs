using UnityEngine;
using UnityEngine.UI;
using DataUI.ListItems;
using DbUtilities;
using DataUI.Utilities;


namespace DataUI {
    public class VocabTranslationListUI : UIController {
        
        //translations
        GameObject vocabTranslationList,
                   submitNewTranslation, newTranslationPanel, activateNewTranslationBtn,
                   translation;

        private string vocabTranslationSelected;
        public string VocabTranslationSelected {
            get { return vocabTranslationSelected; }
            set { vocabTranslationSelected = value; }
        }
        public GameObject translationPrefab;
        InputField inputTranslationEnTxt, inputTranslationCyTxt;
        InputField searchTranslations;
        float searchWait = 0f;
        bool searchCountDown = false;
        //int translationSelectInt = 0;

        void Update() {
            if (searchCountDown) {
                searchWait -= Time.deltaTime;
                if (searchWait <= 0) {
                    SearchTranslationNow();
                    searchCountDown = false;
                }
            }
        }

        void Start() {
            //TRANSLATIONS UI
            //translationUI = FindObjectOfType<TranslationUI>().gameObject;
            //translationUIpanel = translationUI.transform.Find("Panel").gameObject;
            //adding
            submitNewTranslation = GetPanel().transform.Find("SubmitNewTranslation").gameObject;
            activateNewTranslationBtn = submitNewTranslation.transform.Find("ActivateNewTranslationBtn").gameObject;
            newTranslationPanel = submitNewTranslation.transform.Find("NewTranslationPanel").gameObject;
            inputTranslationEnTxt = newTranslationPanel.transform.Find("EnglishVocab").GetComponent<InputField>();
            inputTranslationCyTxt = newTranslationPanel.transform.Find("WelshVocab").GetComponent<InputField>();
            //searching
            searchTranslations = GetPanel().transform.Find("SearchTranslations").GetComponent<InputField>();
            vocabTranslationSelected = "VocabTranslationSelected";
            CreateSelectionToggleGroup(vocabTranslationSelected);
            vocabTranslationList = transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
            FillDisplayFromDb(DbCommands.GetTranslationsDisplayQry(), vocabTranslationList.transform, BuildVocabTranslation);
        }


        //Display a panel to add a new translation 
        public void ActivateNewTranslation() {
            newTranslationPanel.SetActive(true);
            activateNewTranslationBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUItxt); //indicate to user that button no longer functions.
        }

        public void DeactivateSelectedTranslation() {
            Translation selectedTranslation = (Translation)GetSelectedItemFromGroup(vocabTranslationSelected);
            if (selectedTranslation != null) {
                selectedTranslation.DeselectSelf();
            }
        }

        public void DeactivateNewTranslation() {
            newTranslationPanel.SetActive(false);
            activateNewTranslationBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUIbtn); //indicate to user that button is functioning.
        }

        public void InsertTranslation() {
            DbCommands.InsertTupleToTable("EnglishVocab", inputTranslationEnTxt.text);
            DbCommands.InsertTupleToTable("WelshVocab", inputTranslationCyTxt.text);
            DbCommands.InsertTupleToTable("VocabTranslations", inputTranslationEnTxt.text, inputTranslationCyTxt.text);
            FillDisplayFromDb(DbCommands.GetTranslationsDisplayQry(), vocabTranslationList.transform, BuildVocabTranslation);
            searchTranslations.text = inputTranslationEnTxt.text;
            SearchTranslationNow();
            inputTranslationEnTxt.text = "";
            inputTranslationCyTxt.text = "";

        }



        private Transform BuildVocabTranslation(string[] strArray) {
            string EnStr = strArray[0];
            string CyStr = strArray[1];
            translation = Instantiate(translationPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            translation.transform.Find("EnglishVocab").GetComponent<InputField>().text = EnStr;
            translation.transform.Find("WelshVocab").GetComponent<InputField>().text = CyStr;
            translation.GetComponent<Translation>().CurrentEnglish = EnStr;
            translation.GetComponent<Translation>().CurrentWelsh = CyStr;
            //translation.transform.SetParent(translations.transform, false);
            return translation.transform;
        }

        public void SearchTranslations() {
            searchWait = 0.5f;
            searchCountDown = true;
        }

        private void SearchTranslationNow() {
            if (searchTranslations.text == "") {
                FillDisplayFromDb(DbCommands.GetTranslationsDisplayQry(), vocabTranslationList.transform, BuildVocabTranslation);
            }
            else {
                string searchText = "%" + searchTranslations.text + "%";
                string sqlqry = DbQueries.GetTranslationSearchQry(searchText);
                FillDisplayFromDb(sqlqry, vocabTranslationList.transform, BuildVocabTranslation, searchText);
            }
        }
    }
}