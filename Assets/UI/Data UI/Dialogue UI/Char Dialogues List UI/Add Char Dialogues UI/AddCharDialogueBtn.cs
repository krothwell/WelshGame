using UnityEngine;
using System.Collections;
using DbUtilities;

namespace DataUI {
    namespace ListItems {
        public class AddCharDialogueBtn : MonoBehaviour {
            DialogueUI dui;
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
                dui = FindObjectOfType<DialogueUI>();
            }

            public void SelectCharLink() {
                dui.SetSelectedCharLink(gameObject);
                dui.InsertNewCharLink();
                dui.DisplayCharsRelatedToDialogue();
                dui.DeactivateCharacterLink();
            }

            public void DisplayDeletionOption() {
                transform.FindChild("DeletionOption").gameObject.SetActive(true);
            }

            public void Delete() {
                string[,] fields = { { "CharacterNames", characterName }, { "Scenes", "null" } };
                DbCommands.DeleteTupleInTable("Characters", fields);
                Destroy(gameObject);
            }
        }
    }
}