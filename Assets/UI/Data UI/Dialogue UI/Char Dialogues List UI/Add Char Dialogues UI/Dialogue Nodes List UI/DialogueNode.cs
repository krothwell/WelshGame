using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;
using System.Collections;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class DialogueNode : MonoBehaviour {
            DialogueUI dialogueUI;
            InputField input;
            Image inputBG;
            GameObject options;

            private string myText;
            public string MyText {
                get { return myText; }
                set { myText = value; }
            }
            private string myID;
            public string MyID {
                get { return myID; }
                set { myID = value; }
            }

            // Use this for initialization
            void Start() {
                dialogueUI = FindObjectOfType<DialogueUI>();
                inputBG = transform.GetComponentInChildren<Image>();
                input = transform.GetComponentInChildren<InputField>();
                options = transform.FindChild("NodeOptions").gameObject;
            }

            void Update() {
                DeselectIfClickingAnotherNode();
            }

            void DeselectIfClickingAnotherNode() {
                /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
                if (Input.GetMouseButtonUp(0)) {
                    MouseSelection.ClickSelect();
                    if (MouseSelection.IsClickedGameObjectName("DialogNode") && MouseSelection.ClickedDifferentGameObjectTo(gameObject)) {
                        DeselectNode();
                    }
                }
            }

            void OnMouseUp() {
                SelectNode();
            }

            public void SelectNode() {
                DisplayOptions();
                SetMyColour(Colours.colorDataUIInputSelected);
                dialogueUI.SetSelectedNode(gameObject);
                dialogueUI.DisplayChoicesRelatedToNode();
                dialogueUI.HidePlayerChoiceResults();
            }

            private void DeselectNode() {
                HideOptions();
                SetMyColour(Color.white);
                input.readOnly = true;
            }

            private void DisplayOptions() {
                options.SetActive(true);
            }

            private void HideOptions() {
                options.SetActive(false);
            }

            public void DeleteNode() {
                string[,] fields = { { "NodeIDs", myID } };
                DbCommands.DeleteTupleInTable("DialogueNodes",
                                             fields);
                dialogueUI.HidePlayerChoices();
                Destroy(gameObject);
            }

            public void EditNode() {
                dialogueUI.SetActiveNodeEdit();
            }

            void SetMyColour(Color newColor) {
                inputBG.color = newColor;
            }

            public void UpdateNodeDisplay(string newText) {
                input.GetComponent<InputField>().text = newText;
            }
        }
    }
}