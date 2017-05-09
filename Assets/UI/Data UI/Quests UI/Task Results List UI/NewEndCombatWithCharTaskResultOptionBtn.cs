using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DbUtilities;
using UnityUtilities;

namespace DataUI {
    namespace ListItems {
        public class NewEndCombatWithCharTaskResultOptionBtn : MonoBehaviour {
            Text characterNameText, sceneNameText;
            QuestsUI questsUI;
            string characterName, scene;
            void Start() {
                questsUI = FindObjectOfType<QuestsUI>();
            }
            public void InitialiseMe(string charName, string sceneName) {
                characterNameText = transform.FindChild("CharName").GetComponent<Text>();
                sceneNameText = transform.FindChild("SceneName").GetComponent<Text>();
                characterNameText.text = charName;
                sceneNameText.text = sceneName;
                scene = sceneName;
                characterName = charName;
            }

            void OnMouseUpAsButton() {
                Debugging.PrintDbTable("QuestTaskEndCombatWithCharResults");
                string resultID = DbCommands.GenerateUniqueID("QuestTaskResults", "ResultIDs", "ResultID");
                questsUI.InsertTaskResult(resultID);
                string selectedTaskID = (questsUI.GetSelectedItemFromGroup(questsUI.selectedTask) as Task).MyID;
                DbCommands.InsertTupleToTable("QuestTaskEndCombatWithCharResults",
                                    resultID,
                                    selectedTaskID,
                                    characterName,
                                    scene
                                 );
                questsUI.DisplayResultsRelatedToTaskCompletion(selectedTaskID);
                questsUI.HideNewTaskResultPanel();
            }
        }
    }
}
