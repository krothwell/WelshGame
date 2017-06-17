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

        //GrammarListUI grammarListUI;
        //ProficienciesListUI proficienciesListUI;
        public string SideMenuGroup;
        void Start() {
            //grammarListUI = GetPanel().GetComponentInChildren<GrammarListUI>();
            //proficienciesListUI = GetPanel().GetComponentInChildren<ProficienciesListUI>();
            SideMenuGroup = "SideMenuGroup";
            CreateNewMenuToggleGroup(SideMenuGroup);
        }
    }
}