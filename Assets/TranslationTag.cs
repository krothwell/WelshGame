using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DbUtilities;
using DataUI.Utilities;

namespace DataUI {
    namespace ListItems {
        public class TranslationTag : MonoBehaviour, ISelectableUI {
            VocabTranslationListUI vocabTranslationListUI;
            TagsListUI tagsListUI;
            private string tagText;
            public string TagText {
                get { return tagText; }
                set { tagText = value; }
            }

            private bool isAssignedToSelectedTranslation;
            public bool IsAssignedToSelectedTranslation {
                get { return isAssignedToSelectedTranslation; }
                set { isAssignedToSelectedTranslation = value; }
            }

            GameObject options;
            GameObject textInput;
            GameObject addTagBtn;

            // Use this for initialization
            void Awake() {
                vocabTranslationListUI = FindObjectOfType<VocabTranslationListUI>();
                tagsListUI = FindObjectOfType<TagsListUI>();
                vocabTranslationListUI = FindObjectOfType<VocabTranslationListUI>();
                options = gameObject.transform.Find("Options").gameObject;
                textInput = gameObject.GetComponentInChildren<InputField>().gameObject;

            }

            public void ToggleInsertRemoveToTagList() {
                if (isAssignedToSelectedTranslation) {
                    DeleteFromTagList();
                }
                else {
                    InsertToTagList();
                }
            }

            public void InsertToTagList() {
                print(vocabTranslationListUI);
                Translation currentTranslation = (Translation)vocabTranslationListUI.GetSelectedItemFromGroup(vocabTranslationListUI.VocabTranslationSelected);
                string welsh = currentTranslation.CurrentWelsh;
                string english = currentTranslation.CurrentEnglish; 
                DbCommands.InsertTupleToTable("VocabTagged", tagText, english, welsh);
                addTagBtn.GetComponent<Text>().text = "Remove";
                Image btnImg = textInput.GetComponent<Image>();
                btnImg.color = Color.green;
                IsAssignedToSelectedTranslation = true;
            }


            public void DeleteFromTagList() {
                Translation currentTranslation = (Translation)vocabTranslationListUI.GetSelectedItemFromGroup(vocabTranslationListUI.VocabTranslationSelected);
                string[,] tagFields = new string[,] {
                    { "Tags", tagText },
                    { "EnglishText", currentTranslation.CurrentEnglish },
                    { "WelshText", currentTranslation.CurrentWelsh }
                };
                DbCommands.DeleteTupleInTable("VocabTagged", tagFields);
                addTagBtn.GetComponent<Text>().text = "Add tag";
                Image btnImg = textInput.GetComponent<Image>();
                btnImg.color = Color.white;
                IsAssignedToSelectedTranslation = false;
            }

            public void ActivateAddRule(bool related) {
                addTagBtn = gameObject.transform.Find("AddRemoveTag").gameObject;
                if (related) {
                    addTagBtn.GetComponent<Text>().text = "Remove";
                    Image btnImg = textInput.GetComponent<Image>();
                    btnImg.color = Color.green;
                    IsAssignedToSelectedTranslation = true;
                }
                else { IsAssignedToSelectedTranslation = false; }
                addTagBtn.SetActive(true);
            }

            public void DeactivateTag() {
                addTagBtn = gameObject.transform.Find("AddRemoveTag").gameObject;
                Image btnImg = textInput.GetComponent<Image>();
                btnImg.color = Colours.colorDataUIPanelInactive;
                IsAssignedToSelectedTranslation = false;
                addTagBtn.SetActive(false);
            }

            public void ActivateTagOptions() {
                Image btnImg = textInput.GetComponent<Image>();
                btnImg.color = Color.cyan;
                options.SetActive(true);
            }

            public void Edit() {
                tagsListUI.SetActiveTagEdit();
            }

            public void UpdateTagDisplay(string newText) {
                textInput.GetComponent<InputField>().text = newText;
            }

            public void DeactivateRuleOptions() {
                Image btnImg = textInput.GetComponent<Image>();
                btnImg.color = Color.white;
                options.SetActive(false);
            }

            public void SelectText() {
                EventSystem.current.SetSelectedGameObject(textInput, null);
            }

            public void DeleteTag() {
                string[,] fields = { { "Tags", tagText } };
                DbCommands.DeleteTupleInTable("TranslationTags",
                                             fields);
                Destroy(gameObject);
            }

            public void SelectSelf() {
                vocabTranslationListUI.DeactivateSelectedTranslation();
                tagsListUI.ClearTagging();
                ActivateTagOptions();
            }

            public void DeselectSelf() {
                DeactivateRuleOptions();
            }

            void OnMouseUpAsButton() {
                tagsListUI.ToggleSelectionTo(this, tagsListUI.SelectedTag);
            }

        }
    }
}
