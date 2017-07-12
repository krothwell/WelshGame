using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using DataUI.ListItems;

namespace DataUI {
    public class DialogueNodeDetailsUI : UIController {
        Toggle endDialogueOptionToggle;
        public Toggle EndDialogueOptionToggle {
            get { return endDialogueOptionToggle; }
            set { endDialogueOptionToggle = value; }
        }
        DialogueUI dialogueUI;
        DialogueNodesListUI dialogueNodesListUI;
        GameObject displayDialogueNodeDetailsBtn, selectedCharOverride, nodeSpeakerBtn, selectNodeSpeakersUI, charOverrideList;
        public GameObject DialogueNodeSpeaker;
        InputField inputNodeText;
        Dropdown nodeTypeDropdown;
        public GameObject[] OptionComponentGroupArray;
        bool editing = false;
        public string NodeType;
        string overrideName, overrideScene;
        PlayerChoicesListUI playerChoicesListUI;
        // Use this for initialization
        void Start() {
            dialogueUI = FindObjectOfType<DialogueUI>();
            dialogueNodesListUI = FindObjectOfType<DialogueNodesListUI>();
            endDialogueOptionToggle = GetPanel().transform.GetComponentInChildren<Toggle>();
            //dialogueNodesList = transform.parent.Find("ScrollView").GetComponentInChildren<VerticalLayoutGroup>().gameObject;
            nodeTypeDropdown = GetPanel().GetComponentInChildren<Dropdown>();
            displayDialogueNodeDetailsBtn = transform.Find("ActivateNodeDetailsBtn").gameObject;
            inputNodeText = GetPanel().transform.GetComponentInChildren<InputField>();
            NodeType = "NodeType";
            CreateSelectionToggleGroup(NodeType);
            playerChoicesListUI = FindObjectOfType<PlayerChoicesListUI>();
            nodeSpeakerBtn = GetPanel().transform.Find("NodeSpeakerBtn").gameObject;
            selectNodeSpeakersUI = GetPanel().transform.Find("SelectNodeSpeakersUI").gameObject;
            charOverrideList = selectNodeSpeakersUI.transform.Find("Panel").GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        }

        public void ToggleChoiceComponents() {
            ToggleSelectionTo(OptionComponentGroupArray[nodeTypeDropdown.value].GetComponent<ISelectableUI>(), NodeType);
        }

        public void DeactivateNodeDetails() {
            GetPanel().SetActive(false);
            displayDialogueNodeDetailsBtn.GetComponent<Button>().interactable = true;
            displayDialogueNodeDetailsBtn.GetComponent<Text>().text = "New node";
            editing = false;
            nodeTypeDropdown.interactable = true;
        }

        public void ActivateNodeDetails() {
            nodeTypeDropdown.value = 0;
            ToggleChoiceComponents();
            GetPanel().SetActive(true);
            displayDialogueNodeDetailsBtn.GetComponent<Button>().interactable = false; //indicate to user that button no longer functions.
            if (editing) {
                print((dialogueNodesListUI.GetSelectedItemFromGroup(dialogueNodesListUI.SelectedNode) as DialogueNode));
                string[] nodeDesc = DbCommands.GetTupleFromTable("DialogueNodes",
                    "NodeIDs = " + (dialogueNodesListUI.GetSelectedItemFromGroup(dialogueNodesListUI.SelectedNode) as DialogueNode).GetComponent<DialogueNode>().MyID);
                inputNodeText.text = nodeDesc[1];
                bool endDialogueOptionBool = false;
                if (nodeDesc[5] != "") {
                    endDialogueOptionBool = (int.Parse(nodeDesc[5]) == 1) ? true : false;
                }
                endDialogueOptionToggle.isOn = endDialogueOptionBool;
                SetOverrideBtnTxt(nodeDesc[3], nodeDesc[4]);
            } else {
                ClearEditNodeDetails();
            }
        }

        private void ClearEditNodeDetails() {
            inputNodeText.text = "";
            endDialogueOptionToggle.isOn = false;
            SetOverrideBtnTxt("<i>None</i>", "");
        }

        public void SetCharOverrideDetails() {
            DialogueNodeSpeaker charOverride;
            if (selectedCharOverride != null) {
                charOverride = selectedCharOverride.GetComponent<DialogueNodeSpeaker>();
            }
            else {
                charOverride = null;
            }
            overrideName = (charOverride != null) ? charOverride.CharacterName : "null";
            overrideScene = (charOverride != null) ? charOverride.SceneName : "null";
        }


