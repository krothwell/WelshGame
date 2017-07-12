using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using DataUI.ListItems;

public class PlayerChoiceDetailsVocabTestUI : UIController, ISelectableUI {
    Transform list;
    ListSearcher listSearcher;
    ListDisplayInfo playerChoiceVocabListInfo;
    public GameObject VocabPlayerChoiceBtnPrefab;
    public void SelectSelf() {
        DisplayComponents();
        playerChoiceVocabListInfo = new ListDisplayInfo(
            DbQueries.GetVocabQry,
            BuildVocabPlayerChoice
        );
        list = GetComponentInChildren<VerticalLayoutGroup>().transform;
        FillDisplayFromDb(playerChoiceVocabListInfo.GetMyDefaultQuery(), list.transform, BuildVocabPlayerChoice);
        listSearcher = GetPanel().GetComponentInChildren<ListSearcher>();
        listSearcher.SetSearchInfo(playerChoiceVocabListInfo);
    }

    public void DeselectSelf() {
        HideComponents();
    }

    public Transform BuildVocabPlayerChoice(string[] strArray) {
        string engStr = (strArray[0]);
        string cymStr = (strArray[1]);
        PlayerChoiceVocabToTestBtn vocabPlayerChoiceBtn = Instantiate(VocabPlayerChoiceBtnPrefab, new Vector2(0f, 0f), Quaternion.identity).GetComponent<PlayerChoiceVocabToTestBtn>();
        vocabPlayerChoiceBtn.InitialiseMe(engStr, cymStr);
        return vocabPlayerChoiceBtn.transform;
    }
}
