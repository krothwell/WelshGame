﻿using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using DbUtilities;
using UnityEngine;
public class DbSetup {
	private enum tbls 	{
						PlayerGames,	
						EnglishVocab,
						WelshVocab,
						VocabGrammar,
                        TranslationTags,
                        VocabTranslations,
                        VocabTagged,
                        DiscoveredVocab,
						DiscoveredGrammar,
						VocabRuleList,
						Proficiencies,
						AcquiredVocabReadSkills,
                        AcquiredVocabWriteSkills,
						AcquiredGrammarSkills,
						SentenceIdentifications,
						SentenceTranslations,
						SentenceGrammar,
						SentenceRulesList,
						DiscoveredSentences,
                        Dialogues,
                        ActivatedDialogues,
                        Characters,
                        CharacterDialogues,
                        DialogueNodes,
                        DialogueNodesVocabTests,
                        PlayerChoices,
                        PlayerChoicesVocabTests,
                        PlayerChoiceResults,
                        QuestsActivatedByDialogueChoices,
                        TasksActivatedByDialogueChoices,
                        TasksCompletedByDialogueChoices,
                        WelshVocabActivatedByDialogueChoices,
                        GrammarActivatedByDialogueChoices,
                        DialoguesActivatedByDialogueChoices,
                        Quests,
                        QuestsActivated,
                        QuestTasks,
                        QuestTasksActivated,
                        QuestTaskResults,
                        QuestTaskStartDialogueResults,
                        QuestTaskActivateDialogueResults,
                        QuestTaskEndCombatWithCharResults,
                        QuestTaskParts,
                        QuestTaskPartsPrefab,
                        QuestTaskPartsCompleteQuest,
                        QuestTaskPartsActivateDialogueNode,
                        CompletedQuestTaskParts,
                        QuestTaskPartsEquipItem,
                        QuestTaskPartsDefeatCharTagged,
                        SavedWorldItems,
                        PremadeWorldItems
						};

	private enum tblSqlStrs {header,body, pk};
	private int numberOfTbls = Enum.GetNames(typeof(tbls)).Length;
	public static string[,] tblSqlArray;
	string sql;								

	private static string conn;

    public DbSetup () {
        conn = DbCommands.GetConnectionString();
        tblSqlArray = new string[
                                    numberOfTbls,
                                    Enum.GetNames(typeof(tblSqlStrs)).Length
                                    ]; //size of array is the # tables by the sql string parts to build the tables


        SetTblSqlArray();
    }

