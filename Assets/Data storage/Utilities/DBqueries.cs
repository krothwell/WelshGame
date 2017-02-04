

namespace DbUtilities {
    /// <summary>
    /// Queries used to pull data from the SQLite database. As a general rule, any variables that are used as values in
    /// the query (e.g. to customise the records returned when used with the database) should use the function GetParameterNameFromValue.
    /// This should be used "both ends", when the query string is built, and when the parameters are entered into the database
    /// before the query is ran. This ensures that the parameter names match up since they are both derived from the function using
    /// the same value. The function is part of the DbCommands class in the DbUtilities namespace.
    /// </summary>
    public class DBqueries {

        /// <summary>
        /// A left join is used to check if the grammar rule is related to a particular translation in the
        /// vocab rule list. By default the primary key values for the rule list are set to null, and the relation
        /// will be set to 0 if for all of the grammar rules unless these args are entered.
        /// </summary>
        public static string GetGrammarRuleDisplayQry(string englishStr = null, string welshStr = null) {
            string englishStrQry = "''";
            string welshStrQry = "''";
            if (englishStr != null) {
                englishStrQry = DbCommands.GetParameterNameFromValue(englishStr);
            }
            if (welshStr != null) {
                welshStrQry = DbCommands.GetParameterNameFromValue(welshStr);
            }
            return "SELECT VocabGrammar.RuleIDs, VocabGrammar.ShortDescriptions, " +
                        "CASE WHEN VocabGrammar.RuleIDs = VocabRuleList.RuleIDs THEN 1 ELSE 0 END TranslationRules " +
                    "FROM VocabGrammar " +
                    "LEFT JOIN VocabRuleList ON VocabGrammar.RuleIDs = VocabRuleList.RuleIDs " +
                    "AND VocabRuleList.EnglishText = " + englishStrQry + " AND VocabRuleList.WelshText = " + welshStrQry + " " +
                        "ORDER BY VocabGrammar.RuleIDs ASC;";
        }

        public static string GetTranslationSearchQry(string searchStr) {
            string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
            return "SELECT * FROM VocabTranslations WHERE EnglishText LIKE " + searchParam + " OR WelshText LIKE " + searchParam + " ORDER BY EnglishText ASC;";
        }

        public static string GetCharacterNamesWithScene(string sceneName) {
            string sceneParam = DbCommands.GetParameterNameFromValue(sceneName);
            return "SELECT CharacterNames FROM Characters WHERE scenes = " + sceneParam + ";";
        }
    }
}