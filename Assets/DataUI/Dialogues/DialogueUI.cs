using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;



public class DialogueUI : MonoBehaviour {
    UI ui;
    DataUI dataUI;
    private enum ChoiceResultOptions {
        DialogueNodes
    };
    //DIALOGUES
    DialogueListManager dialogueListManager;
    GameObject dialoguesPanel, dialogueList,
               submitNewDialogue, newDialoguePanel, activateNewDialogueBtn, //new dialogue components
               selectedDialogue;
    public GameObject dialoguePrefab;
    InputField inputShortDescriptionText; //new dialogue components
    Toggle dialogueActive;

    //CHARACTERS
    GameObject charPanel,
               characterList,
               newCharLink, newCharLinkPanel, newCharLinkList, newCharLinkBtn, selectedCharLink;
    public GameObject characterLinkPrefab, charDialoguePrefab;
    GameObject charListManager;

    //NODES
    GameObject dialogNodesListManager, dialogNodesPanel, dialogNodesScrollView, dialogNodesList,
               selectedNode,
               submitEditNewDialogNode, newDialogNodePanel, activateEditNewDialogNodeBtn,
               newCharOverridePanel, activateCharOverrideBtn, charOverrideList,
               selectedCharOverride;
    InputField inputNodeText;
    Toggle endDialogueOptionToggle;
    public GameObject dialogNodePrefab, charOverridePrefab;
    bool editingNode = false;

    //PLAYER CHOICES
    GameObject playerChoicesListManager, playerChoicesPanel, playerChoicesList,
               selectedChoice,
               submitEditNewPlayerChoice, newPlayerChoicePanel, activateEditNewPlayerChoiceBtn;
    InputField inputChoiceText;
    public GameObject playerChoicePrefab;
    bool editingChoice = false;

    //PLAYER CHOICE RESULTS
    GameObject pChoiceResultsListManager, pChoiceResultsPanel, pChoiceResultsList,
               selectedChoiceResult,
               submitNewChoiceResult, newChoiceResultPanel, activateNewChoiceResultBtn, newChoiceResultOptionList, selectedChoiceResultOption;
    ChoiceResultOptions choiceResultOptions;
    InputField inputResultText;
    public GameObject pChoiceResultTitlePrefab,
                      pChoiceResultPrefab,
                      optionChoiceResultPrefab;