	void SetTblSqlArray() {
		tblSqlArray[(int)tbls.PlayerGames, (int)tblSqlStrs.header] 				= 	"PlayerGames";
		tblSqlArray[(int)tbls.PlayerGames, (int)tblSqlStrs.body] 				= 	"SaveIDs INTEGER NOT NULL, "
																				+	"SaveRefs VARCHAR(50), "
																				+	"PlayerNames VARCHAR(50), "
                                                                                +   "PortraitImages VARCHAR(200), "
																				+   "Dates VARCHAR(50), "
                                                                                +   "LocationName VARCHAR(25), "
                                                                                +   "LocationX REAL NOT NULL, "
                                                                                +   "LocationY REAL NOT NULL, "
                                                                                +   "SkillPointsSpent INT,";
        tblSqlArray[(int)tbls.PlayerGames, (int)tblSqlStrs.pk] 					=	"SaveIDs"; 

		tblSqlArray[(int)tbls.EnglishVocab, (int)tblSqlStrs.header]				= 	"EnglishVocab";
		tblSqlArray[(int)tbls.EnglishVocab, (int)tblSqlStrs.body]				= 	"EnglishText VARCHAR(140) NOT NULL, ";
		tblSqlArray[(int)tbls.EnglishVocab, (int)tblSqlStrs.pk]					=	"EnglishText"; 

		tblSqlArray[(int)tbls.WelshVocab, (int)tblSqlStrs.header] 				= 	"WelshVocab";
		tblSqlArray[(int)tbls.WelshVocab, (int)tblSqlStrs.body] 				= 	"WelshText VARCHAR(140) NOT NULL, ";
		tblSqlArray[(int)tbls.WelshVocab, (int)tblSqlStrs.pk] 					= 	"WelshText";

		tblSqlArray[(int)tbls.VocabGrammar, (int)tblSqlStrs.header] 			= 	"VocabGrammar";
		tblSqlArray[(int)tbls.VocabGrammar, (int)tblSqlStrs.body]				= 	"RuleIDs INTEGER NOT NULL, "
																				+	"ShortDescriptions VARCHAR(100), "
                                                                                +   "LongDescriptions VARCHAR(500), ";
        tblSqlArray[(int)tbls.VocabGrammar, (int)tblSqlStrs.pk]					= 	"RuleIDs";

        tblSqlArray[(int)tbls.TranslationTags, (int)tblSqlStrs.header]          =   "TranslationTags";
        tblSqlArray[(int)tbls.TranslationTags, (int)tblSqlStrs.body]            =   "Tags VARCHAR(140) NOT NULL, ";
        tblSqlArray[(int)tbls.TranslationTags, (int)tblSqlStrs.pk]              =   "Tags";

        tblSqlArray[(int)tbls.VocabTranslations, (int)tblSqlStrs.header]		= 	"VocabTranslations";
		tblSqlArray[(int)tbls.VocabTranslations, (int)tblSqlStrs.body]			= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+	"WelshText VARCHAR(140) NOT NULL, "
																				+	"FOREIGN KEY (EnglishText) "
																					+ "REFERENCES EnglishVocab(EnglishText) ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (WelshText) "
																					+ "REFERENCES WelshVocab(WelshText) ON DELETE CASCADE ON UPDATE CASCADE, ";
		tblSqlArray[(int)tbls.VocabTranslations, (int)tblSqlStrs.pk]			= 	"EnglishText, WelshText";


        tblSqlArray[(int)tbls.VocabTagged, (int)tblSqlStrs.header]              =   "VocabTagged";
        tblSqlArray[(int)tbls.VocabTagged, (int)tblSqlStrs.body]                =   "Tags VARCHAR(140) NOT NULL, "
                                                                                +   "EnglishText VARCHAR(140) NOT NULL, "
                                                                                +   "WelshText VARCHAR(140) NOT NULL, "
                                                                                +   "FOREIGN KEY (Tags) "
                                                                                    + "REFERENCES TranslationTags(Tags) ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                +   "FOREIGN KEY (EnglishText, WelshText) "
                                                                                    + "REFERENCES VocabTranslations(EnglishText, WelshText) "
                                                                                    + "ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.VocabTagged, (int)tblSqlStrs.pk]                  =   "Tags, EnglishText, WelshText";

        tblSqlArray[(int)tbls.DiscoveredVocab, (int)tblSqlStrs.header]			= 	"DiscoveredVocab";
		tblSqlArray[(int)tbls.DiscoveredVocab, (int)tblSqlStrs.body]			= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+	"WelshText VARCHAR(140) NOT NULL, "
																				+	"SaveIDs INT NOT NULL, "
																				+	"ReadCorrectTallies INT NOT NULL, "
                                                                                +   "WriteCorrectTallies INT NOT NULL, "
                                                                                +	"FOREIGN KEY (EnglishText, WelshText) "
																					+ "REFERENCES VocabTranslations(EnglishText,WelshText) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, ";
		tblSqlArray[(int)tbls.DiscoveredVocab, (int)tblSqlStrs.pk]				= 	"EnglishText, WelshText, SaveIDs";

		tblSqlArray[(int)tbls.DiscoveredGrammar, (int)tblSqlStrs.header]		= 	"DiscoveredVocabGrammar";
		tblSqlArray[(int)tbls.DiscoveredGrammar, (int)tblSqlStrs.body]			= 	"RuleIDs INT NOT NULL, "
																				+	"SaveIDs INT NOT NULL, "
																				+	"CorrectTallies INT NOT NULL, "
																				+	"FOREIGN KEY (RuleIDs) REFERENCES VocabGrammar(RuleIDs) ON DELETE CASCADE, "
																				+	"FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, ";
		tblSqlArray[(int)tbls.DiscoveredGrammar, (int)tblSqlStrs.pk]			= 	"RuleIDs, SaveIDs";

		tblSqlArray[(int)tbls.VocabRuleList, (int)tblSqlStrs.header]			= 	"VocabRuleList";
		tblSqlArray[(int)tbls.VocabRuleList, (int)tblSqlStrs.body]				= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+	"WelshText VARCHAR(140) NOT NULL, "
																				+	"RuleIDs INT NOT NULL, "
																				+	"FOREIGN KEY (EnglishText, WelshText) "
																					+ "REFERENCES VocabTranslations(EnglishText, WelshText) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (RuleIDs) REFERENCES VocabGrammar(RuleIDs) ON DELETE CASCADE, ";
		tblSqlArray[(int)tbls.VocabRuleList, (int)tblSqlStrs.pk]				= 	"EnglishText, WelshText, RuleIDs";

		tblSqlArray[(int)tbls.Proficiencies, (int)tblSqlStrs.header]			= 	"Proficiencies";
		tblSqlArray[(int)tbls.Proficiencies, (int)tblSqlStrs.body]				= 	"ProficiencyNames VARCHAR(20) NOT NULL, "
																				+	"Thresholds INT NOT NULL, ";
		tblSqlArray[(int)tbls.Proficiencies, (int)tblSqlStrs.pk]				= 	"ProficiencyNames";

		tblSqlArray[(int)tbls.AcquiredVocabWriteSkills, (int)tblSqlStrs.header]	= 	"AcquiredVocabWriteSkills";
		tblSqlArray[(int)tbls.AcquiredVocabWriteSkills, (int)tblSqlStrs.body]	= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+ 	"WelshText VARCHAR(140) NOT NULL, "
																				+	"ProficiencyNames VARCHAR(20) NOT NULL, "
																				+	"SaveIDs INT NOT NULL, "
																				+	"FOREIGN KEY (EnglishText, WelshText) "
																					+ "REFERENCES VocabTranslations(EnglishText,WelshText) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (ProficiencyNames) REFERENCES Proficiencies(ProficiencyNames) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, ";
		tblSqlArray[(int)tbls.AcquiredVocabWriteSkills, (int)tblSqlStrs.pk]		= 	"EnglishText, WelshText, ProficiencyNames, SaveIDs";

        tblSqlArray[(int)tbls.AcquiredVocabReadSkills, (int)tblSqlStrs.header]  =   "AcquiredVocabReadSkills";
        tblSqlArray[(int)tbls.AcquiredVocabReadSkills, (int)tblSqlStrs.body]    =   "EnglishText VARCHAR(140) NOT NULL, "
                                                                                +   "WelshText VARCHAR(140) NOT NULL, "
                                                                                +   "ProficiencyNames VARCHAR(20) NOT NULL, "
                                                                                +   "SaveIDs INT NOT NULL, "
                                                                                +   "FOREIGN KEY (EnglishText, WelshText) "
                                                                                    + "REFERENCES VocabTranslations(EnglishText,WelshText) "
                                                                                    + "ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                +   "FOREIGN KEY (ProficiencyNames) REFERENCES Proficiencies(ProficiencyNames) "
                                                                                    + "ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                +   "FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.AcquiredVocabReadSkills, (int)tblSqlStrs.pk]      =   "EnglishText, WelshText, ProficiencyNames, SaveIDs";

