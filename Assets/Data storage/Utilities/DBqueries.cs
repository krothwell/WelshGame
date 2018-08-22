
using UnityEngine;
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
                englishStrQry = (englishStrQry == null) ? "''" : englishStrQry;
            }
            if (welshStr != null) {
                welshStrQry = DbCommands.GetParameterNameFromValue(welshStr);
                welshStrQry = (welshStrQry == null) ? "''" : welshStrQry;
            }


            string sql = "SELECT VocabGrammar.RuleIDs, VocabGrammar.ShortDescriptions, " +
                        "CASE WHEN VocabGrammar.RuleIDs = VocabRuleList.RuleIDs THEN 1 ELSE 0 END TranslationRules " +
                    "FROM VocabGrammar " +
                    "LEFT JOIN VocabRuleList ON VocabGrammar.RuleIDs = VocabRuleList.RuleIDs " +
                    "AND VocabRuleList.EnglishText = " + englishStrQry + " AND VocabRuleList.WelshText = " + welshStrQry + " " +
                        "ORDER BY VocabGrammar.RuleIDs ASC;";
            Debug.Log(sql);
            return sql;
        }

        public static string GetTaggedVocabDisplayQry(string englishStr = null, string welshStr = null) {
            string englishStrQry = "''";
            string welshStrQry = "''";
            if (englishStr != null) {
                englishStrQry = DbCommands.GetParameterNameFromValue(englishStr);
                englishStrQry = (englishStrQry == null) ? "''" : englishStrQry;
            }
            if (welshStr != null) {
                welshStrQry = DbCommands.GetParameterNameFromValue(welshStr);
                welshStrQry = (welshStrQry == null) ? "''" : welshStrQry;
            }


            string sql = "SELECT TranslationTags.Tags, " +
                        "CASE WHEN TranslationTags.Tags = VocabTagged.Tags THEN 1 ELSE 0 END TranslationRules " +
                        "FROM TranslationTags " +
                        "LEFT JOIN VocabTagged ON TranslationTags.Tags = VocabTagged.Tags " +
                            "AND VocabTagged.EnglishText = " + englishStrQry + " " +
                            "AND VocabTagged.WelshText = " + welshStrQry + " " +
                        "ORDER BY TranslationTags.Tags ASC;";
            Debug.Log(sql);
            return sql;
        }

        public static string GetDiscoveredGrammarRelatedToVocab(string englishStr = null, string welshStr = null) {
            string englishStrQry = "''";
            string welshStrQry = "''";
            if (englishStr != null) {
                englishStrQry = DbCommands.GetParameterNameFromValue(englishStr);
                englishStrQry = (englishStrQry == null) ? "''" : englishStrQry;
            }
            if (welshStr != null) {
                welshStrQry = DbCommands.GetParameterNameFromValue(welshStr);
                welshStrQry = (welshStrQry == null) ? "''" : welshStrQry;
            }

            string sql = "SELECT VocabGrammar.RuleIDs, VocabGrammar.ShortDescriptions " +
                        "FROM VocabGrammar " +
                            "INNER JOIN DiscoveredVocabGrammar ON VocabGrammar.RuleIDs = DiscoveredVocabGrammar.RuleIDs " +
                                "AND DiscoveredVocabGrammar.SaveIDs = 0 " +
                            "INNER JOIN VocabRuleList ON VocabGrammar.RuleIDs = VocabRuleList.RuleIDs " +
                                "AND VocabRuleList.EnglishText = " + englishStrQry + " AND VocabRuleList.WelshText = " + welshStrQry + " " +
                        "ORDER BY VocabGrammar.RuleIDs ASC;";
            Debug.Log(sql);
            return sql;
        }

        public static string GetTranslationQry(string searchStr = "") {
            string qryStr = "SELECT * FROM VocabTranslations ";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = "WHERE EnglishText LIKE " + searchParam + " OR WelshText LIKE " + searchParam + " ORDER BY EnglishText ASC";
                qryStr += whereStr;
            }
            qryStr += ";";
            return qryStr;
        }

        public static string GetCharacterNamesWithScene(string sceneName) {
            string sceneParam = DbCommands.GetParameterNameFromValue(sceneName);
            return "SELECT CharacterNames FROM Characters WHERE scenes = " + sceneParam + ";";
        }

        public static string GetQuestsDisplayQry() {
            return "SELECT * FROM Quests;";
        }

        public static string GetTasksDisplayQry(string questName) {
            return "SELECT * FROM QuestTasks WHERE QuestNames = " + DbCommands.GetParameterNameFromValue(questName) + ";";
        }

        public static string GetEquipItemPartsRelatedToTask(string taskID) {
            return "SELECT QuestTaskParts.PartIDs, QuestTaskPartsEquipItem.ItemNames "
                + "FROM QuestTaskParts "
                + "INNER JOIN QuestTaskPartsEquipItem ON QuestTaskPartsEquipItem.PartIDs = QuestTaskParts.PartIDs "
                + "WHERE TaskIDs = " + DbCommands.GetParameterNameFromValue(taskID) + ";";
        }

        public static string GetPrefabPartsRelatedToTask(string taskID) {
            return "SELECT QuestTaskParts.PartIDs, QuestTaskPartsPrefab.PrefabLabel "
                + "FROM QuestTaskParts "
                + "INNER JOIN QuestTaskPartsPrefab ON QuestTaskPartsPrefab.PartIDs = QuestTaskParts.PartIDs "
                + "WHERE TaskIDs = " + DbCommands.GetParameterNameFromValue(taskID) + ";";
        }

        public static string GetDefeatCharTagPartsRelatedToTaskQry(string taskID) {
            return "SELECT QuestTaskParts.PartIDs, QuestTaskPartsDefeatCharTagged.CharacterTags FROM QuestTaskParts "
                + "INNER JOIN QuestTaskPartsDefeatCharTagged ON QuestTaskPartsDefeatCharTagged.PartIDs = QuestTaskParts.PartIDs "
                + "WHERE TaskIDs = " + DbCommands.GetParameterNameFromValue(taskID) + ";";
        }

        public static string GetTranslationsDisplayQry() {
            return "SELECT * FROM VocabTranslations ORDER BY EnglishText ASC;";
        }

        public static string GetProficienciesDisplayQry() {
            return "SELECT * FROM Proficiencies ORDER BY Thresholds ASC;";
        }

        public static string GetActivateDialoguePartsRelatedToTaskQry(string taskID) {
            return "SELECT " +
                    "QuestTaskParts.PartIDs, " +
                    "Dialogues.DialogueDescriptions, " +
                    "DialogueNodes.NodeText, " +
                    "QuestTaskPartsActivateDialogueNode.NodeIDs " +
                "FROM QuestTaskPartsActivateDialogueNode "
                + "INNER JOIN QuestTaskParts ON QuestTaskPartsActivateDialogueNode.PartIDs = QuestTaskParts.PartIDs "
                + "INNER JOIN DialogueNodes ON QuestTaskPartsActivateDialogueNode.NodeIDs = DialogueNodes.NodeIDs "
                + "INNER JOIN Dialogues ON DialogueNodes.DialogueIDs = Dialogues.DialogueIDs "
                + "WHERE QuestTaskParts.TaskIDs = " + DbCommands.GetParameterNameFromValue(taskID) + ";";
        }

        public static string GetActivateDialoguePartsRelatedToGameTaskQry(string taskID) {
            return "SELECT " +
                    "QuestTaskParts.PartIDs, " +
                    "CharacterDialogues.CharacterNames " +
                "FROM QuestTaskPartsActivateDialogueNode "
                + "INNER JOIN QuestTaskParts ON QuestTaskPartsActivateDialogueNode.PartIDs = QuestTaskParts.PartIDs "
                + "INNER JOIN DialogueNodes ON QuestTaskPartsActivateDialogueNode.NodeIDs = DialogueNodes.NodeIDs "
                + "INNER JOIN CharacterDialogues ON DialogueNodes.DialogueIDs = CharacterDialogues.DialogueIDs "
                + "WHERE QuestTaskParts.TaskIDs = " + DbCommands.GetParameterNameFromValue(taskID) + ";";
        }

        public static string GetCompleteQuestPartsRelatedToTaskQry(string taskID) {
            return "SELECT QuestTaskParts.PartIDs, QuestTaskPartsCompleteQuest.QuestNames, Quests.QuestDescriptions FROM QuestTaskPartsCompleteQuest "
                + "INNER JOIN QuestTaskParts ON QuestTaskParts.PartIDs = QuestTaskPartsCompleteQuest.PartIDs "
                + "INNER JOIN Quests ON Quests.QuestNames = QuestTaskPartsCompleteQuest.QuestNames "
                + "WHERE TaskIDs = " + DbCommands.GetParameterNameFromValue(taskID) + ";";
        }

        public static string GetTaskPartOptionsEquipItemDisplayQry() {
            return "SELECT DISTINCT ItemNames FROM PremadeWorldItems;";
        }

        public static string GetTaskPartOptionsCompleteQuestDisplayQry() {
            return "SELECT QuestNames, QuestDescriptions FROM Quests;";
        }

        public static string GetTaskPartOptionsActivateDialogueNodeDisplayQry(string searchStr = "") {
            string qryStr = "SELECT Dialogues.DialogueDescriptions, DialogueNodes.NodeText, DialogueNodes.NodeIDs FROM DialogueNodes "
                + "INNER JOIN Dialogues ON Dialogues.DialogueIDs = DialogueNodes.DialogueIDs";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE Dialogues.DialogueDescriptions LIKE " + searchParam +
                                  " OR DialogueNodes.NodeText LIKE " + searchParam +
                                  " OR DialogueNodes.NodeIDs LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += " ORDER BY Dialogues.DialogueIDs ASC;";
            return qryStr;
        }

        public static string GetDialogueDisplayQry(string searchStr = "") {
            string qryStr = "SELECT Dialogues.DialogueIDs, Dialogues.DialogueDescriptions, ActivatedDialogues.SaveIDs "
                + "FROM Dialogues LEFT JOIN ActivatedDialogues ON Dialogues.DialogueIDs = ActivatedDialogues.DialogueIDs AND ActivatedDialogues.SaveIDS = -1";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE Dialogues.DialogueDescriptions LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += " ORDER BY Dialogues.DialogueIDs ASC;";
            return qryStr;
        }