    void Start() {
        UpdateCharactersTableFromGame();
        choiceResultOptions = new ChoiceResultOptions();
        
        ui = FindObjectOfType<UI>();
        dataUI = FindObjectOfType<DataUI>();

        //DIALOGUE COMPONENTS
        dialogueListManager = FindObjectOfType<DialogueListManager>();
        dialoguesPanel = dialogueListManager.transform.FindChild("DialoguesPanel").gameObject;
        dialogueList = dialoguesPanel.transform.FindChild("DialogueList").gameObject;
        //NEW DIALOGUE COMPONENTS
        submitNewDialogue = dialoguesPanel.transform.FindChild("SubmitNewDialog").gameObject;
        newDialoguePanel = submitNewDialogue.transform.FindChild("NewDialogPanel").gameObject;
        activateNewDialogueBtn = submitNewDialogue.transform.FindChild("ActivateNewDialogBtn").gameObject;
        inputShortDescriptionText = newDialoguePanel.transform.FindChild("InputShortDescriptionText").GetComponent<InputField>();
        dialogueActive = newDialoguePanel.transform.GetComponentInChildren<Toggle>();

        //LINKED CHARACTER COMPONENTS
        charListManager = transform.GetChild(0).FindChild("CharacterListManager").gameObject;
        charPanel = charListManager.transform.FindChild("CharactersPanel").gameObject;
        characterList = charPanel.transform.FindChild("CharacterList").gameObject;
        //LINK NEW CHARACTER COMPONENTS
        newCharLink = charPanel.transform.FindChild("LinkToCharacter").gameObject;
        newCharLinkPanel = newCharLink.transform.FindChild("NewCharLinkPanel").gameObject;
        newCharLinkList = newCharLinkPanel.transform.FindChild("CharacterList").gameObject;
        newCharLinkBtn = newCharLink.transform.FindChild("ActivateLinkCharBtn").gameObject;

        //NODE COMPONENTS
        dialogNodesListManager = transform.GetChild(0).FindChild("DialogNodesManager").gameObject;
        dialogNodesPanel = dialogNodesListManager.transform.FindChild("DialogNodesPanel").gameObject;
        dialogNodesScrollView = dialogNodesListManager.transform.GetComponentInChildren<ScrollRect>().gameObject;
        dialogNodesList = dialogNodesScrollView.transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        //NEW NODE COMPONENTS
        submitEditNewDialogNode = dialogNodesPanel.transform.FindChild("SubmitNewDialogNode").gameObject;
        newDialogNodePanel = submitEditNewDialogNode.transform.FindChild("NewDialogNodePanel").gameObject;
        activateEditNewDialogNodeBtn = submitEditNewDialogNode.transform.FindChild("ActivateNewDialogNodeBtn").gameObject;
        inputNodeText = newDialogNodePanel.transform.GetComponentInChildren<InputField>();
        endDialogueOptionToggle = newDialogNodePanel.transform.GetComponentInChildren<Toggle>();
        newCharOverridePanel = newDialogNodePanel.transform.FindChild("CharacterOverridePanel").gameObject;
        activateCharOverrideBtn = newDialogNodePanel.transform.FindChild("OverrideCharBtn").gameObject;
        charOverrideList = newCharOverridePanel.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

        //PLAYER CHOICE COMPONENTS
        playerChoicesListManager = transform.GetChild(0).FindChild("PlayerChoicesManager").gameObject;
        playerChoicesPanel = playerChoicesListManager.transform.FindChild("PlayerChoicesPanel").gameObject;
        playerChoicesList = playerChoicesPanel.transform.FindChild("PlayerChoicesList").gameObject;
        //NEW PLAYER CHOICE COMPONENTS
        submitEditNewPlayerChoice = playerChoicesPanel.transform.FindChild("SubmitEditNewPlayerChoice").gameObject;
        newPlayerChoicePanel = submitEditNewPlayerChoice.transform.FindChild("NewPlayerChoicePanel").gameObject;
        activateEditNewPlayerChoiceBtn = submitEditNewPlayerChoice.transform.FindChild("ActivateEditNewPlayerChoiceBtn").gameObject;
        inputChoiceText = newPlayerChoicePanel.transform.GetComponentInChildren<InputField>();

        //PLAYER CHOICE RESULTS COMPONENTS
        pChoiceResultsListManager = transform.GetChild(0).FindChild("PchoiceResultsManager").gameObject;
        pChoiceResultsPanel = pChoiceResultsListManager.transform.FindChild("PChoiceResultsPanel").gameObject;
        pChoiceResultsList = pChoiceResultsPanel.transform.FindChild("PChoiceResultsList").gameObject;
        //NEW PLAYER CHOICE RESULTS COMPONENTS
        submitNewChoiceResult = pChoiceResultsPanel.transform.FindChild("SubmitNewChoiceResult").gameObject;
        newChoiceResultPanel = submitNewChoiceResult.transform.FindChild("NewChoiceResultPanel").gameObject;
        newChoiceResultOptionList = newChoiceResultPanel.transform.FindChild("SelectedOptionList").gameObject;
        activateNewChoiceResultBtn = submitNewChoiceResult.transform.FindChild("ActivateNewChoiceResultBtn").gameObject;
        inputResultText = newChoiceResultPanel.transform.GetComponentInChildren<InputField>();

        //display dialogue list
        ui.FillDisplayFromDb(GetDialogueDisplayQry(), dialogueList.transform, BuildDialogue);
    }