        tblSqlArray[(int)tbls.AcquiredGrammarSkills, (int)tblSqlStrs.header]	= 	"AcquiredGrammarSkills";
		tblSqlArray[(int)tbls.AcquiredGrammarSkills, (int)tblSqlStrs.body]		= 	"RuleIDs INT NOT NULL, "
																				+	"ProficiencyNames VARCHAR(20) NOT NULL, "
																				+	"SaveIDs INT NOT NULL, "
																				+	"FOREIGN KEY (RuleIDs) REFERENCES VocabGrammar(RuleIDs) ON DELETE CASCADE, "
																				+	"FOREIGN KEY (ProficiencyNames) REFERENCES Proficiencies(ProficiencyNames) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, ";
		tblSqlArray[(int)tbls.AcquiredGrammarSkills, (int)tblSqlStrs.pk]		= 	"RuleIDs, ProficiencyNames, SaveIDs";

		tblSqlArray[(int)tbls.SentenceIdentifications, (int)tblSqlStrs.header]	= 	"SentenceIdentifications";
		tblSqlArray[(int)tbls.SentenceIdentifications, (int)tblSqlStrs.body]	= 	"SentenceIDs INT NOT NULL, ";
		tblSqlArray[(int)tbls.SentenceIdentifications, (int)tblSqlStrs.pk]		= 	"SentenceIDs";

		tblSqlArray[(int)tbls.SentenceTranslations, (int)tblSqlStrs.header]		= 	"SentenceTranslations";
		tblSqlArray[(int)tbls.SentenceTranslations, (int)tblSqlStrs.body]		= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+ 	"WelshText VARCHAR(140) NOT NULL, "
																				+	"SentenceIDs INT NOT NULL, "
																				+	"OrderIDs INT NOT NULL, "
																				+	"FOREIGN KEY (EnglishText, WelshText) "
																					+ "REFERENCES VocabTranslations(EnglishText, WelshText) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (SentenceIDs) "
																					+ "REFERENCES SentenceIdentifications(SentenceIDs) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, ";
		tblSqlArray[(int)tbls.SentenceTranslations, (int)tblSqlStrs.pk]			= 	"EnglishText, WelshText, SentenceIDs, OrderIDs";

		tblSqlArray[(int)tbls.SentenceGrammar, (int)tblSqlStrs.header]			= 	"SentenceGrammar";
		tblSqlArray[(int)tbls.SentenceGrammar, (int)tblSqlStrs.body]			= 	"RuleIDs INT NOT NULL, "
																				+	"Descriptions VARCHAR(200), ";
		tblSqlArray[(int)tbls.SentenceGrammar, (int)tblSqlStrs.pk]				= 	"RuleIDs";

		tblSqlArray[(int)tbls.SentenceRulesList, (int)tblSqlStrs.header]		= 	"SentenceRulesList";
		tblSqlArray[(int)tbls.SentenceRulesList, (int)tblSqlStrs.body]			= 	"RuleIDs INT NOT NULL, "
																				+	"SentenceIDs INT NOT NULL, "
																				+	"EnglishText VARCHAR(140) NOT NULL, "
																				+ 	"WelshText VARCHAR(140) NOT NULL, "
																				+	"OrderIDs INT NOT NULL, "
																				+	"FOREIGN KEY (RuleIDs) REFERENCES SentenceGrammar(RuleIDs) ON DELETE CASCADE, "
																				+	"FOREIGN KEY (EnglishText, WelshText, SentenceIDs, OrderIDs) "
																					+ "REFERENCES SentenceTranslations(EnglishText, WelshText, SentenceIDs, OrderIDs) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, ";
		tblSqlArray[(int)tbls.SentenceRulesList, (int)tblSqlStrs.pk]			= 	"RuleIDs, SentenceIDs, EnglishText, WelshText, OrderIDs";//fk referencing composite pk must use all values of pk

		tblSqlArray[(int)tbls.DiscoveredSentences, (int)tblSqlStrs.header]		= 	"DiscoveredSentences";
		tblSqlArray[(int)tbls.DiscoveredSentences, (int)tblSqlStrs.body]		= 	"SentenceIDs INT NOT NULL, "
																				+	"SaveIDs INT NOT NULL, "
																				+	"FOREIGN KEY (SentenceIDs) "
																					+ "REFERENCES SentenceIdentifications (SentenceIDs) ON DELETE CASCADE, "
																				+	"FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, ";
		tblSqlArray[(int)tbls.DiscoveredSentences, (int)tblSqlStrs.pk]			= 	"SentenceIDs, SaveIDs";

