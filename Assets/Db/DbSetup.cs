using UnityEngine;
using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System.Text.RegularExpressions;

public class DbSetup : MonoBehaviour {
	private enum tbls 	{
						PlayerGames,	
						EnglishVocab,
						WelshVocab,
						VocabGrammar,
						VocabTranslations,
						DiscoveredVocab,
						DiscoveredGrammar,
						VocabRuleList,
						Proficiencies,
						AcquiredVocabSkills,
						AcquiredGrammarSkills,
						SentenceIdentifications,
						SentenceTranslations,
						SentenceGrammar,
						SentenceRulesList,
						DiscoveredSentences,
                        Dialogues,
                        Characters,
                        CharacterDialogues,
                        DialogueNodes,
                        PlayerChoices
						};
	private enum tblSqlStrs {header,body, pk};
	private int numberOfTbls = Enum.GetNames(typeof(tbls)).Length;
	public static string[,] tblSqlArray;
	string sql;								

	public static string conn;
	public static string DB_PATH = "/GameDb.s3db";

    void Awake () {
        conn = "URI=file:" + Application.dataPath + DB_PATH;

        tblSqlArray = new string[
                                    numberOfTbls,
                                    Enum.GetNames(typeof(tblSqlStrs)).Length
                                    ]; //size of array is the # tables by the sql string parts to build the tables


        SetTblSqlArray();
    }

    //initialization
    void Start() {

        //CreateTables();

        //ReplaceTable("CharacterDialogues");
        PrintTable("Characters");
        //CopyTable("DialogueNodes", "copied");
        //DropTable("DialogueNodes", false);
        //PrintTable("DialogueNodes");

        //string[,] fields = { { "CharacterNames", "Player" }};
        //DeleteTupleInTable("Characters", fields);

        //InsertTupleToTable("Characters", "!Player", "null");
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
                                                                                +   "LocationY REAL NOT NULL, ";
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

		tblSqlArray[(int)tbls.VocabTranslations, (int)tblSqlStrs.header]		= 	"VocabTranslations";
		tblSqlArray[(int)tbls.VocabTranslations, (int)tblSqlStrs.body]			= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+	"WelshText VARCHAR(140) NOT NULL, "
																				+	"FOREIGN KEY (EnglishText) "
																					+ "REFERENCES EnglishVocab(EnglishText) ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (WelshText) "
																					+ "REFERENCES WelshVocab(WelshText) ON DELETE CASCADE ON UPDATE CASCADE, ";
		tblSqlArray[(int)tbls.VocabTranslations, (int)tblSqlStrs.pk]			= 	"EnglishText, WelshText";

		tblSqlArray[(int)tbls.DiscoveredVocab, (int)tblSqlStrs.header]			= 	"DiscoveredVocab";
		tblSqlArray[(int)tbls.DiscoveredVocab, (int)tblSqlStrs.body]			= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+	"WelshText VARCHAR(140) NOT NULL, "
																				+	"SaveIDs INT NOT NULL, "
																				+	"CorrectTallies INT NOT NULL, "
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

		tblSqlArray[(int)tbls.AcquiredVocabSkills, (int)tblSqlStrs.header]		= 	"AcquiredVocabSkills";
		tblSqlArray[(int)tbls.AcquiredVocabSkills, (int)tblSqlStrs.body]		= 	"EnglishText VARCHAR(140) NOT NULL, "
																				+ 	"WelshText VARCHAR(140) NOT NULL, "
																				+	"ProficiencyNames VARCHAR(20) NOT NULL, "
																				+	"SaveIDs INT NOT NULL, "
																				+	"FOREIGN KEY (EnglishText, WelshText) "
																					+ "REFERENCES VocabTranslations(EnglishText,WelshText) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (ProficiencyNames) REFERENCES Proficiencies(ProficiencyNames) "
																					+ "ON DELETE CASCADE ON UPDATE CASCADE, "
																				+	"FOREIGN KEY (SaveIDs) REFERENCES PlayerGames(SaveIDs) ON DELETE CASCADE, ";
		tblSqlArray[(int)tbls.AcquiredVocabSkills, (int)tblSqlStrs.pk]			= 	"EnglishText, WelshText, ProficiencyNames, SaveIDs";

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
                                                                                + "DialogueDescriptions VARCHAR(200) NOT NULL, "
                                                                                + "Active INT NOT NULL, ";
        tblSqlArray[(int)tbls.Dialogues, (int)tblSqlStrs.pk]                    = "DialogueIDs";

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

        tblSqlArray[(int)tbls.PlayerChoices, (int)tblSqlStrs.header]            = "PlayerChoices";
        tblSqlArray[(int)tbls.PlayerChoices, (int)tblSqlStrs.body]              = "ChoiceIDs INT NOT NULL, "
                                                                                + "ChoiceText VARCHAR(500) NOT NULL, "
                                                                                + "NodeIDs INT NULL, "
                                                                                + "NextNodes INT, "
                                                                                + "FOREIGN KEY (NodeIDs) REFERENCES DialogueNodes(NodeIDs) ON DELETE CASCADE, "
                                                                                + "FOREIGN KEY (NextNodes) REFERENCES DialogueNodes(NodeIDs) ON DELETE CASCADE, ";
        tblSqlArray[(int)tbls.PlayerChoices, (int)tblSqlStrs.pk]                = "ChoiceIDs";

    }