public static string GetNewNodeChoiceResultQry(string dialogueID) {
            return "SELECT * FROM DialogueNodes WHERE DialogueIDs = " + dialogueID + ";";
        }

        public static string GetNewQuestChoiceResultQry() {
            return "SELECT * FROM Quests;";
        }

        public static string GetCurrentActivateQuestsPlayerChoiceResultQry(string choiceIDs) {
            return "SELECT QuestActivateResults.ResultIDs, Quests.QuestNames, Quests.QuestDescriptions FROM Quests "
                + "LEFT JOIN QuestActivateResults ON Quests.QuestNames = QuestActivateResults.QuestNames "
                + "WHERE ChoiceIDs = " + choiceIDs;
        }

        public static string GetCurrentActivateTasksPlayerChoiceResultQry(string choiceID) {
            return "SELECT TasksActivatedByDialogueChoices.ResultIDs, QuestTasks.TaskIDs, QuestTasks.TaskDescriptions, QuestTasks.QuestNames "
                + "FROM QuestTasks "
                + "LEFT JOIN TasksActivatedByDialogueChoices ON QuestTasks.TaskIDs = TasksActivatedByDialogueChoices.TaskIDs "
                + "WHERE ChoiceIDs = " + choiceID + " "
                + "ORDER BY QuestTasks.QuestNames;";
        }

        public static string GetCurrentCompleteTasksPlayerChoiceResultQry(string choiceID) {
            return "SELECT TasksCompletedByDialogueChoices.ResultIDs, QuestTasks.TaskIDs, QuestTasks.TaskDescriptions, QuestTasks.QuestNames "
                + "FROM QuestTasks "
                + "LEFT JOIN TasksCompletedByDialogueChoices ON QuestTasks.TaskIDs = TasksCompletedByDialogueChoices.TaskIDs "
                + "WHERE ChoiceIDs = " + choiceID + " "
                + "ORDER BY QuestTasks.QuestNames;";
        }

        public static string GetCurrentActivateVocabPlayerChoiceResultQry(string choiceID) {
            return "SELECT WelshVocabActivatedByDialogueChoices.ResultIDs, WelshVocabActivatedByDialogueChoices.ChoiceIDs, WelshVocabActivatedByDialogueChoices.EnglishText, WelshVocabActivatedByDialogueChoices.WelshText "
                + "FROM WelshVocabActivatedByDialogueChoices "
                + "WHERE ChoiceIDs = " + choiceID + " "
                    + " AND (WelshVocabActivatedByDialogueChoices.EnglishText NOT IN (SELECT DiscoveredVocab.EnglishText FROM DiscoveredVocab WHERE DiscoveredVocab.SaveIDs = 0) "
                    + " OR WelshVocabActivatedByDialogueChoices.WelshText NOT IN (SELECT DiscoveredVocab.WelshText FROM DiscoveredVocab WHERE DiscoveredVocab.SaveIDs = 0)) "
                + "ORDER BY WelshVocabActivatedByDialogueChoices.EnglishText;";
        }

        public static string GetCurrentActivateGrammarPlayerChoiceResultQry(string choiceID) {
            return "SELECT GrammarActivatedByDialogueChoices.ResultIDs, VocabGrammar.RuleIDs, VocabGrammar.ShortDescriptions, VocabGrammar.LongDescriptions "
                + "FROM VocabGrammar "
                + "INNER JOIN GrammarActivatedByDialogueChoices ON GrammarActivatedByDialogueChoices.RuleIDs = VocabGrammar.RuleIDs "
                + "WHERE GrammarActivatedByDialogueChoices.ChoiceIDs = " + choiceID + " "
                    + " AND GrammarActivatedByDialogueChoices.RuleIDs NOT IN (SELECT DiscoveredVocabGrammar.RuleIDs FROM DiscoveredVocabGrammar WHERE DiscoveredVocabGrammar.SaveIDs = 0) "
                + "ORDER BY VocabGrammar.RuleIDs;";
        }

        public static string GetCurrentActivateDialoguePlayerChoiceResultQry(string choiceID) {
            return "SELECT DialoguesActivatedByDialogueChoices.ResultIDs, Dialogues.DialogueIDs, Dialogues.DialogueDescriptions "
                + "FROM Dialogues "
                + "INNER JOIN DialoguesActivatedByDialogueChoices ON DialoguesActivatedByDialogueChoices.DialogueIDs = Dialogues.DialogueIDs "
                + "WHERE DialoguesActivatedByDialogueChoices.ChoiceIDs = " + choiceID + " "
                + "ORDER BY Dialogues.DialogueIDs;";
        }

        public static string GetNewActivateTaskPlayerChoiceResultQry() {
            return "SELECT QuestTasks.TaskIDs, QuestTasks.TaskDescriptions, QuestTasks.QuestNames "
                + "FROM QuestTasks "
                + "WHERE QuestTasks.TaskIDs NOT IN (SELECT QuestTasksActivated.TaskIDs FROM QuestTasksActivated WHERE SaveIDs = -1);";
        }

        public static string GetNewActivateVocabPlayerChoiceResultQry(string searchStr = "") {
            string qryStr = "SELECT EnglishText, WelshText FROM VocabTranslations";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE EnglishText LIKE " + searchParam + " OR WelshText LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += ";";
            return qryStr;
        }

        public static string GetVocabQry(string searchStr = "") {
            string qryStr = "SELECT EnglishText, WelshText FROM VocabTranslations";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE EnglishText LIKE " + searchParam + " OR WelshText LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += ";";
            return qryStr;
        }

        public static string GetNewActivateGrammarPlayerChoiceResultQry(string searchStr = "") {
            string qryStr = "SELECT VocabGrammar.RuleIDs, VocabGrammar.ShortDescriptions, VocabGrammar.LongDescriptions "
                 + "FROM VocabGrammar";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE VocabGrammar.ShortDescriptions LIKE " + searchParam + " OR VocabGrammar.LongDescriptions LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += ";";
            return qryStr;
        }

        public static string GetNewActivateDialoguePlayerChoiceResultQry(string searchStr = "") {
            string qryStr = "SELECT Dialogues.DialogueIDs, Dialogues.DialogueDescriptions "
                 + "FROM Dialogues";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE Dialogues.DialogueDescriptions LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += ";";
            Debug.Log(qryStr);
            return qryStr;
        }

        public static string GetQuestActivateCountFromChoiceIDqry(string choiceID) {
            return "SELECT COUNT(*) FROM QuestActivateResults "
                + "INNER JOIN PlayerChoiceResults ON QuestActivateResults.ResultIDs = PlayerChoiceResults.ResultIDs "
                + "WHERE PlayerChoiceResults.ChoiceIDs = " + choiceID + ";";
        }

        public static string GetTaskActivateCountFromChoiceIDqry(string choiceID) {
            return "SELECT COUNT(*) FROM TasksActivatedByDialogueChoices "
                + "INNER JOIN PlayerChoiceResults ON TasksActivatedByDialogueChoices.ResultIDs = PlayerChoiceResults.ResultIDs "
                + "WHERE PlayerChoiceResults.ChoiceIDs = " + choiceID + ";";
        }

        public static string GetTaskCompleteCountFromChoiceIDqry(string choiceID) {
            return "SELECT COUNT(*) FROM TasksCompletedByDialogueChoices "
                + "INNER JOIN PlayerChoiceResults ON TasksCompletedByDialogueChoices.ResultIDs = PlayerChoiceResults.ResultIDs "
                + "WHERE PlayerChoiceResults.ChoiceIDs = " + choiceID + ";";
        }

        public static string GetPrefabTaskPartsFromTaskIDqry(string taskID) {
            string sql = "SELECT QuestTaskPartsPrefab.PrefabPath, QuestTaskPartsPrefab.PartIDs FROM QuestTasksActivated "
                + "INNER JOIN QuestTaskParts ON QuestTasksActivated.TaskIDs = QuestTaskParts.TaskIDs "
                + "INNER JOIN QuestTaskPartsPrefab ON QuestTaskParts.PartIDs = QuestTaskPartsPrefab.PartIDs "
                + "WHERE QuestTasksActivated.TaskIDs = " + taskID + ";";
            return sql;
        }

        public static string GetVocabActivateCountFromChoiceIDqry(string choiceID) {
            return "SELECT COUNT(*) FROM WelshVocabActivatedByDialogueChoices "
                + "INNER JOIN PlayerChoiceResults ON WelshVocabActivatedByDialogueChoices.ResultIDs = PlayerChoiceResults.ResultIDs "
                + "WHERE PlayerChoiceResults.ChoiceIDs = " + choiceID + ";";
        }

        public static string GetGrammarActivateCountFromChoiceIDqry(string choiceID) {
            return "SELECT COUNT(*) FROM GrammarActivatedByDialogueChoices "
                + "INNER JOIN PlayerChoiceResults ON GrammarActivatedByDialogueChoices.ResultIDs = PlayerChoiceResults.ResultIDs "
                + "WHERE PlayerChoiceResults.ChoiceIDs = " + choiceID + ";";
        }

        public static string GetDialogueActivateCountFromChoiceIDqry(string choiceID) {
            return "SELECT COUNT(*) FROM DialoguesActivatedByDialogueChoices "
                + "INNER JOIN PlayerChoiceResults ON DialoguesActivatedByDialogueChoices.ResultIDs = PlayerChoiceResults.ResultIDs "
                + "WHERE PlayerChoiceResults.ChoiceIDs = " + choiceID + ";";
        }

        public static string GetEquipItemTaskPartCountFromActiveQuests(string itemName, string saveID) {
            return "SELECT COUNT(QuestTaskPartsEquipItem.PartIDs) "
                + "FROM QuestTaskPartsEquipItem "
                + "INNER JOIN QuestTaskParts ON QuestTaskPartsEquipItem.PartIDs = QuestTaskParts.PartIDs "
                + "LEFT JOIN QuestTasks ON QuestTaskParts.TaskIDs = QuestTasks.TaskIDs "
                + "LEFT JOIN QuestsActivated ON QuestTasks.QuestNames = QuestsActivated.QuestNames "
                + "WHERE QuestTaskPartsEquipItem.ItemNames = " + DbCommands.GetParameterNameFromValue(itemName)
                    + " AND QuestsActivated.SaveIDs = " + saveID;
        }

        public static string GetTaskPartsCompleteFromTaskID(string taskID, string saveID) {
            return "SELECT COUNT(*) "
                + "FROM CompletedQuestTaskParts "
                + "INNER JOIN QuestTaskParts ON CompletedQuestTaskParts.PartIDs = QuestTaskParts.PartIDs "
                + "WHERE QuestTaskParts.TaskIDs = " + taskID
                    + " AND CompletedQuestTaskParts.SaveIDs = " + saveID;
        }

        public static string GetTasksCompleteFromQuestName(string questName, string saveID) {
            return "SELECT COUNT(*) "
                + "FROM QuestTasksActivated "
                + "INNER JOIN QuestTasks ON QuestTasksActivated.TaskIDs = QuestTasks.TaskIDs "
                + "WHERE QuestTasks.QuestNames = " + DbCommands.GetParameterNameFromValue(questName)
                    + " AND QuestTasksActivated.SaveIDs = " + saveID;
        }


        public static string GetEquipItemTasksData(string itemName, string saveID) {
            
            string sql = "SELECT DISTINCT QuestTaskPartsEquipItem.PartIDs, QuestTaskParts.TaskIDs, QuestTasks.QuestNames "
                + "FROM QuestTaskPartsEquipItem "
                + "INNER JOIN QuestTaskParts ON QuestTaskPartsEquipItem.PartIDs = QuestTaskParts.PartIDs "
                + "INNER JOIN QuestTasksActivated ON QuestTaskParts.TaskIDs = QuestTasksActivated.TaskIDs "
                + "INNER JOIN QuestTasks ON QuestTaskParts.TaskIDs = QuestTasks.TaskIDs "
                + "INNER JOIN QuestsActivated ON QuestTasks.QuestNames = QuestsActivated.QuestNames "
                + "WHERE QuestTaskPartsEquipItem.ItemNames = " + DbCommands.GetParameterNameFromValue(itemName)
                    + " AND QuestsActivated.SaveIDs = " + saveID
                    + " AND QuestTaskPartsEquipItem.PartIDs NOT IN (SELECT CompletedQuestTaskParts.PartIDs FROM CompletedQuestTaskParts WHERE CompletedQuestTaskParts.SaveIDs = 0)";
            return sql;
        }

        public static string GetDefeatEnemyTagTasksData(string tag, string saveID) {
            string sql = "SELECT DISTINCT QuestTaskPartsDefeatCharTagged.PartIDs, QuestTaskParts.TaskIDs, QuestTasks.QuestNames "
                + "FROM QuestTaskPartsDefeatCharTagged "
                + "INNER JOIN QuestTaskParts ON QuestTaskPartsDefeatCharTagged.PartIDs = QuestTaskParts.PartIDs "
                + "INNER JOIN QuestTasksActivated ON QuestTaskParts.TaskIDs = QuestTasksActivated.TaskIDs "
                + "INNER JOIN QuestTasks ON QuestTaskParts.TaskIDs = QuestTasks.TaskIDs "
                + "INNER JOIN QuestsActivated ON QuestTasks.QuestNames = QuestsActivated.QuestNames "
                + "WHERE QuestTaskPartsDefeatCharTagged.CharacterTags = " + DbCommands.GetParameterNameFromValue(tag)
                    + " AND QuestsActivated.SaveIDs = " + saveID
                    + " AND QuestTaskPartsDefeatCharTagged.PartIDs NOT IN (SELECT CompletedQuestTaskParts.PartIDs FROM CompletedQuestTaskParts WHERE CompletedQuestTaskParts.SaveIDs = 0)";
            return sql;
        }

        public static string GetCompleteQuestTasksData(string questName, string saveID) {
            string sql = "SELECT DISTINCT QuestTaskPartsCompleteQuest.PartIDs, QuestTaskParts.TaskIDs "
                + "FROM QuestTaskPartsCompleteQuest "
                + "INNER JOIN QuestTaskParts ON QuestTaskPartsCompleteQuest.PartIDs = QuestTaskParts.PartIDs "
                + "INNER JOIN QuestTasksActivated ON QuestTaskParts.TaskIDs = QuestTasksActivated.TaskIDs "
                + "WHERE QuestTaskPartsCompleteQuest.QuestNames = " + DbCommands.GetParameterNameFromValue(questName)
                    + " AND QuestsActivated.SaveIDs = " + saveID
                    + " AND QuestTaskPartsCompleteQuest.PartIDs NOT IN (SELECT CompletedQuestTaskParts.PartIDs FROM CompletedQuestTaskParts WHERE CompletedQuestTaskParts.SaveIDs = 0)";
            return sql;
        }


        public static string GetCharLinkDisplayQry() {
            return "SELECT * FROM Characters WHERE CharacterNames != '!Player' ORDER BY CharacterNames ASC;";
        }

        public static string GetCharDialogueDisplayQry(string dialogueID) {
            return "SELECT * FROM CharacterDialogues WHERE DialogueIDs = "
                + dialogueID + ";";
        }

        public static string GetCharsRelatedToDialogue(string dialogueID) {
            return "SELECT CharacterDialogues.CharacterNames FROM CharacterDialogues WHERE DialogueIDs = "
                + dialogueID + ";";
        }

        public static string GetActiveDialoguesWithCharacter(string charName) {
            string sql = "SELECT ActivatedDialogues.DialogueIDs FROM ActivatedDialogues " +
                   "INNER JOIN CharacterDialogues ON CharacterDialogues.DialogueIDs = ActivatedDialogues.DialogueIDs " +
                   "WHERE CharacterDialogues.CharacterNames = " + DbCommands.GetParameterNameFromValue(charName) + " " +
                    "AND ActivatedDialogues.Completed = 0 " +
                    "AND SaveIDs = 0;";
            Debug.Log(sql);
            return sql;
        }

        public static string GetDialogueNodeDisplayQry(string dialogueID) {
            return "SELECT * FROM DialogueNodes WHERE DialogueIDs = "
               + dialogueID + " AND DialogueNodes.NodeIDs NOT IN (SELECT DialogueNodesVocabTests.NodeIDs FROM DialogueNodesVocabTests);";
        }

        public static string GetDialogueNodeVocabDisplayQry(string dialogueID) {
            string sqlString = "SELECT DialogueNodes.NodeIDs, " +
                        "DialogueNodesVocabTests.EnglishText, DialogueNodesVocabTests.WelshText " +
                    "FROM DialogueNodes INNER JOIN DialogueNodesVocabTests " +
                        "ON DialogueNodes.NodeIDs = DialogueNodesVocabTests.NodeIDs " +
                    "WHERE DialogueNodes.DialogueIDs = " + dialogueID + ";";
            return sqlString;
        }

        public static string GetPlayerChoiceDisplayQry(string nodeID) {
            return "SELECT * FROM PlayerChoices WHERE NodeIDs = "
               + nodeID + " AND PlayerChoices.ChoiceIDs NOT IN (SELECT PlayerChoicesVocabTests.ChoiceIDs FROM PlayerChoicesVocabTests);";
        }

        public static string GetPlayerChoiceVocabDisplayQry(string nodeID) {
            return "SELECT PlayerChoices.ChoiceIDs, PlayerChoices.NextNodes, " +
                        "PlayerChoicesVocabTests.EnglishText, PlayerChoicesVocabTests.WelshText " +
                    "FROM PlayerChoices INNER JOIN PlayerChoicesVocabTests " +
                        "ON PlayerChoices.ChoiceIDs = PlayerChoicesVocabTests.ChoiceIDs " +
                    "WHERE PlayerChoices.NodeIDs = " + nodeID + ";";
        }

        public static string GetChoiceCompleteDialogueQry(string choiceID) {
            return "SELECT ChoiceIDs FROM PlayerChoices WHERE ChoiceIDs = "
               + choiceID + ";";
        }

        public static string GetNextNodeResultQry(string choiceNextNodeID) {
            return "SELECT * FROM DialogueNodes WHERE NodeIDs = " + choiceNextNodeID + ";";
        }

        public static string GetDialogueResultsRelatingToChoice(string choiceID) {
            return "SELECT * FROM DialogueNodes WHERE NodeIDs = " + choiceID + ";";
        }

        public static string GetActivatedQuestsInCurrentGame() {
            return "SELECT * FROM QuestsActivated WHERE SaveIDs = 0 ORDER BY Completed DESC;";
        }

        public static string GetTasksRelatedToQuest(string questName) {
            return "SELECT * FROM QuestTasks WHERE QuestNames = " + DbCommands.GetParameterNameFromValue(questName) + " "
                + "AND TaskIDs In (SELECT TaskIDs FROM QuestTasksActivated WHERE SaveIDs = 0)";
        }

        public static string GetPartsRelatedToTask(string taskID) {
            return "SELECT * FROM QuestTaskParts WHERE TaskIDs = " + taskID;
        }

        public static string GetSaveGamesDisplayQry(bool autoSaveIncluded) {
            string whereStr = " WHERE SaveIDs != 0 AND SaveIDs != -1";
            if (!autoSaveIncluded) {
                whereStr += " AND SaveIDs > -2 ";
            }
            return "SELECT * FROM PlayerGames" + whereStr + ";";
        }

        public static string GetTaskResultOptionsToStartDialogueQry(string searchStr = "") {
            string qryStr = "SELECT Dialogues.DialogueIDs, Dialogues.DialogueDescriptions, CharacterDialogues.CharacterNames "
                + "FROM Dialogues LEFT JOIN CharacterDialogues ON Dialogues.DialogueIDs = CharacterDialogues.DialogueIDs ";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE Dialogues.DialogueDescriptions LIKE " + searchParam + " OR CharacterDialogues.CharacterNames LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += " ORDER BY CharacterDialogues.CharacterNames;";
            return qryStr;
        }

        public static string GetTaskResultOptionsToEndCombatWithCharQry(string searchStr = "") {
            string qryStr = "SELECT Characters.CharacterNames, Characters.Scenes "
                + "FROM Characters ";
            if (searchStr != "") {
                string searchParam = DbCommands.GetParameterNameFromValue(searchStr);
                string whereStr = " WHERE Characters.CharacterNames LIKE " + searchParam + " OR Characters.Scenes LIKE " + searchParam;
                qryStr += whereStr;
            }
            qryStr += " ORDER BY CharacterNames;";
            return qryStr;
        }

        public static string GetStartDialogueResultsRelatedToTaskQry(string taskID) {
            return "SELECT " 
                + "QuestTaskStartDialogueResults.ResultIDs, Dialogues.DialogueIDs, Dialogues.DialogueDescriptions "
                + "FROM QuestTaskStartDialogueResults "
                    + "INNER JOIN Dialogues ON Dialogues.DialogueIDs = QuestTaskStartDialogueResults.DialogueIDs "
                + "WHERE QuestTaskStartDialogueResults.TaskIDs = " + taskID;
        }

        public static string GetActivateDialogueResultsRelatedToTaskQry(string taskID) {
            return "SELECT QuestTaskActivateDialogueResults.ResultIDs, Dialogues.DialogueIDs, Dialogues.DialogueDescriptions "
                + "FROM QuestTaskActivateDialogueResults INNER JOIN Dialogues ON Dialogues.DialogueIDs = QuestTaskActivateDialogueResults.DialogueIDs "
                + "WHERE TaskIDs = " + taskID;
        }

        public static string GetEndCombatWithCharResultsRelatedToTaskQry(string taskID) {
            return "SELECT QuestTaskEndCombatWithCharResults.ResultIDs, QuestTaskEndCombatWithCharResults.CharacterNames, QuestTaskEndCombatWithCharResults.Scenes "
                + "FROM QuestTaskEndCombatWithCharResults "
                + "WHERE QuestTaskEndCombatWithCharResults.TaskIDs = " + taskID;
        }

        public static string GetAllTaskResultsQry(string taskID) {
            return "SELECT DISTINCT "
                    + "QuestTaskEndCombatWithCharResults.CharacterNames, "
                    + "QuestTaskStartDialogueResults.DialogueIDs "
                + "FROM "
                    + "QuestTasks "
                    + "LEFT JOIN QuestTaskStartDialogueResults ON "
                        + "QuestTasks.TaskIDs = QuestTaskStartDialogueResults.TaskIDs "
                    + "LEFT JOIN QuestTaskEndCombatWithCharResults ON "
                        + "QuestTasks.TaskIDs = QuestTaskEndCombatWithCharResults.TaskIDs "
                + "WHERE "
                    + "QuestTasks.TaskIDs = " + taskID;
        }

        public static string GetSearchQry(string tableName, params string[] fieldNames) {
            string selectStr = "SELECT ";
            for (int i = 0; i < fieldNames.Length; i++) {
                string fieldName = fieldNames[i];
                if (i > 0) {
                    selectStr += ", " + fieldName;
                }
                else {
                    selectStr += fieldName;
                }
            }
            string fromStr = "FROM " + tableName + " ";
            return selectStr + fromStr;
        }

        public static string GetGrammarRelatedToVocab(string EnglishTxt, string WelshTxt) {
            return "SELECT VocabGrammar.ShortDescriptions, VocabGrammar.LongDescriptions "
                + "FROM VocabGrammar "
                + "INNER JOIN VocabRuleList ON VocabGrammar.RuleIDs = VocabRuleList.RuleIDs "
                + "WHERE VocabRuleList.EnglishText = " + DbCommands.GetParameterNameFromValue(EnglishTxt)
                    + " AND VocabRuleList.WelshText = " + DbCommands.GetParameterNameFromValue(WelshTxt)
                    + " AND VocabGrammar.RuleIDs IN (SELECT DiscoveredVocabGrammar.RuleIDs FROM DiscoveredVocabGrammar WHERE DiscoveredVocabGrammar.SaveIDs = 0)";
        }

        public static string GetPlayerVocabSkillsQry() {
            return "SELECT EnglishText, WelshText, ReadCorrectTallies, WriteCorrectTallies FROM DiscoveredVocab WHERE SaveIDs = 0";
        }

        public static string GetPlayerGrammarSkillsQry() {
            return "SELECT VocabGrammar.RuleIDs, VocabGrammar.ShortDescriptions, VocabGrammar.LongDescriptions, DiscoveredVocabGrammar.CorrectTallies " +
                "FROM DiscoveredVocabGrammar " +
                    "INNER JOIN VocabGrammar ON VocabGrammar.RuleIDs = DiscoveredVocabGrammar.RuleIDs " +
                "WHERE DiscoveredVocabGrammar.SaveIDs = 0";
        }

        public static string GetWelshThresholdsQry() {
            return "SELECT * FROM Proficiencies ORDER BY Thresholds ASC;";
        }

        public static string GetSavedWorldItemsQry(string saveID, string sceneName) {
            string sql = "SELECT LocationX, LocationY, LocationZ, ParentPath, ItemNames, PrefabPath, SaveIDs " +
                   "FROM SavedWorldItems " +
                   "WHERE SaveIDs = " + DbCommands.GetParameterNameFromValue(saveID) + " " +
                   "AND SceneNames = " + DbCommands.GetParameterNameFromValue(sceneName) + ";";
            //Debug.Log(sql);
            return sql;
        }

        public static string GetWriteSkillAcquiredForVocabQry(string en, string cy) {
            string sql = "SELECT ProficiencyNames " +
                   "FROM AcquiredVocabWriteSkills " +
                   "WHERE SaveIDs = 0 " +
                   "AND EnglishText = " + DbCommands.GetParameterNameFromValue(en) + " " +
                   "AND WelshText = " + DbCommands.GetParameterNameFromValue(cy);
            Debug.Log(sql);
            return sql;
        }

        public static string GetReadSkillAcquiredForVocabQry(string en, string cy) {
            string sql = "SELECT ProficiencyNames " +
                   "FROM AcquiredVocabReadSkills " +
                   "WHERE SaveIDs = 0 " +
                   "AND EnglishText = " + DbCommands.GetParameterNameFromValue(en) + " " +
                   "AND WelshText = " + DbCommands.GetParameterNameFromValue(cy);
            Debug.Log(sql);
            return sql;
        }

        public static string GetGrammarSkillAcquiredForVocabQry(string grammarID) {
            string sql = "SELECT ProficiencyNames " +
                   "FROM AcquiredGrammarSkills " +
                   "WHERE SaveIDs = 0 " +
                   "AND RuleIDs = " + grammarID + ";";
            Debug.Log(sql);
            return sql;
        }

        public static string GetPathsForActivePrefabQuestParts(string saveID) {
            string sql = "SELECT QuestTaskPartsPrefab.PrefabPath, QuestTaskParts.PartIDs, QuestTasks.TaskIDs, QuestTasks.QuestNames " +
                   "FROM QuestTaskPartsPrefab " +
                   "INNER JOIN QuestTaskParts ON QuestTaskParts.PartIDs = QuestTaskPartsPrefab.PartIDs " +
                   "INNER JOIN QuestTasksActivated ON QuestTasksActivated.TaskIDs = QuestTaskParts.TaskIDs " +
                   "INNER JOIN QuestTasks ON QuestTasks.TaskIDs = QuestTasksActivated.TaskIDs " +
                   "WHERE QuestTaskPartsPrefab.PartIDs " +
                    "NOT IN (SELECT CompletedQuestTaskParts.PartIDs FROM CompletedQuestTaskParts WHERE CompletedQuestTaskParts.SaveIDs = " + saveID + ") " +
                   "AND SaveIDs = " + saveID;
           // Debug.Log(sql);
            return sql;
        }
    }
}