using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using System.Text.RegularExpressions;
using UnityEngine; //Application.dataPath

namespace DbUtilities {

    /// <summary>
    /// A number of commands that build common SQL strings based on args 
    /// provided, and run them on the database.
    /// </summary>
    public class DbCommands {
        private static string conn;

        public static string GetConn() {
            string ret = conn;
            return ret;
        }

        public static void SetConnectionString(string connectionStr) {
            conn = connectionStr;
        }

        public static string GetConnectionString() {
            return conn;
        }

        public static void InsertTupleToTable(string tableName, params string[] values) {
            IDbConnection _dbc = new SqliteConnection(conn);
            _dbc.Open(); //Open connection to the database.
            IDbCommand _dbcm = _dbc.CreateCommand();
            _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
            _dbcm.ExecuteNonQuery();
            string sqlInsert = "INSERT OR IGNORE INTO " + tableName + " "
                             + "VALUES (";
            string sqlValues = "";
            for (int i = 0; i < values.Length; i++) {
                string value;
                if (values[i] != "null") {
                    value = "@value" + i;
                    _dbcm.Parameters.Add(new SqliteParameter(value, values[i]));
                }
                else { value = values[i]; }
                if (i == (values.Length - 1)) {
                    sqlValues += (value + ");");
                }
                else {
                    sqlValues += (value + ",");
                }

            }
            _dbcm.CommandText = sqlInsert + sqlValues;

            _dbcm.ExecuteNonQuery();
            _dbcm.Dispose();
            _dbcm = null;
            _dbc.Close();
            _dbc = null;


            //PrintTable (tableName);
        }

        public static void DeleteTupleInTable(string tableName, string[,] fields) {
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
                }
                else { value = " IS " + fieldValue; }
                if (i == 0) {
                    sqlValues += fieldName + value;
                }
                else {
                    sqlValues += " AND " + fieldName + value;
                }
            }
            sqlValues += ";";
            sqlDelete += sqlValues;
            //_dbcm.Cancel();
            _dbr.Dispose();
            _dbr = null;
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

            _dbcmUpd.ExecuteNonQuery();
            _dbcmUpd.Dispose();
            _dbcmUpd = null;
            _dbcUpd.Close();
            _dbcUpd = null;
        }

        public static void UpdateTableTuple(string tblName, string condition, string[,] fields, params string[] qryParameters) {
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
                }
                else {
                    _dbcm.Parameters.Add(new SqliteParameter(fieldValueParamName, fieldValue));
                }
                foreach (string value in qryParameters) {
                    _dbcm.Parameters.Add(new SqliteParameter(GetParameterNameFromValue(value), value));
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
            Debug.Log(sql);
            _dbcm.CommandText = sql;

            _dbcm.ExecuteNonQuery();
            _dbcm.Dispose();
            _dbcm = null;
            _dbc.Close();
            _dbc = null;
        }

        /// <summary>
        /// Returns a string array list of query data using the passed sql query string.
        /// The query should be written with the param values in mind using the GetParameterNameFromValue 
        /// to ensure that the query parameters match those generated using the same method here.
        /// </summary>
        /// <param name="sqlQry">Query to get records from Db</param>
        /// <param name="targetList">The list to be populated</param>
        /// <param name="paramValues">The values of the parameters</param>
        public static void GetDataStringsFromQry(string sqlQry, out List<string[]> targetList, params string[] paramValues) {
            targetList = new List<string[]>();
            IDbConnection _dbc = new SqliteConnection(conn);
            _dbc.Open(); //Open connection to the database.
            IDbCommand _dbcm = _dbc.CreateCommand();
            _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
            _dbcm.ExecuteNonQuery();
            _dbcm.CommandText = sqlQry;
            foreach (string qryParameter in paramValues) {
                _dbcm.Parameters.Add(new SqliteParameter(GetParameterNameFromValue(qryParameter), qryParameter));
            }
            IDataReader _dbr = _dbcm.ExecuteReader();
            while (_dbr.Read()) {
                string[] strArray = new string[_dbr.FieldCount];
                //column fields
                for (int i = 0; i < _dbr.FieldCount; i++) {
                    strArray[i] = (_dbr[_dbr.GetName(i)]).ToString();
                }
                //row
                targetList.Add(strArray);
            }
        }

        /// <summary>
        /// Provides array list of table data specified from the tblName parameter.
        /// </summary>
        public static void GetDbTableListToPrint(string tblName, out List<string[]> tblList) {
            tblList = new List<string[]>();
            string[] listName = new string[1];
            listName[0] = "TABLE NAME: " + tblName;
            tblList.Add(listName);
            IDbConnection _dbc = new SqliteConnection(conn);
            _dbc.Open(); //Open connection to the database.
            IDbCommand _dbcm = _dbc.CreateCommand();
            _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
            _dbcm.ExecuteNonQuery();
            string sql = "SELECT * FROM " + tblName;
            _dbcm.CommandText = sql;
            IDataReader _dbr = _dbcm.ExecuteReader();
            string[] fieldHeadings = new string[_dbr.FieldCount];
            for (int i = 0; i < _dbr.FieldCount; i++) {
                fieldHeadings[i] = (_dbr.GetName(i)).ToUpper();
                
            }
            tblList.Add(fieldHeadings);
            while (_dbr.Read()) {
                string[] tupleArray = new string[_dbr.FieldCount];
                for (int i = 0; i < _dbr.FieldCount; i++) {
                    tupleArray[i] = (_dbr[_dbr.GetName(i)].ToString());
                }
                tblList.Add(tupleArray);
            }
            _dbr.Dispose();
            _dbr = null;
            _dbcm.Dispose();
            _dbcm = null;
            _dbc.Close();
            _dbc = null;
        }

        public static string GenerateUniqueID(string tblName, string columnName, string pseudonym) {
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

        public static int GetCountFromTable(string tblName, string condition = null, params string[] qryParameters) {
            IDbConnection _dbc = new SqliteConnection(conn);
            _dbc.Open(); //Open connection to the database.
            IDbCommand _dbcm = _dbc.CreateCommand();
            _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
            _dbcm.ExecuteNonQuery();
            string sql;
            sql = "SELECT COUNT(*) FROM " + tblName + " ";
            if (condition != null) {
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

        public static string GetParameterNameFromValue(string value) {
            if (value == null) {
                value = "";
            }
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            string ret = rgx.Replace(value, "");
            if (ret == "") {
                return null;
            }
            else {
                return "@" + ret;
            }
        }

        public static string GetTranslationsDisplayQry() {
            return "SELECT * FROM VocabTranslations ORDER BY EnglishText ASC;";
        }

        public static string GetProficienciesDisplayQry() {
            return "SELECT * FROM Proficiencies ORDER BY Thresholds ASC;";
        }



        public static string GetSaveGamesDisplayQry(bool autoSaveIncluded) {
            string whereStr = " WHERE SaveRefs != 'CurrentGame'";
            if (!autoSaveIncluded) {
                whereStr += " AND SaveRefs != 'Autosave'";
            }
            return "SELECT * FROM PlayerGames" + whereStr + ";";
        }

        public static string[] GetRandomTupleFromTable(string tblName) {
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

        public static string[] GetTupleFromTable(string tblName, string condition = "", string orderBy = "", params string[] qryParameters) {
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
            //Debug.Log(sql);
            _dbcm.CommandText = sql;
            foreach (string qryParameter in qryParameters) {
                _dbcm.Parameters.Add(new SqliteParameter(GetParameterNameFromValue(qryParameter), qryParameter));
            }
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
            //print(sql);
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
}