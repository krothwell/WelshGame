

namespace DbUtilities {
    /// <summary>
    /// Queries used to pull data from the SQLite database. As a general rule, any variables that are used as values in
    /// the query (e.g. to customise the records returned when used with the database) should use the function GetParameterNameFromValue.
    /// This should be used "both ends", when the query string is built, and when the parameters are entered into the database
    /// before the query is ran. This ensures that the parameter names match up since they are both derived from the function using
    /// the same value. The function is part of the DbCommands class in the DbUtilities namespace.
    /// </summary>
    public class DbQueries {

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

        public static string GetQuestsDisplayQry() {
            return "SELECT * FROM Quests;";
        }

        public static string GetTasksDisplayQry(string questName) {
            return "SELECT * FROM QuestTasks WHERE QuestNames = " + DbCommands.GetParameterNameFromValue(questName) +";";
        }

        public static string GetEquipItemPartsRelatedToTask(string taskID) {
            return "SELECT QuestTaskParts.PartIDs, QuestTaskPartsEquipItem.ItemNames "
                + "FROM QuestTaskParts "
                + "LEFT JOIN QuestTaskPartsEquipItem ON QuestTaskPartsEquipItem.PartIDs = QuestTaskParts.PartIDs "
                + "WHERE TaskIDs = " + DbCommands.GetParameterNameFromValue(taskID) + ";";
        }

        public static string GetTaskPartOptionsEquipItemDisplayQry() {
            return "SELECT DISTINCT ItemNames FROM WorldItems;";
        }

        public static string GetDialogueDisplayQry() {
            return "SELECT Dialogues.DialogueIDs, Dialogues.DialogueDescriptions, ActivatedDialogues.SaveIDs " 
                + "FROM Dialogues LEFT JOIN ActivatedDialogues ON Dialogues.DialogueIDs = ActivatedDialogues.DialogueIDs "
                + "ORDER BY Dialogues.DialogueIDs ASC;";
        }

        public static string GetNewNodeChoiceResultQry(string dialogueID) {
            return "SELECT * FROM DialogueNodes WHERE DialogueIDs = " + dialogueID + ";";
        }

        public static string GetNewQuestChoiceResultQry() {
            return "SELECT * FROM Quests;";
        }

        public static string GetActivateQuestsPlayerChoiceResultQry(string choiceIDs) {
            return "SELECT QuestActivateResults.ResultIDs, Quests.QuestNames, Quests.QuestDescriptions FROM Quests "
                + "LEFT JOIN QuestActivateResults ON Quests.QuestNames = QuestActivateResults.QuestNames "
                + "WHERE ChoiceIDs = " + choiceIDs;
        }

        public static string GetQuestActivateCountFromChoiceIDqry(string choiceID) {
            return "SELECT COUNT(*) FROM QuestActivateResults "
                + "INNER JOIN PlayerChoiceResults ON QuestActivateResults.ResultIDs = PlayerChoiceResults.ResultIDs "
                + "WHERE PlayerChoiceResults.ChoiceIDs = " + choiceID + ";";
        }

        public static string GetCharLinkDisplayQry() {
            return "SELECT * FROM Characters WHERE CharacterNames != '!Player' ORDER BY CharacterNames ASC;";
        }

        public static string GetCharDialogueDisplayQry(string dialogueID) {
            return "SELECT * FROM CharacterDialogues WHERE DialogueIDs = "
                + dialogueID + ";";
        }

        public static string GetDialogueNodeDisplayQry(string dialogueID) {
            return "SELECT * FROM DialogueNodes WHERE DialogueIDs = "
               + dialogueID + ";";
        }

        public static string GetPlayerChoiceDisplayQry(string nodeID) {
            return "SELECT * FROM PlayerChoices WHERE NodeIDs = "
               + nodeID + ";";
        }

        public static string GetNextNodeResultQry(string choiceNextNodeID) {
            return "SELECT * FROM DialogueNodes WHERE NodeIDs = " + choiceNextNodeID + ";";
        }

        public static string GetDialogueResultsRelatingToChoice(string choiceID) {
            return "SELECT * FROM DialogueNodes WHERE NodeIDs = " + choiceID + ";";
        }

        public static string GetActivatedQuestsInCurrentGame() {
            return "SELECT * FROM QuestsActivated WHERE SaveIDs = 0;";
        }
    }
}