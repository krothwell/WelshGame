using UnityEngine;
using UnityEngine.UI;
using DataUI.ListItems;
using DbUtilities;
using DataUI.Utilities;

namespace DataUI {
    public class TagsListUI : UIController {
        TranslationUI translationUI;
        VocabTranslationListUI vocabTranslationListUI;
        Button tagsListBtn, tagDetailsBtn;
        GameObject tagsList, tagDetails;
        Text tagDetailsBtnText;
        InputField tagDetailsInput;
        public GameObject TagPrefab;
        private string selectedTag;
        bool editingTag;
        public string SelectedTag {
            get { return selectedTag; }
            set { selectedTag = value; }
        }


        // Use this for initialization
        void Start() {
            translationUI = FindObjectOfType<TranslationUI>();
            vocabTranslationListUI = FindObjectOfType<VocabTranslationListUI>();
            tagsListBtn = transform.Find("TagsListBtn").GetComponent<Button>();
            tagsList = GetPanel().GetComponentInChildren<GridLayoutGroup>().gameObject;
            tagDetails = GetPanel().transform.Find("SubmitTagDetails").gameObject;
            tagDetailsBtn = tagDetails.GetComponentInChildren<Button>();
            tagDetailsBtnText = tagDetailsBtn.GetComponentInChildren<Text>();
            tagDetailsInput = tagDetails.transform.Find("Panel").GetComponentInChildren<InputField>();
            selectedTag = "SelectedTag";
            CreateSelectionToggleGroup(selectedTag);
        }

        // Update is called once per frame
        void Update() {

        }

        public void ToggleSideMenuToTagsList() {
            if (!GetPanel().activeSelf) {
                translationUI.ToggleMenuTo(GetComponent<UIController>(), translationUI.SideMenuGroup);
                translationUI.ToggleSelectedSideMenuButton(tagsListBtn);
            }
            Translation translation = (Translation)(vocabTranslationListUI.GetSelectedItemFromGroup(vocabTranslationListUI.VocabTranslationSelected));
            string eng, cym;
            if (translation == null) {
                eng = cym = null;
            }
            else {
                eng = translation.CurrentEnglish; cym = translation.CurrentWelsh;
            }
            FillDisplayFromDb(DbQueries.GetTaggedVocabDisplayQry(eng, cym),
                tagsList.transform,
                BuildTag,
                eng,
                cym
                );
        }

        public Transform BuildTag(string[] strArray) {
            string tagText = strArray[0];
            print(strArray[1]);
            int vocabTagged = int.Parse(strArray[1]);
            GameObject tagGo = Instantiate(TagPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            tagGo.transform.GetComponentInChildren<InputField>().text = tagText;
            print("building tag");
            if (vocabTranslationListUI.IsItemSelectedInGroup(vocabTranslationListUI.VocabTranslationSelected)) {
                if (vocabTagged == 1) {
                    tagGo.GetComponent<TranslationTag>().ActivateAddRule(true);
                }
                else {
                    tagGo.GetComponent<TranslationTag>().ActivateAddRule(false);
                }
            }
            tagGo.GetComponentInChildren<InputField>().text = tagText;
            tagGo.GetComponent<TranslationTag>().TagText = tagText;
            //grammarRule.transform.SetParent(ruleList.transform, false);
            return tagGo.transform;
        }

        public void SetActiveTagEdit() {
            tagDetailsBtnText.text = "Edit tag";
            editingTag = true;
            ActivateTagDetails();
        }

        public void ActivateTagDetails() {
            tagDetails.transform.Find("Panel").gameObject.SetActive(true);
            tagDetailsBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUItxt); //indicate to user that button no longer functions.
            if (editingTag) {
                TranslationTag selectedTagObj = (TranslationTag)(GetSelectedItemFromGroup(selectedTag));
                tagDetailsInput.text = selectedTagObj.TagText;
            }
        }

        public void ClearTagging() {
            foreach (Transform tt in tagsList.transform) {
                TranslationTag ttag = tt.GetComponent<TranslationTag>();
                ttag.DeactivateTag();
            }
        }

        public void UpdateInsertTag() {
            if (tagDetailsInput.text != null) {
                if (editingTag) {
                    string[,] fieldVals = new string[,] {
                                                { "Tags", tagDetailsInput.text },
                                            };
                    TranslationTag tt = GetSelectedItemFromGroup(SelectedTag) as TranslationTag;
                    string tagTxt = tt.GetComponent<TranslationTag>().TagText;
                    DbCommands.UpdateTableTuple("TranslationTags", "Tags = " + DbCommands.GetParameterNameFromValue(tagTxt), fieldVals, tagTxt);
                    tt.GetComponent<TranslationTag>().UpdateTagDisplay(tagDetailsInput.text);
                }
                else {
                    DbCommands.InsertTupleToTable("TranslationTags", tagDetailsInput.text);
                    FillDisplayFromDb(DbQueries.GetTaggedVocabDisplayQry(), tagsList.transform, BuildTag);
                }
            }

        }

        public void DeactivateTagDetails() {
            tagDetailsInput.text = "";
            tagDetailsBtnText.text = "New tag";
            editingTag = false;
            tagDetails.transform.Find("Panel").gameObject.SetActive(false);
            tagDetailsBtn.GetComponent<Button>().colors.normalColor.Equals(Colours.colorDataUIbtn); //indicate to user that button is functioning.
        }
    }
}