        public void UpdateInsertNewTextOnlyNode() {
            if (inputNodeText.text != null) {
                string endDialogueStr = endDialogueOptionToggle.isOn ? "1" : "0";
                SetCharOverrideDetails();
                if (editing) {
                    string[,] fieldVals = new string[,] {
                                                { "NodeText", inputNodeText.text },
                                                { "EndDialogueOption", endDialogueStr },
                                                { "CharacterSpeaking", overrideName },
                                                { "Scenes", overrideScene }
                                            };
                    DbCommands.UpdateTableTuple("DialogueNodes", "NodeIDs = " + (dialogueNodesListUI.GetSelectedItemFromGroup(dialogueNodesListUI.SelectedNode) as DialogueNode).MyID, fieldVals);
                    DialogueNodeTextOnly selectedNode = (dialogueNodesListUI.GetSelectedItemFromGroup(dialogueNodesListUI.SelectedNode) as DialogueNode).GetComponent<DialogueNodeTextOnly>();
                    print(selectedNode);
                    selectedNode.UpdateNodeDisplay(inputNodeText.text);
                }
                else {
                    string nodeID = DbCommands.GenerateUniqueID("DialogueNodes", "NodeIDs", "NodeID");
                    InsertDialogueNode(inputNodeText.text, nodeID, endDialogueStr);
                    dialogueNodesListUI.DisplayNodesRelatedToDialogue();
                    playerChoicesListUI.HidePlayerChoices();
                }
            }
        }

        public void SetCharacterOverride(GameObject charOverride) {
            selectedCharOverride = charOverride;
            string name = charOverride.GetComponent<DialogueNodeSpeaker>().CharacterName;
            string scene = charOverride.GetComponent<DialogueNodeSpeaker>().SceneName;
            SetOverrideBtnTxt(name, scene);
        }

        public void InsertDialogueNode(string nodeText, string nodeID, string endDialogueStr) {
            DbCommands.InsertTupleToTable("DialogueNodes",
                                            nodeID,
                                            nodeText,
                                            (dialogueUI.GetSelectedItemFromGroup(dialogueUI.selectedDialogue) as Dialogue).MyID,
                                            overrideName,
                                            overrideScene,
                                            endDialogueStr);
        }

        public void SetActiveNodeEdit() {
            displayDialogueNodeDetailsBtn.GetComponent<Text>().text = "Edit node";
            editing = true;
            nodeTypeDropdown.value = 0;
            nodeTypeDropdown.interactable = false;
            ActivateNodeDetails();
        }

        private void SetOverrideBtnTxt(string name, string scene) {
            string nameTxt = (name == "") ? "<i>None</i>" : name;
            string btnTxt = (scene != "") ? nameTxt + " <i>in " + scene + "</i>" : nameTxt;
            nodeSpeakerBtn.GetComponent<Text>().text = btnTxt;
        }


        public void ActivateNewCharacterOverride() {
            selectNodeSpeakersUI.transform.Find("Panel").gameObject.SetActive(true);
            nodeSpeakerBtn.GetComponent<Button>().interactable = false;
            string dialogueID = ((dialogueUI.GetSelectedItemFromGroup(dialogueUI.selectedDialogue)) as Dialogue).MyID;
            /*We only want to pick characters from scenes which are the same as those of the characters related to the dialogue
             * as a whole */
            string qry = "SELECT * FROM Characters "
                + "WHERE (Characters.Scenes IN (SELECT Scenes FROM CharacterDialogues "
                    + "WHERE DialogueIDs = " + dialogueID + ")) "
                + "OR (Characters.CharacterNames = '!Player') "
                + "ORDER BY CharacterNames ASC;";
            print(dialogueID);
            FillDisplayFromDb(qry, charOverrideList.transform, BuildDialogueNodeSpeaker);
        }

        public void DeactivateNewCharacterOverride() {
            selectNodeSpeakersUI.transform.Find("Panel").gameObject.SetActive(false);
            nodeSpeakerBtn.GetComponent<Button>().interactable = true;
        }

        private Transform BuildDialogueNodeSpeaker(string[] strArray) {
            string charName = strArray[0];
            string sceneName = strArray[1];
            GameObject charOverride = Instantiate(DialogueNodeSpeaker, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
            string sceneStr = " <i>in " + sceneName + "</i>";
            if (sceneName == "") {
                sceneStr = " <i>scene not found</i>";
            }
            charOverride.GetComponent<Text>().text = charName + sceneStr;
            charOverride.GetComponent<DialogueNodeSpeaker>().CharacterName = charName;
            charOverride.GetComponent<DialogueNodeSpeaker>().SceneName = sceneName;

            return charOverride.transform;
        }
    }
}