    public void ActivateNewDialogue() {
        newDialoguePanel.SetActive(true);
        activateNewDialogueBtn.GetComponent<Text>().color = dataUI.colorDataUItxt; //indicate to user that button no longer functions.
    }

    public void ActivateCharacterLink() {
        newCharLinkPanel.SetActive(true);
        newCharLinkBtn.GetComponent<Text>().color = dataUI.colorDataUItxt; //indicate to user that button no longer functions.
        ui.FillDisplayFromDb(GetCharLinkDisplayQry(), newCharLinkList.transform, BuildCharacterLink);
    }

    public void ActivateEditNewNode() {
        newDialogNodePanel.SetActive(true);
        activateEditNewDialogNodeBtn.GetComponent<Text>().color = dataUI.colorDataUItxt; //indicate to user that button no longer functions.
        if (editingNode) {
            string[] nodeDesc = DbSetup.GetTupleFromTable("DialogueNodes",
                "NodeIDs = " + selectedNode.GetComponent<DialogNode>().MyID + ";");
            inputNodeText.text = nodeDesc[1];
            bool endDialogueOptionBool = false;
            if (nodeDesc[5] != "") {
                endDialogueOptionBool = (int.Parse(nodeDesc[5]) == 1) ? true : false;
            }
            print(endDialogueOptionToggle);
            endDialogueOptionToggle.isOn = endDialogueOptionBool;
            SetOverrideBtnTxt(nodeDesc[3],nodeDesc[4]);
        }
        else {
            ClearEditNodeDetails();
        }
        
    }

    private void ClearEditNodeDetails() {
        inputNodeText.text = "";
        endDialogueOptionToggle.isOn = false;
        SetOverrideBtnTxt("<i>None</i>", "");
    }

    public void ActivateNewCharacterOverride() {
        newCharOverridePanel.SetActive(true);
        activateCharOverrideBtn.GetComponent<Button>().interactable = false;
        string dialogueID = selectedDialogue.GetComponent<Dialogue>().MyID;
        /*We only want to pick characters from scenes which are the same as those of the characters related to the dialogue
         * as a whole */
        string qry = "SELECT * FROM Characters " 
            + "WHERE (Characters.Scenes IN (SELECT Scenes FROM CharacterDialogues "
                + "WHERE DialogueIDs = " + dialogueID + ")) "
            + "OR (Characters.CharacterNames = '!Player') "
            + "ORDER BY CharacterNames ASC;";
        ui.FillDisplayFromDb(qry, charOverrideList.transform, BuildCharOverride);
    }

    public void ActivateEditNewChoice() {
        newPlayerChoicePanel.SetActive(true);
        activateEditNewPlayerChoiceBtn.GetComponent<Text>().color = dataUI.colorDataUItxt; //indicate to user that button no longer functions.
        if (editingChoice) {
            string[] choiceDesc = DbSetup.GetTupleFromTable("PlayerChoices",
                "ChoiceIDs = " + selectedChoice.GetComponent<PlayerChoice>().MyID + ";");
            inputChoiceText.text = choiceDesc[1];
        }
    }

    public void ActivateNewChoiceResult() {
        newChoiceResultPanel.SetActive(true);
        activateNewChoiceResultBtn.GetComponent<Text>().color = dataUI.colorDataUItxt; //indicate to user that button no longer functions.
    }

    public void DeactivateNewDialogue() {
        newDialoguePanel.SetActive(false);
        inputShortDescriptionText.GetComponent<InputField>().text = "";
        activateNewDialogueBtn.GetComponent<Text>().color = dataUI.colorDataUIbtn;
    }

    public void DeactivateCharacterLink() {
        newCharLinkPanel.SetActive(false);
        newCharLinkBtn.GetComponent<Text>().color = dataUI.colorDataUIbtn;
    }

