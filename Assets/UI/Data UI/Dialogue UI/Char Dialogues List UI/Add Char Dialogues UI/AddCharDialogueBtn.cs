using UnityEngine;
using System.Collections;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        public class AddCharDialogueBtn : MonoBehaviour {
            DialogueUI dialogueUI;
            private string characterName;
            public string CharacterName {
                get { return characterName; }
                set { characterName = value; }
            }
            private string sceneName;
            public string SceneName {
                get { return sceneName; }
                set { sceneName = value; }
            }

            void Start() {
                dialogueUI = FindObjectOfType<DialogueUI>();
            }

            public void SelectCharLink() {
                dialogueUI.SetSelectedCharLink(gameObject);
                dialogueUI.InsertNewCharLink();
                dialogueUI.DisplayCharsRelatedToDialogue();
                dialogueUI.DeactivateCharacterLink();
            }

            public void DisplayDeletionOption() {
                transform.Find("DeletionOption").gameObject.SetActive(true);
            }

            public void Delete() {
                string[,] fields = { { "CharacterNames", characterName }, { "Scenes", "null" } };
                DbCommands.DeleteTupleInTable("Characters", fields);
                Destroy(gameObject);
            }
        }
    }
}