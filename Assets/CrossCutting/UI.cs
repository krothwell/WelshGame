using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class UI : MonoBehaviour {

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FillDisplayFromDb(string query,
                                  Transform display,
                                  Func<IDataReader, Transform> buildItem,
                                  string search = null,
                                  params string[] qryParameters) {
        EmptyDisplay(display);
        IDbConnection _dbc = new SqliteConnection(DbSetup.conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();
        _dbcm.CommandText = query;
        if (search != null) {
            _dbcm.Parameters.Add(new SqliteParameter("@searchText", "%" + search + "%"));
        }
        foreach (string qryParameter in qryParameters) {
            _dbcm.Parameters.Add(new SqliteParameter(DbSetup.GetParameterNameFromValue(qryParameter), qryParameter));
            print(qryParameter);
        }
        print(query);
        IDataReader _dbr = _dbcm.ExecuteReader();
        while (_dbr.Read()) {
            Transform item = buildItem(_dbr);
            item.SetParent(display, false);
        }
        _dbr.Dispose();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    public void AppendDisplayWithTitle(Transform display, Transform titleTransform, string titleText) {
        titleTransform.GetComponentInChildren<Text>().text = titleText;
        titleTransform.SetParent(display, false);
    }

    public void AppendDisplayFromDb(string query, Transform display, Func<IDataReader, Transform> buildItem) {
        IDbConnection _dbc = new SqliteConnection(DbSetup.conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();
        _dbcm.CommandText = query;
        print(query);
        IDataReader _dbr = _dbcm.ExecuteReader();
        while (_dbr.Read()) {
            Transform item = buildItem(_dbr);
            item.SetParent(display, false);
        }
        _dbr.Dispose();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    public void FillDisplayFromTransform(Transform goHolder,
                                         Transform display,
                                         Func<GameObject, Transform> buildItem) {
        EmptyDisplay(display);
        foreach (Transform gObj in goHolder) {
            Transform item = buildItem(gObj.gameObject);
            item.SetParent(display, false);
        }
    }

    public void EmptyDisplay(Transform display) {
        foreach (Transform item in display) {
            Destroy(item.gameObject);
        }
    }

    public void DeselectIfClickingAnotherListItem(string itemName, GameObject go, Action deselectFunction) {
        /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
        if (Input.GetMouseButtonUp(0)) {
            SelectController.ClickSelect();
            if (SelectController.IsClickedGameObjectName(itemName) && SelectController.ClickedDifferentGameObjectTo(go)) {
                deselectFunction();
            }
        }
    }


}