    public void DeactivateNewNode() {
        newDialogNodePanel.SetActive(false);
        activateEditNewDialogNodeBtn.GetComponent<Text>().color = dataUI.colorDataUIbtn;
        activateEditNewDialogNodeBtn.GetComponent<Text>().text = "New node";
        editingNode = false;
    }

    public void DeactivateNewCharacterOverride() {
        newCharOverridePanel.SetActive(false);
        activateCharOverrideBtn.GetComponent<Button>().interactable = true;
    }

    private void SetOverrideBtnTxt (string name, string scene) {
        string nameTxt = (name == "") ? "<i>None</i>" : name;
        string btnTxt = (scene != "") ? nameTxt + " <i>in " + scene + "</i>" : nameTxt;
        activateCharOverrideBtn.GetComponent<Text>().text = btnTxt;
    }

    public void DeactivateNewPlayerChoice() {
        newPlayerChoicePanel.SetActive(false);
        activateEditNewPlayerChoiceBtn.GetComponent<Text>().color = dataUI.colorDataUIbtn;
        activateEditNewPlayerChoiceBtn.GetComponent<Text>().text = "New choice";
    }

    public void DeactivateNewChoiceResult() {
        newChoiceResultPanel.SetActive(false);
        activateNewChoiceResultBtn.GetComponent<Text>().color = dataUI.colorDataUIbtn; //indicate to user that button no longer functions.
    }

    public void InsertNewDialogue() {
        if ((inputShortDescriptionText.text != null) && (inputShortDescriptionText.text != "")) {
            string dialogueActiveStr = dialogueActive.isOn ? "1" : "0";
            string dialogueID = DbSetup.GenerateUniqueID("Dialogues", "DialogueIDs", "DialogueID");
            DbSetup.InsertTupleToTable("Dialogues", dialogueID, inputShortDescriptionText.text, dialogueActiveStr);
            ui.FillDisplayFromDb(GetDialogueDisplayQry(), dialogueList.transform, BuildDialogue);
            HideCharsRelatedToDialogue();
            HideNodesRelatedToDialogue();
        }
    }

    public void InsertNewCharLink() {
        DbSetup.InsertTupleToTable("CharacterDialogues",
                                    selectedCharLink.GetComponent<CharacterLink>().CharacterName,
                                    selectedCharLink.GetComponent<CharacterLink>().SceneName,
                                    selectedDialogue.GetComponent<Dialogue>().MyID);
    }

    public void UpdateInsertNewNode() {
        string endDialogueStr = endDialogueOptionToggle.isOn ? "1" : "0";
        if (inputNodeText.text != null) {
            CharacterOverride charOverride;
            if (selectedCharOverride != null) {
                charOverride = selectedCharOverride.GetComponent<CharacterOverride>();
            }
            else {
                charOverride = null;
            }
            string overrideName = (charOverride != null) ? charOverride.CharacterName : "null";
            string overrideScene = (charOverride != null) ? charOverride.SceneName : "null";
            if (editingNode) {
                string[,] fieldVals = new string[,] {
                                                { "NodeText", inputNodeText.text },
                                                { "EndDialogueOption", endDialogueStr },
                                                { "CharacterSpeaking", overrideName },
                                                { "Scenes", overrideScene }
                                            };
                DbSetup.UpdateTableTuple("DialogueNodes", "NodeIDs = " + selectedNode.GetComponent<DialogNode>().MyID, fieldVals);
                selectedNode.GetComponent<DialogNode>().UpdateNodeDisplay(inputNodeText.text);
            }
            else {
                string nodeID = DbSetup.GenerateUniqueID("DialogueNodes", "NodeIDs", "NodeID");
                DbSetup.InsertTupleToTable("DialogueNodes",
                                        nodeID,
                                        inputNodeText.text,
                                        selectedDialogue.GetComponent<Dialogue>().MyID,
                                        overrideName,
                                        overrideScene,
                                        endDialogueStr);
                ui.FillDisplayFromDb(GetDialogueNodeDisplayQry(), dialogNodesList.transform, BuildDialogNode);
                HidePlayerChoices();
            }
        }
    }