        //DIALOGUES
        tblSqlArray[(int)tbls.Dialogues, (int)tblSqlStrs.header]                = "Dialogues";
        tblSqlArray[(int)tbls.Dialogues, (int)tblSqlStrs.body]                  = "DialogueIDs INT NOT NULL, "
                                                                                + "DialogueDescriptions VARCHAR(200) NOT NULL, ";
        tblSqlArray[(int)tbls.Dialogues, (int)tblSqlStrs.pk]                    = "DialogueIDs";

        tblSqlArray[(int)tbls.ActivatedDialogues, (int)tblSqlStrs.header]       = "ActivatedDialogues";
        tblSqlArray[(int)tbls.ActivatedDialogues, (int)tblSqlStrs.body]         = "DialogueIDs INT NOT NULL, "
                                                                                + "SaveIDs INT NOT NULL, "
                                                                                + "Completed INT NOT NULL, "
                                                                                + "FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, "
                                                                                + "FOREIGN KEY (DialogueIDs) REFERENCES Dialogues(DialogueIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.ActivatedDialogues, (int)tblSqlStrs.pk]           = "DialogueIDs, SaveIDs";

        tblSqlArray[(int)tbls.Characters, (int)tblSqlStrs.header]               = "Characters";
        tblSqlArray[(int)tbls.Characters, (int)tblSqlStrs.body]                 = "CharacterNames VARCHAR(100) NOT NULL, "
                                                                                + "Scenes VARCHAR(100) NULL,";
        tblSqlArray[(int)tbls.Characters, (int)tblSqlStrs.pk]                   = "CharacterNames, Scenes";

        tblSqlArray[(int)tbls.CharacterDialogues, (int)tblSqlStrs.header]       = "CharacterDialogues";
        tblSqlArray[(int)tbls.CharacterDialogues, (int)tblSqlStrs.body]         = "CharacterNames VARCHAR(100) NULL, "
                                                                                + "Scenes VARCHAR(100) NULL,"
                                                                                + "DialogueIDs INT NOT NULL, "
                                                                                + "FOREIGN KEY (CharacterNames, Scenes) REFERENCES Characters(CharacterNames, Scenes) ON DELETE CASCADE, "
                                                                                + "FOREIGN KEY (DialogueIDs) REFERENCES Dialogues(DialogueIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.CharacterDialogues, (int)tblSqlStrs.pk]           = "CharacterNames, Scenes, DialogueIDs";

        tblSqlArray[(int)tbls.DialogueNodes, (int)tblSqlStrs.header]            = "DialogueNodes";
        tblSqlArray[(int)tbls.DialogueNodes, (int)tblSqlStrs.body]              = "NodeIDs INT NULL, "
                                                                                + "NodeText VARCHAR(500) NOT NULL, "
                                                                                + "DialogueIDs INT, "
                                                                                + "CharacterSpeaking VARCHAR(100) NULL, "
                                                                                + "Scenes VARCHAR(100) NULL, "
                                                                                + "EndDialogueOption INT, "
                                                                                + "FOREIGN KEY (CharacterSpeaking, Scenes) REFERENCES Characters(CharacterNames, Scenes) ON DELETE CASCADE, "
                                                                                + "FOREIGN KEY (DialogueIDs) REFERENCES Dialogues(DialogueIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.DialogueNodes, (int)tblSqlStrs.pk]                = "NodeIDs";

        tblSqlArray[(int)tbls.DialogueNodesVocabTests, (int)tblSqlStrs.header]  = "DialogueNodesVocabTests";
        tblSqlArray[(int)tbls.DialogueNodesVocabTests, (int)tblSqlStrs.body]    = "NodeIDs INT NULL, "
                                                                                + "EnglishText VARCHAR(140) NOT NULL, "
                                                                                + "WelshText VARCHAR(140) NOT NULL, "
                                                                                + "FOREIGN KEY (NodeIDs) "
                                                                                    + "REFERENCES DialogueNodes(NodeIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                                + "FOREIGN KEY (EnglishText, WelshText) "
                                                                                    + "REFERENCES VocabTranslations(EnglishText, WelshText) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.DialogueNodesVocabTests, (int)tblSqlStrs.pk]      = "NodeIDs, EnglishText, WelshText";

        tblSqlArray[(int)tbls.PlayerChoices, (int)tblSqlStrs.header]            = "PlayerChoices";
        tblSqlArray[(int)tbls.PlayerChoices, (int)tblSqlStrs.body]              = "ChoiceIDs INT NOT NULL, "
                                                                                + "ChoiceText VARCHAR(500) NOT NULL, "
                                                                                + "NodeIDs INT NULL, "
                                                                                + "NextNodes INT, "
                                                                                + "MarkDialogueCompleted INT NOT NULL, "
                                                                                + "FOREIGN KEY (NodeIDs) REFERENCES DialogueNodes(NodeIDs) ON DELETE CASCADE, "
                                                                                + "FOREIGN KEY (NextNodes) REFERENCES DialogueNodes(NodeIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.PlayerChoices, (int)tblSqlStrs.pk]                = "ChoiceIDs";

