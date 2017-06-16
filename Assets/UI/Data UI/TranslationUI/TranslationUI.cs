using UnityEngine;
using UnityEngine.UI;
using DataUI.ListItems;
using DbUtilities;
using DataUI.Utilities;


namespace DataUI {
    /// <summary>
    /// Responsible for menus to add new translations and related data (grammar 
    /// and proficiencies) to the database and displaying related data in lists
    /// so that they can be edited or deleted. Translations created here can be 
    /// tagged with grammar. Proficiencies which relate to translations and 
    /// grammar can also be added / edited. Methods (probably only in the games
    /// dialogue UI) will use the database data managed here to help the player
    /// learn Welsh and track their progress, but they do not communicate to
    /// this class or children of this class.
    /// </summary>
    public class TranslationUI : UIController {
        Button grammarRulesBtn, proficienciesBtn;
        GrammarListUI grammarListUI;
        ProficienciesListUI proficienciesListUI;
        public string SideMenuGroup;
        void Start() {
            grammarListUI = GetPanel().GetComponentInChildren<GrammarListUI>();
            proficienciesListUI = GetPanel().GetComponentInChildren<ProficienciesListUI>();
            SideMenuGroup = "SideMenuGroup";
            CreateNewMenuToggleGroup(SideMenuGroup);
            proficienciesBtn = proficienciesListUI.transform.Find("ProficienciesBtn").gameObject.GetComponent<Button>();
            grammarRulesBtn = grammarListUI.transform.Find("GrammarRulesBtn").gameObject.GetComponent<Button>();
        }

        public void ToggleSideMenu() {
            if (grammarListUI.GetPanel().activeSelf) {
                ToggleMenuTo(proficienciesListUI.GetComponent<UIController>(), SideMenuGroup);
                Proficiency proficiency = (Proficiency)(proficienciesListUI.GetSelectedItemFromGroup(proficienciesListUI.SelectedProficiency));
                proficienciesBtn.colors.normalColor.Equals(Colours.colorDataUItxt);
                grammarRulesBtn.colors.normalColor.Equals(Colours.colorDataUIbtn);
                FillDisplayFromDb(DbCommands.GetProficienciesDisplayQry(), proficienciesListUI.ProficienciesList.transform, proficienciesListUI.BuildProficiency);
            }
            else {
                ToggleMenuTo(grammarListUI.GetComponent<UIController>(), SideMenuGroup);
                Translation translation = (Translation)(grammarListUI.GetSelectedItemFromGroup(grammarListUI.TranslationSelected));
                proficienciesBtn.colors.normalColor.Equals(Colours.colorDataUIbtn);
                grammarRulesBtn.colors.normalColor.Equals(Colours.colorDataUItxt);
                FillDisplayFromDb(DbQueries.GetGrammarRuleDisplayQry(translation.CurrentEnglish, translation.CurrentWelsh),
                    grammarListUI.GrammarList.transform,
                    grammarListUI.BuildRule,
                    translation.CurrentEnglish,
                    translation.CurrentWelsh
                );
            }
        }
    }
}