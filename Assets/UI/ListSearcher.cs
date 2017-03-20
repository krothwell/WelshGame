
using UnityEngine;
using UnityEngine.UI;
using DbUtilities;

public class ListSearcher : UIController {
    float searchWait = 0f;
    bool searchCountDown = false;
    public GameObject listToSearch;
    InputField myInput;
    ListDisplayInfo searchInfo;
    string searchQry;
    // Use this for initialization
    void Start () {
        myInput = GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
        if (searchCountDown) {
            searchWait -= Time.deltaTime;
            if (searchWait <= 0) {
                SearchNow();
                searchCountDown = false;
            }
        }
    }

    public void Search() {
        if (searchInfo != null) {
            searchWait = 0.5f;
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