        tblSqlArray[(int)tbls.PlayerChoicesVocabTests, (int)tblSqlStrs.header] = "PlayerChoicesVocabTests";
        tblSqlArray[(int)tbls.PlayerChoicesVocabTests, (int)tblSqlStrs.body]   = "ChoiceIDs INT NOT NULL, "
                                                                               + "EnglishText VARCHAR(140) NOT NULL, "
                                                                               + "WelshText VARCHAR(140) NOT NULL, "
                                                                               + "FOREIGN KEY (ChoiceIDs) "
                                                                                    + "REFERENCES PlayerChoices(ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                               + "FOREIGN KEY (EnglishText, WelshText) "
                                                                                    + "REFERENCES VocabTranslations(EnglishText, WelshText) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.PlayerChoicesVocabTests, (int)tblSqlStrs.pk]     = "ChoiceIDs, EnglishText, WelshText";

        tblSqlArray[(int)tbls.PlayerChoiceResults, (int)tblSqlStrs.header]      = "PlayerChoiceResults";
        tblSqlArray[(int)tbls.PlayerChoiceResults, (int)tblSqlStrs.body]        = "ResultIDs INT NOT NULL, "
                                                                                + "ChoiceIDs INT NOT NULL, "
                                                                                + "FOREIGN KEY (ChoiceIDs) REFERENCES PlayerChoices(ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.PlayerChoiceResults, (int)tblSqlStrs.pk]          = "ResultIDs, ChoiceIDs";

