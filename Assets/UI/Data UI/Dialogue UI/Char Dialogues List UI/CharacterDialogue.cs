using UnityEngine;
using UnityEngine.UI;
using DataUI.Utilities;
using System.Collections;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class CharacterDialogue : MonoBehaviour {
            private string characterName;
            public string CharacterName {
                get { return characterName; }
                set { characterName = value; }
            }
            private string dialogueID;
            public string DialogueID {
                get { return dialogueID; }
                set { dialogueID = value; }
            }
            private string sceneName;
            public string SceneName {
                get { return sceneName; }
                set { sceneName = value; }
            }

            GameObject removeLinkBtn;
            InputField input;
            // Use this for initialization
            void Start() {
                removeLinkBtn = transform.FindChild("RemoveLink").gameObject;
                input = transform.GetComponentInChildren<InputField>();
            }
            void Update() {
                DeselectIfClickingAnotherChar();
            }

            void DeselectIfClickingAnotherChar() {
                /* if another dialogue is selected that is not this dialogue, then this dialogue should be deselected */
                if (Input.GetMouseButtonUp(0)) {
                    if (MouseSelection.IsClickedGameObjectName("CharacterDialog") && MouseSelection.IsClickedDifferentGameObjectTo(gameObject)) {
                        DeselectCharDialogue();
                    }
                }
            }

            void OnMouseUpAsButton() {
                SelectCharDialogue();
            }

            public void SelectCharDialogue() {
                DisplayRemoveLinkBtn();
                SetMyColour(Colours.colorDataUIInputSelected);
            }

            private void DeselectCharDialogue() {
                HideRemoveLinkBtn();
                SetMyColour(Color.white);
            }

            private void DisplayRemoveLinkBtn() {
                removeLinkBtn.SetActive(true);
            }

            private void HideRemoveLinkBtn() {
                removeLinkBtn.SetActive(false);
            }

            public void DeleteCharacterDialogue() {
                string[,] fields = { { "CharacterNames", characterName }, { "DialogueIDs", dialogueID } };
                DbCommands.DeleteTupleInTable("CharacterDialogues",
                                             fields);
                Destroy(gameObject);
            }

            void SetMyColour(Color newColor) {
                input.colors.normalColor.Equals(newColor);
            }
        }
    }
}