    void ReplaceTable(string tblName) {
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

        PrintTable(tblName);

        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    void CopyTable(string tblNameTo, string tblNameFrom) {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();

        sql = "SELECT * FROM " + tblNameFrom;
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

        PrintTable(tblNameTo);

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
		_dbcm.ExecuteNonQuery();
		_dbcm.Dispose();
		_dbcm = null;
		_dbc.Close();
		_dbc = null;
	}

    void DropTable(string tblName, bool cascade = true) {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        if (cascade) {
            _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
            _dbcm.ExecuteNonQuery();
        }
        _dbcm.CommandText = "DROP TABLE IF EXISTS " + tblName + ";";
        _dbcm.ExecuteNonQuery();
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }


    void CreateTables() {
		for (int i = 0; i < numberOfTbls; i++) {
			CreateTable(i);
		}
	}

	public static void InsertTupleToTable (string tableName, params string[] values) {
		IDbConnection _dbc = new SqliteConnection(conn);
		_dbc.Open(); //Open connection to the database.
		IDbCommand _dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = "PRAGMA foreign_keys=ON;";
		_dbcm.ExecuteNonQuery();
		string sqlInsert = "INSERT OR IGNORE INTO " + tableName + " "
						 + "VALUES (";
		string sqlValues= "";
		for (int i = 0; i < values.Length; i++) {
            string value;
            if (values[i] != "null") {
                value = "@value" + i;
                _dbcm.Parameters.Add(new SqliteParameter(value, values[i]));
            } else { value = values[i]; }
            if (i == (values.Length - 1)) {
                sqlValues += (value + ");");
            }
            else {
                sqlValues += (value + ",");
            }   
            
		}
		_dbcm.CommandText = sqlInsert + sqlValues;
        print(sqlInsert + sqlValues);

        _dbcm.ExecuteNonQuery();
		_dbcm.Dispose();
		_dbcm = null;
		_dbc.Close();
		_dbc = null;


		//PrintTable (tableName);
	}

	public static void DeleteTupleInTable (string tableName, string[,] fields) {
		IDbConnection _dbc = new SqliteConnection(conn);
		_dbc.Open(); //Open connection to the database.
		IDbCommand _dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = "PRAGMA foreign_keys=ON;";
		_dbcm.ExecuteNonQuery();
		string sqlRead = "SELECT * FROM " + tableName + ";";
		_dbcm.CommandText = sqlRead;
		IDataReader _dbr = _dbcm.ExecuteReader();
		string sqlDelete = "DELETE FROM " + tableName + " ";
		string sqlValues = "WHERE ";
		for (int i = 0; i < fields.GetLength(0); i++) {
            string fieldValue = fields[i, 1];
            string fieldName = fields[i, 0];
            string value;
            if (fieldValue.ToLower() != "null") {
                value = "@value" + i;
                _dbcm.Parameters.Add(new SqliteParameter(value, fieldValue));
                value = " = " + value;
            } else { value = " IS " + fieldValue; }
			if (i == 0) {
				sqlValues += fieldName + value;
			} else {
				sqlValues += " AND " + fieldName + value;
			}	
		}
		sqlValues += ";";
		sqlDelete += sqlValues;
		//_dbcm.Cancel();
		_dbr.Dispose();
		_dbr = null;
        print(sqlDelete);
		_dbcm.CommandText = sqlDelete;
		_dbcm.ExecuteNonQuery();
		_dbcm.Dispose();
		_dbcm = null;
		_dbc.Close();
		_dbc = null;
	}

    public static void UpdateTableField(string tblName, string field, string value, string condition, params string[] qryParameters) {
        IDbConnection _dbcUpd = new SqliteConnection(conn);
        _dbcUpd.Open(); //Open connection to the database.
        IDbCommand _dbcmUpd = _dbcUpd.CreateCommand();
        string pValue = "null";
        if (value.ToLower() != "null") { 
            _dbcmUpd.CommandText = "PRAGMA foreign_keys=ON;";
            pValue = "@value";
            _dbcmUpd.ExecuteNonQuery();
        }
        string sql = "UPDATE " + tblName + " "
                    + "SET " + field + " = "
                    + pValue + " "
					+ "WHERE " + condition;
        if (pValue != "null") {
            _dbcmUpd.Parameters.Add(new SqliteParameter("@value", value));
        }
		foreach (string qryParameter in qryParameters) {
			_dbcmUpd.Parameters.Add(new SqliteParameter(GetParameterNameFromValue(qryParameter), qryParameter));
		}
		_dbcmUpd.CommandText = sql;
        print(sql);

		_dbcmUpd.ExecuteNonQuery();
		_dbcmUpd.Dispose();
		_dbcmUpd = null;
		_dbcUpd.Close();
		_dbcUpd = null;
	}

    public static void UpdateTableTuple(string tblName, string condition, string[,] fields) {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();
        string updateSql = "UPDATE " + tblName + " ";
        string setSql = "SET ";
        for (int i = 0; i < fields.GetLength(0); i++) {
            string fieldValue = fields[i, 1];
            string fieldValueParamName = GetParameterNameFromValue(fields[i, 1]);
            if (fieldValue == null || fieldValue == "" || fieldValue == "null") {
                fieldValue = "null";
                fieldValueParamName = "null";
            } else {
                _dbcm.Parameters.Add(new SqliteParameter(fieldValueParamName, fieldValue));
            }
            string fieldName = fields[i, 0];
            if ((i + 1) >= fields.GetLength(0)) {
                setSql += fieldName + " = " + fieldValueParamName;
            }
            else {
                setSql += fieldName + " = " + fieldValueParamName + ", ";
            }
            
        }

        string whereSql = " WHERE " + condition;
        string sql = updateSql + setSql + whereSql + ";";
        print(sql);
        
        _dbcm.CommandText = sql;

        _dbcm.ExecuteNonQuery();
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
        PrintTable(tblName);
    }



    public static void PrintTable (string tblName) {
		print ("TABLE NAME: " + tblName);
		IDbConnection _dbc = new SqliteConnection(conn);
		_dbc.Open(); //Open connection to the database.
		IDbCommand _dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = "PRAGMA foreign_keys=ON;";
		_dbcm.ExecuteNonQuery();
		string sql = "SELECT * FROM " + tblName;
		_dbcm.CommandText = sql;
		IDataReader _dbr = _dbcm.ExecuteReader();
		string fieldHeadings = "";
		for (int i = 0; i < _dbr.FieldCount; i++) {
			fieldHeadings += (_dbr.GetName(i)) + "\t";
		}
		print (fieldHeadings);

		while (_dbr.Read()) {
			string tupleStr = "";
			for (int i = 0; i < _dbr.FieldCount; i++) {
				tupleStr += (_dbr[_dbr.GetName(i)]) + "\t\t";
			}
			print (tupleStr);
		}
		_dbr.Dispose();
		_dbr = null;
		_dbcm.Dispose();
		_dbcm = null;
		_dbc.Close();
		_dbc = null;
	}

    public static string GenerateUniqueID(string tblName, string columnName,string pseudonym) {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();
        string sql = "SELECT MAX(" + columnName + ") AS \"" + pseudonym + "\" FROM " + tblName + ";";
        _dbcm.CommandText = sql;
        IDataReader _dbr = _dbcm.ExecuteReader();
        _dbr.Read();
        int uniqueID = 1;
        string ruleStr = _dbr[pseudonym].ToString();
        if (ruleStr != null && ruleStr != "") {
            uniqueID = int.Parse(ruleStr) + 1;
        }
        _dbr.Dispose();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
        return uniqueID.ToString();
    }

    public static int GetCountFromTable (string tblName, string condition = null, params string[] qryParameters) {
		IDbConnection _dbc = new SqliteConnection(conn);
		_dbc.Open(); //Open connection to the database.
		IDbCommand _dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = "PRAGMA foreign_keys=ON;";
		_dbcm.ExecuteNonQuery();
		string sql;
		sql = "SELECT COUNT(*) FROM " + tblName + " ";
		if (condition !=  null) {
			sql += "WHERE " + condition + ";";
		}
		foreach (string qryParameter in qryParameters) {
			_dbcm.Parameters.Add(new SqliteParameter(GetParameterNameFromValue(qryParameter), qryParameter));
		}
		_dbcm.CommandText = sql;
		int count = int.Parse(_dbcm.ExecuteScalar().ToString());
		_dbcm.Dispose();
		_dbcm = null;
		_dbc.Close();
		_dbc = null;
		return count;
	}

	public static string GetParameterNameFromValue (string value) {
		if (value == null) {
			value = "";
		}
		Regex rgx = new Regex("[^a-zA-Z0-9]");
		string ret = rgx.Replace(value, "");
		if (ret == "") {
			return null;
		} else { 
			return "@" + ret;
		}
	}

	public static string GetTranslationsDisplayQry () {
		return "SELECT * FROM VocabTranslations ORDER BY EnglishText ASC;";
	}

	public static string GetGrammarRuleDisplayQry (string englishStr = null, string welshStr = null) {
		string englishStrQry = "''";
		string welshStrQry = "''";
		if (englishStr != null) {
			englishStrQry =  englishStr;
		}
		if (welshStr != null) {
			welshStrQry =  welshStr;
		}
		return 	"SELECT VocabGrammar.RuleIDs, VocabGrammar.ShortDescriptions, " +
					"CASE WHEN VocabGrammar.RuleIDs = VocabRuleList.RuleIDs THEN 1 ELSE 0 END TranslationRules " +
				"FROM VocabGrammar " +
				"LEFT JOIN VocabRuleList ON VocabGrammar.RuleIDs = VocabRuleList.RuleIDs " +
				"AND VocabRuleList.EnglishText = " + englishStrQry + " AND VocabRuleList.WelshText = " + welshStrQry + " " +
					"ORDER BY VocabGrammar.RuleIDs ASC;";
	}

	public static string GetProficienciesDisplayQry () {
		return "SELECT * FROM Proficiencies ORDER BY Thresholds ASC;";
	}

	public static string GetTranslationSearchQry () {
		return "SELECT * FROM VocabTranslations WHERE EnglishText LIKE @searchText OR WelshText LIKE @searchText ORDER BY EnglishText ASC;";
	}

    public static string GetSaveGamesDisplayQry(bool autoSaveIncluded) {
        string whereStr = " WHERE SaveRefs != 'CurrentGame'";
        if (!autoSaveIncluded) {
            whereStr += " AND SaveRefs != 'Autosave'";
        }
        return "SELECT * FROM PlayerGames" + whereStr + ";";
    }

    public static string[] GetRandomTupleFromTable (string tblName) {
		IDbConnection _dbc = new SqliteConnection(conn);
		_dbc.Open(); //Open connection to the database.
		IDbCommand _dbcm = _dbc.CreateCommand();
		string sql;
		sql = "SELECT * FROM " + tblName + " ORDER BY RANDOM() LIMIT 1; ";
		_dbcm.CommandText = sql;
		IDataReader _dbr = _dbcm.ExecuteReader();
		_dbr.Read();
		string[] tupleArray = new string[_dbr.FieldCount];
		for (int i = 0; i < _dbr.FieldCount; i++) {
				tupleArray[i]= _dbr[_dbr.GetName(i)].ToString();
		}
		_dbr.Dispose();
		_dbr = null;
		_dbcm.Dispose();
		_dbcm = null;
		_dbc.Close();
		_dbc = null;
		return tupleArray;
	}

    public static string[] GetTupleFromTable(string tblName, string condition = "", string orderBy = "") {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        string sql;
        sql = "SELECT * FROM " + tblName;
        if (condition != "") {
            sql += " WHERE " + condition;
        }
        if (orderBy != "") {
            sql += " ORDER BY " + orderBy;
        }
        sql += " LIMIT 1;";
        _dbcm.CommandText = sql;
        IDataReader _dbr = _dbcm.ExecuteReader();
        _dbr.Read();
        string[] tupleArray = new string[_dbr.FieldCount];
        for (int i = 0; i < _dbr.FieldCount; i++) {
            tupleArray[i] = _dbr[_dbr.GetName(i)].ToString();
        }
        _dbr.Dispose();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
        return tupleArray;
    }

    public static string GetFieldValueFromTable(string tblName, string field, string condition = "") {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        string sql;
        sql = "SELECT * FROM " + tblName + " WHERE " + condition + " LIMIT 1;";
        print(sql);
        _dbcm.CommandText = sql;
        IDataReader _dbr = _dbcm.ExecuteReader();
        _dbr.Read();
        string fieldValue = _dbr[field].ToString();
        _dbr.Dispose();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
        return fieldValue;
    }

    public static string GetFieldValueFromQry(string qry, string field) {
        IDbConnection _dbc = new SqliteConnection(conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        string sql = qry + " LIMIT 1;";
        _dbcm.CommandText = sql;
        IDataReader _dbr = _dbcm.ExecuteReader();
        _dbr.Read();
        string fieldValue = _dbr[field].ToString();
        _dbr.Dispose();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
        return fieldValue;
    }


}
