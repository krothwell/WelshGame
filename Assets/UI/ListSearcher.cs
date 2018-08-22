
using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using System;

public class ListSearcher : UIController {
    DateTime searchWait;
    bool searchCountDown = false;
    public GameObject listToSearch;
    InputField myInput;
    ListDisplayInfo searchInfo;
    // Use this for initialization
    void Start () {
        myInput = GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
        if (searchCountDown) {
            //print(searchWait);
            if (DateTime.Now > searchWait) {
                SearchNow();
                searchCountDown = false;
            }
        }
    }

    public void Search() {
       if (searchInfo != null) {
            searchWait = DateTime.Now.AddMilliseconds(500);
            searchCountDown = true;
        }
    }


    private void SearchNow() {
        string searchText = "%" + myInput.text + "%";
        if (myInput.text == "") {
            FillDisplayFromDb(searchInfo.GetMyDefaultQuery(), listToSearch.transform, searchInfo.GetMyBuildMethod());
        }
        else {
            FillDisplayFromDb(searchInfo.GetMySearchQuery(searchText), listToSearch.transform, searchInfo.GetMyBuildMethod(), searchText);
        }
    }

    public void SetSearchInfo(ListDisplayInfo searchInfoObj) {
        ActivateSelf();
        searchInfo = searchInfoObj;
    }
}