        tblSqlArray[(int)tbls.QuestsActivatedByDialogueChoices, (int)tblSqlStrs.header] = "QuestActivateResults";
        tblSqlArray[(int)tbls.QuestsActivatedByDialogueChoices, (int)tblSqlStrs.body]   = "ResultIDs INT NOT NULL, "
                                                                                        + "ChoiceIDs INT NOT NULL, "
                                                                                        + "QuestNames VARCHAR(100) NOT NULL, "
                                                                                        + "FOREIGN KEY (ResultIDs, ChoiceIDs) "
                                                                                            + "REFERENCES PlayerChoiceResults(ResultIDs, ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                                        + "FOREIGN KEY (QuestNames) REFERENCES Quests(QuestNames) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestsActivatedByDialogueChoices, (int)tblSqlStrs.pk]     = "ResultIDs, ChoiceIDs, QuestNames";

        tblSqlArray[(int)tbls.TasksActivatedByDialogueChoices, (int)tblSqlStrs.header] = "TasksActivatedByDialogueChoices";
        tblSqlArray[(int)tbls.TasksActivatedByDialogueChoices, (int)tblSqlStrs.body]    = "ResultIDs INT NOT NULL, "
                                                                                        + "ChoiceIDs INT NOT NULL, "
                                                                                        + "TaskIDs INT, "
                                                                                        + "FOREIGN KEY (ResultIDs, ChoiceIDs) "
                                                                                            + "REFERENCES PlayerChoiceResults(ResultIDs, ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                                        + "FOREIGN KEY (TaskIDs) REFERENCES QuestTasks(TaskIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.TasksActivatedByDialogueChoices, (int)tblSqlStrs.pk]      = "ResultIDs, ChoiceIDs, TaskIDs";

        tblSqlArray[(int)tbls.TasksCompletedByDialogueChoices, (int)tblSqlStrs.header]  = "TasksCompletedByDialogueChoices";
        tblSqlArray[(int)tbls.TasksCompletedByDialogueChoices, (int)tblSqlStrs.body]    = "ResultIDs INT NOT NULL, "
                                                                                        + "ChoiceIDs INT NOT NULL, "
                                                                                        + "TaskIDs INT, "
                                                                                        + "FOREIGN KEY (ResultIDs, ChoiceIDs) "
                                                                                            + "REFERENCES PlayerChoiceResults(ResultIDs, ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                                        + "FOREIGN KEY (TaskIDs) REFERENCES QuestTasks(TaskIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.TasksCompletedByDialogueChoices, (int)tblSqlStrs.pk]      = "ResultIDs, ChoiceIDs, TaskIDs";

        tblSqlArray[(int)tbls.WelshVocabActivatedByDialogueChoices, (int)tblSqlStrs.header]  = "WelshVocabActivatedByDialogueChoices";
        tblSqlArray[(int)tbls.WelshVocabActivatedByDialogueChoices, (int)tblSqlStrs.body]    = "ResultIDs INT NOT NULL, "
                                                                                             + "ChoiceIDs INT NOT NULL, "
                                                                                             + "EnglishText VARCHAR(140) NOT NULL, "
                                                                                             + "WelshText VARCHAR(140) NOT NULL, "
                                                                                             + "FOREIGN KEY (ResultIDs, ChoiceIDs) "
                                                                                                + "REFERENCES PlayerChoiceResults(ResultIDs, ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                                             + "FOREIGN KEY (EnglishText, WelshText) "
                                                                                                + "REFERENCES VocabTranslations(EnglishText, WelshText) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.WelshVocabActivatedByDialogueChoices, (int)tblSqlStrs.pk]      = "ResultIDs, ChoiceIDs, EnglishText, WelshText";

        tblSqlArray[(int)tbls.GrammarActivatedByDialogueChoices, (int)tblSqlStrs.header]    = "GrammarActivatedByDialogueChoices";
        tblSqlArray[(int)tbls.GrammarActivatedByDialogueChoices, (int)tblSqlStrs.body]      = "ResultIDs INT NOT NULL, "
                                                                                            + "ChoiceIDs INT NOT NULL, "
                                                                                            + "RuleIDs INT NOT NULL, "
                                                                                            + "FOREIGN KEY (ResultIDs, ChoiceIDs) "
                                                                                                + "REFERENCES PlayerChoiceResults(ResultIDs, ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                                            + "FOREIGN KEY (RuleIDs) "
                                                                                                + "REFERENCES VocabGrammar(RuleIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.GrammarActivatedByDialogueChoices, (int)tblSqlStrs.pk]        = "ResultIDs, ChoiceIDs, RuleIDs";

        tblSqlArray[(int)tbls.DialoguesActivatedByDialogueChoices, (int)tblSqlStrs.header]  = "DialoguesActivatedByDialogueChoices";
        tblSqlArray[(int)tbls.DialoguesActivatedByDialogueChoices, (int)tblSqlStrs.body]    = "ResultIDs INT NOT NULL, "
                                                                                            + "ChoiceIDs INT NOT NULL, "
                                                                                            + "DialogueIDs INT NOT NULL, "
                                                                                            + "FOREIGN KEY (ResultIDs, ChoiceIDs) "
                                                                                                + "REFERENCES PlayerChoiceResults(ResultIDs, ChoiceIDs) ON DELETE CASCADE ON UPDATE CASCADE, "//fk referencing composite pk must use all values of pk
                                                                                            + "FOREIGN KEY (DialogueIDs) "
                                                                                                + "REFERENCES Dialogues(DialogueIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.DialoguesActivatedByDialogueChoices, (int)tblSqlStrs.pk]      = "ResultIDs, ChoiceIDs, DialogueIDs";

        tblSqlArray[(int)tbls.Quests, (int)tblSqlStrs.header]                   = "Quests";
        tblSqlArray[(int)tbls.Quests, (int)tblSqlStrs.body]                     = "QuestNames VARCHAR(100) NOT NULL, "
                                                                                + "QuestDescriptions VARCHAR(500) NULL, ";
        tblSqlArray[(int)tbls.Quests, (int)tblSqlStrs.pk]                       = "QuestNames";

        tblSqlArray[(int)tbls.QuestsActivated, (int)tblSqlStrs.header]          = "QuestsActivated";
        tblSqlArray[(int)tbls.QuestsActivated, (int)tblSqlStrs.body]            = "QuestNames VARCHAR(100) NOT NULL, "
                                                                                + "SaveIDs INT NOT NULL, "
                                                                                + "Completed INT NOT NULL, "
                                                                                + "FOREIGN KEY (QuestNames) REFERENCES Quests(QuestNames) ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                + "FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestsActivated, (int)tblSqlStrs.pk]              = "QuestNames, SaveIDs";

        tblSqlArray[(int)tbls.QuestTasks, (int)tblSqlStrs.header]               = "QuestTasks";
        tblSqlArray[(int)tbls.QuestTasks, (int)tblSqlStrs.body]                 = "TaskIDs INT, "
                                                                                + "TaskDescriptions VARCHAR(500) NULL, "
                                                                                + "QuestNames VARCHAR(100) NOT NULL, "
                                                                                + "FOREIGN KEY (QuestNames) REFERENCES Quests(QuestNames) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTasks, (int)tblSqlStrs.pk]                   = "TaskIDS";

        tblSqlArray[(int)tbls.QuestTaskResults, (int)tblSqlStrs.header]         = "QuestTaskResults";
        tblSqlArray[(int)tbls.QuestTaskResults, (int)tblSqlStrs.body]           = "ResultIDs INT NOT NULL, "
                                                                                + "TaskIDs INT, "
                                                                                + "FOREIGN KEY (TaskIDs) REFERENCES QuestTasks(TaskIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskResults, (int)tblSqlStrs.pk]             = "ResultIDs, TaskIDs";

        tblSqlArray[(int)tbls.QuestTaskStartDialogueResults, (int)tblSqlStrs.header]        = "QuestTaskStartDialogueResults";
        tblSqlArray[(int)tbls.QuestTaskStartDialogueResults, (int)tblSqlStrs.body]          = "ResultIDs INT NOT NULL, "
                                                                                            + "TaskIDs INT, "
                                                                                            + "DialogueIDs INT, "
                                                                                            + "FOREIGN KEY (ResultIDs, TaskIDs) REFERENCES QuestTaskResults(ResultIDs, TaskIDs) ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                            + "FOREIGN KEY (DialogueIDs) REFERENCES Dialogues(DialogueIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskStartDialogueResults, (int)tblSqlStrs.pk]            = "ResultIDs, TaskIDs, DialogueIDs";

        tblSqlArray[(int)tbls.QuestTaskActivateDialogueResults, (int)tblSqlStrs.header] = "QuestTaskActivateDialogueResults";
        tblSqlArray[(int)tbls.QuestTaskActivateDialogueResults, (int)tblSqlStrs.body]   = "ResultIDs INT NOT NULL, "
                                                                                        + "TaskIDs INT, "
                                                                                        + "DialogueIDs INT, "
                                                                                        + "FOREIGN KEY (ResultIDs, TaskIDs) REFERENCES QuestTaskResults(ResultIDs, TaskIDs) ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                        + "FOREIGN KEY (DialogueIDs) REFERENCES Dialogues(DialogueIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskActivateDialogueResults, (int)tblSqlStrs.pk]     = "ResultIDs, TaskIDs, DialogueIDs";

        tblSqlArray[(int)tbls.QuestTaskEndCombatWithCharResults, (int)tblSqlStrs.header] = "QuestTaskEndCombatWithCharResults";
        tblSqlArray[(int)tbls.QuestTaskEndCombatWithCharResults, (int)tblSqlStrs.body]   = "ResultIDs INT NOT NULL, "
                                                                                            + "TaskIDs INT, "
                                                                                            + "CharacterNames VARCHAR(100) NOT NULL, "
                                                                                            + "Scenes VARCHAR(100) NULL, "
                                                                                            + "FOREIGN KEY (ResultIDs, TaskIDs) REFERENCES QuestTaskResults(ResultIDs, TaskIDs) ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                            + "FOREIGN KEY (CharacterNames, Scenes) REFERENCES Characters(CharacterNames, Scenes) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskEndCombatWithCharResults, (int)tblSqlStrs.pk] = "ResultIDs, TaskIDs, CharacterNames, Scenes";

        tblSqlArray[(int)tbls.QuestTasksActivated, (int)tblSqlStrs.header]          = "QuestTasksActivated";
        tblSqlArray[(int)tbls.QuestTasksActivated, (int)tblSqlStrs.body]            = "TaskIDs INT, "
                                                                                    + "SaveIDs INT NOT NULL, "
                                                                                    + "Completed INT, "
                                                                                    + "FOREIGN KEY (TaskIDs) REFERENCES QuestTasks(TaskIDs) ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                    + "FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTasksActivated, (int)tblSqlStrs.pk]              = "TaskIDs, SaveIDs";

        tblSqlArray[(int)tbls.QuestTaskParts, (int)tblSqlStrs.header]           = "QuestTaskParts";
        tblSqlArray[(int)tbls.QuestTaskParts, (int)tblSqlStrs.body]             = "PartIDs INT, "
                                                                                + "TaskIDs INT, "
                                                                                + "FOREIGN KEY (TaskIDs) REFERENCES QuestTasks(TaskIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskParts, (int)tblSqlStrs.pk]               = "PartIDs";

        tblSqlArray[(int)tbls.CompletedQuestTaskParts, (int)tblSqlStrs.header]  = "CompletedQuestTaskParts";
        tblSqlArray[(int)tbls.CompletedQuestTaskParts, (int)tblSqlStrs.body]    = "PartIDs INT, "
                                                                                + "SaveIDs INT NOT NULL, "
                                                                                + "FOREIGN KEY (PartIDs) REFERENCES QuestTaskParts(PartIDs) ON DELETE CASCADE ON UPDATE CASCADE, "
                                                                                + "FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE ON UPDATE CASCADE, ";
        tblSqlArray[(int)tbls.CompletedQuestTaskParts, (int)tblSqlStrs.pk]      = "PartIDs, SaveIDs";

        tblSqlArray[(int)tbls.QuestTaskPartsEquipItem, (int)tblSqlStrs.header]  = "QuestTaskPartsEquipItem";
        tblSqlArray[(int)tbls.QuestTaskPartsEquipItem, (int)tblSqlStrs.body]    = "ItemNames VARCHAR(100) NOT NULL, "
                                                                                + "PartIDs INT, "
                                                                                + "FOREIGN KEY (PartIDs) REFERENCES QuestTaskParts(PartIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskPartsEquipItem, (int)tblSqlStrs.pk]      = "ItemNames, PartIDs";

        tblSqlArray[(int)tbls.QuestTaskPartsPrefab, (int)tblSqlStrs.header]     = "QuestTaskPartsPrefab";
        tblSqlArray[(int)tbls.QuestTaskPartsPrefab, (int)tblSqlStrs.body]       = "PrefabPath VARCHAR(200) NOT NULL, "
                                                                                + "PrefabLabel VarChar(140) NOT NULL, "
                                                                                + "PartIDs INT, "
                                                                                + "FOREIGN KEY (PartIDs) REFERENCES QuestTaskParts(PartIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskPartsPrefab, (int)tblSqlStrs.pk]         = "PrefabPath, PartIDs";

        tblSqlArray[(int)tbls.QuestTaskPartsCompleteQuest, (int)tblSqlStrs.header]  = "QuestTaskPartsCompleteQuest";
        tblSqlArray[(int)tbls.QuestTaskPartsCompleteQuest, (int)tblSqlStrs.body]    = "QuestNames VARCHAR(100) NOT NULL, "
                                                                                    + "PartIDs INT, "
                                                                                    + "FOREIGN KEY (PartIDs) REFERENCES QuestTaskParts(PartIDs) ON DELETE CASCADE, "
                                                                                    + "FOREIGN KEY (QuestNames) REFERENCES Quests(QuestNames) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskPartsCompleteQuest, (int)tblSqlStrs.pk]      = "QuestNames, PartIDs";

        tblSqlArray[(int)tbls.QuestTaskPartsActivateDialogueNode, (int)tblSqlStrs.header]   = "QuestTaskPartsActivateDialogueNode";
        tblSqlArray[(int)tbls.QuestTaskPartsActivateDialogueNode, (int)tblSqlStrs.body]     = "NodeIDs INT NULL, "
                                                                                            + "PartIDs INT, "
                                                                                            + "FOREIGN KEY (PartIDs) REFERENCES QuestTaskParts(PartIDs) ON DELETE CASCADE, "
                                                                                            + "FOREIGN KEY (NodeIDs) REFERENCES DialogueNodes(NodeIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskPartsActivateDialogueNode, (int)tblSqlStrs.pk]       = "NodeIDs, PartIDs";

        tblSqlArray[(int)tbls.QuestTaskPartsDefeatCharTagged, (int)tblSqlStrs.header]   = "QuestTaskPartsDefeatCharTagged";
        tblSqlArray[(int)tbls.QuestTaskPartsDefeatCharTagged, (int)tblSqlStrs.body]     = "CharacterTags VARCHAR(100) NULL,"
                                                                                        + "PartIDs INT, "
                                                                                        + "FOREIGN KEY (PartIDs) REFERENCES QuestTaskParts(PartIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.QuestTaskPartsDefeatCharTagged, (int)tblSqlStrs.pk]       = "CharacterTags, PartIDs";

        //NB: world items should never be in the same location at same time to avoid duplicate errors
        tblSqlArray[(int)tbls.SavedWorldItems, (int)tblSqlStrs.header]          = "SavedWorldItems";
        tblSqlArray[(int)tbls.SavedWorldItems, (int)tblSqlStrs.body]            = "SaveIDs, "
                                                                                + "LocationX INT, "
                                                                                + "LocationY INT, "
                                                                                + "LocationZ INT, "
                                                                                + "ParentPath VARCHAR(250), "
                                                                                + "SceneNames VARCHAR(50), "
                                                                                + "ItemNames VARCHAR(50), "
                                                                                + "PrefabPath VARCHAR(250), "
                                                                                + "FOREIGN KEY (SaveIDs) "
                                                                                    + "REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, "; 
        tblSqlArray[(int)tbls.SavedWorldItems, (int)tblSqlStrs.pk]              = "SaveIDs, LocationX, LocationY, LocationZ, ParentPath, SceneNames, ItemNames";

        tblSqlArray[(int)tbls.PremadeWorldItems, (int)tblSqlStrs.header]        = "PremadeWorldItems";
        tblSqlArray[(int)tbls.PremadeWorldItems, (int)tblSqlStrs.body]          = "StartingLocationX INT, "
                                                                                + "StartingLocationY INT, "
                                                                                + "StartingLocationZ INT, "
                                                                                + "StartingParentPath VARCHAR(250), "
                                                                                + "StartingSceneNames VARCHAR(50), "
                                                                                + "ItemNames VARCHAR(50), ";
        tblSqlArray[(int)tbls.PremadeWorldItems, (int)tblSqlStrs.pk]            = "StartingLocationX, StartingLocationY, StartingLocationZ, StartingParentPath, StartingSceneNames";
    }

    public void ReplaceTable(string tblName) {
        DropTable("copied", false);
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();
        string sql = "CREATE TABLE copied AS SELECT * FROM " + tblName;
        _dbcm.CommandText = sql;
        _dbcm.ExecuteNonQuery();
        DropTable(tblName, false);
        CreateTables();

        CopyTable(tblName, "copied");

        //PrintTable(tblName);

        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    public void CopyTable(string tblNameTo, string tblNameFrom) {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();

        sql = "SELECT * FROM " + tblNameTo;
        string copiedFieldNames = "";
        _dbcm.CommandText = sql;
        IDataReader _dbr = _dbcm.ExecuteReader();
        for (int i = 0; i < _dbr.FieldCount; i++) {
            if (i == _dbr.FieldCount - 1) {
                copiedFieldNames += (_dbr.GetName(i));
            }
            else { copiedFieldNames += (_dbr.GetName(i)) + ","; }
        }

        _dbr.Dispose();
        _dbr = null;

        sql = "INSERT INTO " + tblNameTo + "(" + copiedFieldNames + ") SELECT " + copiedFieldNames + " FROM " + tblNameFrom;

        _dbcm.CommandText = sql;
        _dbcm.ExecuteNonQuery();

        //PrintTable(tblNameTo);

        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    void CreateTable(int sqlArrayRow) {
		IDbConnection _dbc = new SqliteConnection(conn);
		_dbc.Open(); //Open connection to the database.
		IDbCommand _dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = "PRAGMA foreign_keys=ON;";
		_dbcm.ExecuteNonQuery();
		_dbcm.CommandText	= "CREATE TABLE IF NOT EXISTS " 	 
							+ tblSqlArray[sqlArrayRow, (int)tblSqlStrs.header] 	
							+ " (" 							
							+ tblSqlArray[sqlArrayRow, (int)tblSqlStrs.body] 
							+ "PRIMARY KEY (" + tblSqlArray[sqlArrayRow, (int)tblSqlStrs.pk] + ")"	
							+ ");";
        //Debug.Log(_dbcm.CommandText);
		_dbcm.ExecuteNonQuery();
		_dbcm.Dispose();
		_dbcm = null;
		_dbc.Close();
		_dbc = null;
	}

    public void DropTable(string tblName, bool cascade = true) {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        if (cascade) {
            _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
            _dbcm.ExecuteNonQuery();
        }
        //Debug.Log("DROP TABLE");
        string sql = "DROP TABLE IF EXISTS " + tblName + ";";
        Debug.Log(sql);
        _dbcm.CommandText = sql;
        _dbcm.ExecuteNonQuery();
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }


    public void CreateTables() {
		for (int i = 0; i < numberOfTbls; i++) {
			CreateTable(i);
		}
	}

}
