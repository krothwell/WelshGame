using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using DataUI.ListItems;

public class DialogueNodeDetailsVocabTestUI : UIController, ISelectableUI {
    Transform list;
    ListSearcher listSearcher;
    ListDisplayInfo dialogueNodeVocabListInfo;
    public GameObject VocabDialogueNodeBtnPrefab;
    public void SelectSelf() {
        DisplayComponents();
        dialogueNodeVocabListInfo = new ListDisplayInfo(
            DbQueries.GetVocabQry,
            BuildVocabPlayerChoice
        );
        list = GetComponentInChildren<VerticalLayoutGroup>().transform;
        FillDisplayFromDb(dialogueNodeVocabListInfo.GetMyDefaultQuery(), list.transform, BuildVocabPlayerChoice);
        listSearcher = GetPanel().GetComponentInChildren<ListSearcher>();
        listSearcher.SetSearchInfo(dialogueNodeVocabListInfo);
    }

    public void DeselectSelf() {
        HideComponents();
    }

    public Transform BuildVocabPlayerChoice(string[] strArray) {
        string engStr = (strArray[0]);
        string cymStr = (strArray[1]);
        DialogueNodeVocabToTestBtn vocabDialogueNodeBtn = Instantiate(VocabDialogueNodeBtnPrefab, new Vector2(0f, 0f), Quaternion.identity).GetComponent<DialogueNodeVocabToTestBtn>();
        vocabDialogueNodeBtn.InitialiseMe(engStr, cymStr);
        return vocabDialogueNodeBtn.transform;
    }
}