    public void SetDialogueCharacterOverride(GameObject charOverride) {
        selectedCharOverride = charOverride;
        string name = charOverride.GetComponent<CharacterOverride>().CharacterName;
        string scene = charOverride.GetComponent<CharacterOverride>().SceneName;
        SetOverrideBtnTxt(name, scene);
    }

    public void UpdateInsertNewPlayerChoice() {
        if (inputChoiceText.text != null) {
            if (editingChoice) {
                string[,] fieldVals = new string[,] {
                                                { "ChoiceText", inputChoiceText.text },
                                            };
                DbSetup.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + selectedChoice.GetComponent<PlayerChoice>().MyID, fieldVals);
                selectedChoice.GetComponent<PlayerChoice>().UpdateChoiceDisplay(inputChoiceText.text);
            }
            else {
                string choiceID = DbSetup.GenerateUniqueID("PlayerChoices", "ChoiceIDs", "ChoiceID");
                DbSetup.InsertTupleToTable("PlayerChoices",
                                        choiceID,
                                        inputChoiceText.text,
                                        selectedNode.GetComponent<DialogNode>().MyID,
                                        null);
                ui.FillDisplayFromDb(GetPlayerChoiceDisplayQry(), playerChoicesList.transform, BuildPlayerChoice);
            }
        }

    }

    public void InsertNewChoiceResult() {
        switch (choiceResultOptions) {
            case ChoiceResultOptions.DialogueNodes:
                print("InsertNewChoiceResult");
                selectedChoice.GetComponent<PlayerChoice>().MyNextNode = selectedChoiceResultOption.GetComponent<PchoiceResultOption>().MyID;
                string[,] fieldVals = new string[,] {{ "NextNodes", selectedChoiceResultOption.GetComponent<PchoiceResultOption>().MyID }};
                DbSetup.UpdateTableTuple("PlayerChoices", "ChoiceIDs = " + selectedChoice.GetComponent<PlayerChoice>().MyID, fieldVals);
                break;
        }
        DisplayResultsRelatedToChoices();
        DeactivateNewChoiceResult();
    }

    private Transform BuildDialogue(IDataReader _dbr) {
        string descStr = (_dbr["DialogueDescriptions"].ToString());
        string idStr = (_dbr["DialogueIDs"].ToString());
        bool activeBool = ((int)(_dbr["Active"]) == 1) ? true:false;
        GameObject dialogue = Instantiate(dialoguePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        dialogue.transform.FindChild("DescriptionInput").GetComponent<InputField>().text = descStr;
        dialogue.transform.Find("DialogueID").GetComponent<Text>().text = idStr;
        dialogue.transform.GetComponentInChildren<Toggle>().isOn = activeBool;
        dialogue.GetComponent<Dialogue>().MyID = idStr;
        dialogue.GetComponent<Dialogue>().MyDescription = descStr;
        dialogue.GetComponent<Dialogue>().Active = activeBool;
        return dialogue.transform;
    }

    private Transform BuildCharacterLink(IDataReader _dbr) {
        string charName = (_dbr["CharacterNames"].ToString());
        string sceneName = (_dbr["Scenes"].ToString());
        GameObject charLink = Instantiate(characterLinkPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        string sceneStr = " <i>in " + sceneName + "</i>";
        if (sceneName == "") {
            sceneStr = " <i>scene not found</i>";
            charLink.GetComponent<CharacterLink>().DisplayDeletionOption();
        }
        charLink.transform.FindChild("CharacterName").GetComponent<Text>().text = charName + sceneStr;
        charLink.GetComponent<CharacterLink>().CharacterName = charName;
        charLink.GetComponent<CharacterLink>().SceneName = sceneName;

        return charLink.transform;
    }

    private Transform BuildCharacterDialogue(IDataReader _dbr) {
        string charName = (_dbr["CharacterNames"].ToString());
        string dialogueID = (_dbr["DialogueIDs"].ToString());
        string sceneName = (_dbr["Scenes"].ToString());
        print(sceneName);
        string sceneStr = " <i>scene not found</i>";
        if (sceneName != "") {
            sceneStr = " <i>in " + sceneName + "</i>";
        }
        GameObject charDialogue = Instantiate(charDialoguePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        charDialogue.transform.FindChild("CharacterInput").GetComponent<InputField>().text = charName + sceneStr;
        charDialogue.GetComponent<CharacterDialogue>().CharacterName = charName;
        charDialogue.GetComponent<CharacterDialogue>().DialogueID = dialogueID;
        charDialogue.GetComponent<CharacterDialogue>().SceneName = sceneName;
        return charDialogue.transform;
    }

    private Transform BuildDialogNode(IDataReader _dbr) {
        string nodeText = (_dbr["NodeText"].ToString());
        string idStr = (_dbr["NodeIDs"].ToString());
        GameObject dialogNode = Instantiate(dialogNodePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        dialogNode.transform.GetComponentInChildren<InputField>().text = nodeText; 
        dialogNode.transform.Find("NodeID").GetComponent<Text>().text = idStr;
        dialogNode.GetComponent<DialogNode>().MyID = idStr;
        dialogNode.GetComponent<DialogNode>().MyText = nodeText;
        return dialogNode.transform;
    }

    private Transform BuildCharOverride(IDataReader _dbr) {
        string charName = (_dbr["CharacterNames"].ToString());
        string sceneName = (_dbr["Scenes"].ToString());
        GameObject charOverride = Instantiate(charOverridePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        string sceneStr = " <i>in " + sceneName + "</i>";
        if (sceneName == "") {
            sceneStr = " <i>scene not found</i>";
        }
        charOverride.GetComponent<Text>().text = charName + sceneStr;
        charOverride.GetComponent<CharacterOverride>().CharacterName = charName;
        charOverride.GetComponent<CharacterOverride>().SceneName = sceneName;

        return charOverride.transform;
    }

    private Transform BuildPlayerChoice(IDataReader _dbr) {
        string choiceText = (_dbr["ChoiceText"].ToString());
        string idStr = (_dbr["ChoiceIDs"].ToString());
        string nextNode = (_dbr["NextNodes"].ToString());
        GameObject playerChoice = Instantiate(playerChoicePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        playerChoice.transform.GetComponentInChildren<InputField>().text = choiceText;
        playerChoice.transform.Find("ChoiceID").GetComponent<Text>().text = idStr;
        playerChoice.GetComponent<PlayerChoice>().MyID = idStr;
        playerChoice.GetComponent<PlayerChoice>().MyText = choiceText;
        playerChoice.GetComponent<PlayerChoice>().MyNextNode = nextNode;
        return playerChoice.transform;
    }

    private Transform BuildNewChoiceResultNode(IDataReader _dbr) {
        string nodeText = (_dbr["NodeText"].ToString());
        string idStr = (_dbr["NodeIDs"].ToString());
        GameObject optionChoiceResult = Instantiate(optionChoiceResultPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        optionChoiceResult.transform.GetComponentInChildren<InputField>().text = nodeText;
        optionChoiceResult.transform.Find("NodeID").GetComponent<Text>().text = idStr;
        optionChoiceResult.GetComponent<PchoiceResultOption>().MyID = idStr;
        optionChoiceResult.GetComponent<PchoiceResultOption>().MyText = nodeText;
        return optionChoiceResult.transform;
    }

    private Transform BuildChoiceResultNode(IDataReader _dbr) {
        string nodeText = (_dbr["NodeText"].ToString());
        string idStr = (_dbr["NodeIDs"].ToString());
        GameObject choiceResult = Instantiate(pChoiceResultPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        choiceResult.transform.GetComponentInChildren<InputField>().text = string.Format("Node ID <b>{0}</b>: \"{1}\"", idStr, nodeText);
        choiceResult.GetComponent<PchoiceResult>().MyID = idStr;
        choiceResult.GetComponent<PchoiceResult>().MyText = nodeText;
        return choiceResult.transform;
    }

    public void SetSelectedDialogue(GameObject dialogueInput) {
        selectedDialogue = dialogueInput;
    }

    public void SetSelectedCharLink(GameObject charlink) {
        selectedCharLink = charlink;
    }

    public void SetSelectedNode(GameObject node) {
        selectedNode = node;
    }

    public void SetSelectedPlayerChoice(GameObject choice) {
        selectedChoice = choice;
    }

    public void SetSelectedChoiceResultOption(GameObject option) {
        selectedChoiceResultOption = option;
    }

    public void SetSelectedPchoiceResult(GameObject result) {
        selectedChoiceResult = result;
    }

    public GameObject GetSelectedPlayerChoice() {
        return selectedChoice;
    }

    public void DisplayCharsRelatedToDialogue() {
        charListManager.SetActive(true);
        ui.FillDisplayFromDb(GetCharDialogueDisplayQry(), characterList.transform, BuildCharacterDialogue);
    }

    public void DisplayNodesRelatedToDialogue() {
        dialogNodesListManager.SetActive(true);
        ui.FillDisplayFromDb(GetDialogueNodeDisplayQry(), dialogNodesList.transform, BuildDialogNode);
    }

    public void DisplayChoicesRelatedToNode() {
        playerChoicesListManager.SetActive(true);
        ui.FillDisplayFromDb(GetPlayerChoiceDisplayQry(), playerChoicesList.transform, BuildPlayerChoice);
    }

    public void DisplayNewChoiceResultsNodes() {
        ui.FillDisplayFromDb(GetNewNodeChoiceResultQry(), newChoiceResultOptionList.transform, BuildNewChoiceResultNode);
        choiceResultOptions = ChoiceResultOptions.DialogueNodes;
    }

    public void DisplayResultsRelatedToChoices() {

        pChoiceResultsListManager.SetActive(true);
        ui.EmptyDisplay(pChoiceResultsList.transform);
        if (DbSetup.GetFieldValueFromTable("PlayerChoices", "NextNodes", " ChoiceIDs = " + selectedChoice.GetComponent<PlayerChoice>().MyID) != "") {
            print("DisplayChoiceResults");
            GameObject pChoiceResultsTitle = Instantiate(pChoiceResultTitlePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            ui.AppendDisplayWithTitle(pChoiceResultsList.transform, pChoiceResultsTitle.transform, "Goes to dialogue node... ");
            ui.AppendDisplayFromDb(GetNextNodeResultQry(), pChoiceResultsList.transform, BuildChoiceResultNode );
        }
    }

    public void HideCharsRelatedToDialogue() {
        charListManager.SetActive(false);
    }

    public void HideNodesRelatedToDialogue() {
        dialogNodesListManager.SetActive(false);
    }

    public void HidePlayerChoices() {
        playerChoicesListManager.SetActive(false);
    }

    public void HidePlayerChoiceResults() {
        pChoiceResultsListManager.SetActive(false);
    }

    public void SetActiveNodeEdit() {
        activateEditNewDialogNodeBtn.GetComponent<Text>().text = "Edit node";
        editingNode = true;
        ActivateEditNewNode();
    }

    public void SetActivePlayerChoiceEdit() {
        activateEditNewPlayerChoiceBtn.GetComponent<Text>().text = "Edit choice";
        editingChoice = true;
        ActivateEditNewChoice();
    }

    private string GetDialogueDisplayQry() {
        return "SELECT * FROM Dialogues ORDER BY DialogueIDs ASC;";
    }

    private string GetCharLinkDisplayQry() {
        return "SELECT * FROM Characters WHERE CharacterNames != '!Player' ORDER BY CharacterNames ASC;";
    }

    private string GetCharDialogueDisplayQry() {
        return "SELECT * FROM CharacterDialogues WHERE DialogueIDs = "
            + selectedDialogue.GetComponent<Dialogue>().MyID + ";";
    }

    private string GetDialogueNodeDisplayQry () {
        return "SELECT * FROM DialogueNodes WHERE DialogueIDs = "
           + selectedDialogue.GetComponent<Dialogue>().MyID + ";";
    }

    private string GetPlayerChoiceDisplayQry() {
        return "SELECT * FROM PlayerChoices WHERE NodeIDs = "
           + selectedNode.GetComponent<DialogNode>().MyID + ";";
    }

    private string GetNewNodeChoiceResultQry() {
        return "SELECT * FROM DialogueNodes WHERE DialogueIDs = " + selectedDialogue.GetComponent<Dialogue>().MyID + ";";
    }

    private string GetNextNodeResultQry() {
        return "SELECT * FROM DialogueNodes WHERE NodeIDs = " + selectedChoice.GetComponent<PlayerChoice>().MyNextNode + ";";
    }


    /*The characters from the game are updated so that you know which ones still exist in the scene
     * and are given the option to delete them when the list is built. New characters in the scene are also added */
    private void UpdateCharactersTableFromGame() {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        string currentScene = sceneLoader.GetCurrentSceneName();
        NPCs npcs = FindObjectOfType<NPCs>();
        foreach (Transform npc in npcs.transform) {
            string npcName = npc.gameObject.GetComponent<Character>().nameID;
            DbSetup.UpdateTableField(
                "Characters",
                "Scenes",
                currentScene,
                "CharacterNames = " + DbSetup.GetParameterNameFromValue(npcName),
                npcName);
            DbSetup.InsertTupleToTable("Characters", npcName, currentScene);
        }
        print("chars inserted");

        IDbConnection _dbc = new SqliteConnection(DbSetup.conn);
        _dbc.Open(); //Open connection to the database.
        IDbCommand _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "PRAGMA foreign_keys=ON;";
        _dbcm.ExecuteNonQuery();
        string sql = "SELECT CharacterNames FROM Characters WHERE scenes = @value;";
        _dbcm.Parameters.Add(new SqliteParameter("@value", currentScene));
        _dbcm.CommandText = sql;
        IDataReader _dbr = _dbcm.ExecuteReader();

        while (_dbr.Read()) {
            string charName = (_dbr["CharacterNames"].ToString());
            if(!CharacterInScene(charName)) {
                sql = "UPDATE Characters "
                        + "SET Scenes = null "
                        + "WHERE CharacterNames = @charName "
                        + "AND Scenes = @scene";
                IDbCommand _dbcm2 = _dbc.CreateCommand();
                _dbcm2.Parameters.Add(new SqliteParameter("@charName", charName));
                _dbcm2.Parameters.Add(new SqliteParameter("@scene", currentScene));
                _dbcm2.CommandText = sql;
                print(sql);
                _dbcm2.ExecuteNonQuery();
                //DbSetup.UpdateTableField(
                //    "Characters", 
                //    "Scenes", 
                //    "null",
                //    "CharacterNames = " + DbSetup.GetParameterNameFromValue(charName)
                //        + " AND Scenes = " + DbSetup.GetParameterNameFromValue(currentScene), 
                //    charName, currentScene);
            }
        }
        _dbr.Dispose();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;
    }

    private bool CharacterInScene(string characterID) {
        bool ret = false;
        NPCs npcs = FindObjectOfType<NPCs>();
        foreach (Transform npc in npcs.transform) {
            if (npc.gameObject.GetComponent<Character>().nameID  == characterID) {
                ret = true;
                break;
            }
        }
        print (characterID + " found in scene = " + ret.ToString());
        return ret;
    }

}
